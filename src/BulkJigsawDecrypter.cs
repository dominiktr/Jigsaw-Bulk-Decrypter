using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

class BulkJigsawDecrypter {
    static readonly byte[] KEY = new byte[] {
        0x3A, 0x82, 0x2C, 0x03, 0x0C, 0x05, 0xDF, 0x67,
        0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0D
    };

    static readonly byte[] IV = new byte[] {
        0x00, 0x01, 0x00, 0x03, 0x05, 0x03, 0x00, 0x01,
        0x00, 0x00, 0x02, 0x00, 0x06, 0x07, 0x06, 0x00
    };

    static int successCount = 0;
    static int errorCount = 0;
    static int skippedCount = 0;

    static void Main() {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("=== JIGSAW BULK DECRYPTER ===");
        Console.ResetColor();

        Console.WriteLine("Enter the path to scan (e.g., C:\\Users\\Admin\\Documents):");
        string rootPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(rootPath)) {
            Console.WriteLine("No path entered. Exiting.");
        }

        if (!Directory.Exists(rootPath)) {
            Console.WriteLine("Error: Directory does not exist!");
            return;
        }

        Console.WriteLine("Starting scan: " + rootPath);
        Console.WriteLine("Decryption of .zemblax files in progress...");

        ProcessDirectory(rootPath);

        Console.WriteLine("\n------------------------------------------------");
        Console.WriteLine("Finished.");
        Console.WriteLine("Decrypted (created copies): " + successCount);
        Console.WriteLine("Skipped (file exists): " + skippedCount);
        Console.WriteLine("Errors: " + errorCount);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static void ProcessDirectory(string targetDirectory) {
        try {
            string[] files = Directory.GetFiles(targetDirectory, "*.zemblax");
            foreach (string file in files) {
                DecryptSingleFile(file);
            }

            string[] subDirectories = Directory.GetDirectories(targetDirectory);
            foreach (string subDir in subDirectories) {
                ProcessDirectory(subDir);
            }
        } catch (UnauthorizedAccessException) {
            Console.WriteLine("[ACCESS DENIED] Skipping folder: " +targetDirectory);
        } catch (Exception ex) {
            Console.WriteLine("[ERROR] Scanning folder" + targetDirectory+":"+ex.Message);
        }
    }

    static void DecryptSingleFile(string inputFile) {
        string outputFile = inputFile.Substring(0, inputFile.Length - ".zemblax".Length);

        if (File.Exists(outputFile)) {
             outputFile += ".decrypted_copy";
        }
        
        if (File.Exists(outputFile)) {
            Console.WriteLine("[SKIPPED] Target exists: " +Path.GetFileName(outputFile));
            skippedCount++;
            return;
        }

        try {
            using (Aes aes = Aes.Create()) {
                aes.Key = KEY;
                aes.IV = IV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                using (FileStream fsOut = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                using (CryptoStream cs = new CryptoStream(fsIn, decryptor, CryptoStreamMode.Read)) {
                    cs.CopyTo(fsOut);
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[DECRYPTED] "+ Path.GetFileName(inputFile)+" -> "+Path.GetFileName(outputFile));
            Console.ResetColor();
            successCount++;

        } catch (CryptographicException) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[BAD KEY/FAIL] "+ Path.GetFileName(inputFile));
            Console.ResetColor();
            errorCount++;
            if (File.Exists(outputFile)) File.Delete(outputFile);
        } catch (Exception ex) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[IO ERROR] "+Path.GetFileName(inputFile)+":"+ ex.Message);
            Console.ResetColor();
            errorCount++;
            if (File.Exists(outputFile)) File.Delete(outputFile);
        }
    }
}

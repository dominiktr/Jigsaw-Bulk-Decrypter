# Jigsaw Bulk Decrypter

An automated decryptor designed for a specific variant of the Jigsaw ransomware (file extension `.zemblax`).

## Description

This tool provides bulk decryption for files compromised by a specific Jigsaw ransomware variant, distinguished by the `.zemblax` extension appended to encrypted files. The program automatically scans a target directory or drive and restores the original data by creating a decrypted copy of each file.

***

## Safety Notice & Responsible Disclosure

*   This tool is intended for educational and research purposes or as a last-resort recovery method.
*   The author does not bear any responsibility for unlawful, unethical, or otherwise improper use of this tool. Each user is solely responsible for ensuring their actions comply with all applicable laws and regulations.
*   The software is provided "as is". The author takes no responsibility for any data loss or damage caused by the use of this tool. Use at your own risk.
*   The program does not guarantee recovery of any files - effectiveness depends on the ransomware version.
*   Do not attempt to download or execute the associated malware sample unless you are in a secure, isolated sandbox environment.
***

## Technical Analysis & Target Variant

This tool is **hardcoded** to work ONLY with the Jigsaw variant matching the following characteristics:

* **Sample SHA256:** `df049efbfa7ac0b76c8daff5d792c550c7a7a24f6e9e887d01a01013c9caa763`
* **Ransom Extension:** `.zemblax`
* **Encryption Algorithm:** AES-CBC with PKCS7 padding.
* **Malware Characteristics:**
    * Obfuscated with .NET Reactor.
    * Employs anti-debugging and dynamic code injection techniques.
    * Displays a fake ".NET Framework Initialization Error" upon execution.
    * Creates persistence via `drpbx.exe` and `firefox.exe`.

> **Note:** Other Jigsaw variants use different encryption keys. This tool will NOT work for them unless you recompile it with the correct Key/IV found in your sample.
***

## Requirements

*   Windows with .NET Framework 3.5, 4.0, 4.5, or newer (installed by default on most Windows systems)
*   Command Prompt access (cmd.exe)
*   Read/write permissions for the selected directories

***

## Installation
Since this is a simple C# console application, you don't need Visual Studio. You can compile it using the native Windows CSC compiler.
1.  Clone the repository:
    ```
    git clone https://github.com/dominiktr/Jigsaw-Bulk-Decrypter.git
    cd Jigsaw-Bulk-Decrypter/src
    ```
    
2.  Build the project:
    ```
    C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe BulkJigsawDecrypter.cs
    ```
    > If you have a different version of .NET Framework, adjust the path accordingly.

***

## Usage

1.  Run the program as administrator:
    ```
    BulkJigsawDecrypter.exe
    ```

2.  Enter the path to the directory you wish to scan (e.g., `C:\Users\Admin`).  

***

## Example Output
<img width="1100" height="361" alt="Example output" src="https://github.com/user-attachments/assets/2de6e878-ca71-41f3-8e29-32fdcacec81c" />

***

## License

This project is released under the MIT License. See the LICENSE file for details.

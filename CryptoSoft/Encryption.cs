using System;
using System.IO;
using System.Text;

namespace CryptoSoft {
    public class Encryption {
        private readonly string textToCrypt, pathCryptedFile, pathSourceFile;
        private string cryptedText;
        private readonly int encryptionKey;

        private string GetCryptedText() {
            return cryptedText;
        }

        private string GettextToCrypt() {
            return textToCrypt;
        }

        public string GetTargetDirectory() {
            return pathCryptedFile;
        }

        public Encryption(string sourceFile, string key) {
            textToCrypt = File.ReadAllText(sourceFile);
            pathSourceFile = sourceFile;
            pathCryptedFile = sourceFile.Substring(0, sourceFile.IndexOf(".")) + ".crypt";
            encryptionKey = Int32.Parse(key);
            EncryptWithXOR();
            ReplaceClearFile();
        }

        public void EncryptWithXOR() {
            StringBuilder inputText = new(GettextToCrypt());
            StringBuilder outputText = new(GettextToCrypt().Length);
            for (int loop = 0; loop < GettextToCrypt().Length; loop++) {
                outputText.Append((char)(inputText[loop] ^ encryptionKey));
            }
            cryptedText = outputText.ToString();
        }

        public void ReplaceClearFile() {
            File.Delete(pathSourceFile);
            File.WriteAllText(GetTargetDirectory(), GetCryptedText());
        }
    }
}

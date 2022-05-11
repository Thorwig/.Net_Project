namespace CryptoSoft {
    class Program {
        static void Main(string[] args) {
            // structure of string[] args : 
            // 
            // args[0] -> (string) path + name of the file to encrypt
            // args[1] -> (int) XOR encryption key
            new Encryption(args[0], args[1]);
        }
    }
}

// See https://aka.ms/new-console-template for more information

using System.Security.Cryptography;

string filePath = "path/to/your/file.txt";
using var sha256 = SHA256.Create();
using var stream = File.OpenRead(filePath);

byte[] hashBytes = sha256.ComputeHash(stream);
string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

Console.WriteLine($"SHA256: {hash}");
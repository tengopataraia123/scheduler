using System;
using System.Security.Cryptography;
using System.Text;
using MainServer.Application.Services.Programs.Contracts;

namespace MainServer.Application.Services.Programs.Access
{
    public class CodeGenerator : ICodeGenerator
    {
        private int _id;

        public CodeGenerator()
        {
            _id = 0;
        }
        public string GenerateCodeForProgram(string name, string url)
        {
            _id++;

            string rawData = name + url + _id.ToString();

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(rawData);
                byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                string hash = Convert.ToBase64String(hashBytes);

                hash = hash.Length > 8 ? hash.Substring(0, 8) : hash;

                return "ID" + _id.ToString() + "_key" + hash;
            }
        }
    }
}
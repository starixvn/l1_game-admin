using GamesAdmin.Site._Shared.Configurations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GamesAdmin.Site.Features._Shared
{
    public class HomeController : Controller
    {
        private readonly IGameControlPanelSettings panelSettings;
        public HomeController(IGameControlPanelSettings panelSettings) 
        {
            this.panelSettings = panelSettings;
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RedirectToPanel()
        {
            //var userPass = $"admin-123456-{DateTime.UtcNow.ToString("HH")}";
            //var userHash = Encrypt(userPass);

            //var hashed = BCrypt.Net.BCrypt.HashPassword(panelSettings.SecretKey, BCrypt.Net.BCrypt.GenerateSalt(12));

            //return Redirect($"{panelSettings.Host}?token={hashed}-{userHash}");

            return Redirect($"{panelSettings.Host}");
        }

        private static string Encrypt(string text)
        {
            var key = "!Qqs2SRXWER533FV"; 
            var iv = "5TGBaYHOID5egIKA"; 
            // AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.IV = Encoding.UTF8.GetBytes(iv);
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Convert string to byte array
            byte[] src = Encoding.Unicode.GetBytes(text);

            // encryption
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                // Convert byte array to Base64 strings
                return Convert.ToBase64String(dest);
            }
        }
    }
}
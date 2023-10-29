using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.PasswordCreateServices
{
    public class PasswordCreateService:IPasswordCreateService
    {
        public Task<string> GeneratePasswordAsync(int passwordLength)
        {
            Random random = new Random();
            const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            const string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string punctuation = "!@#$%^&*()-_=+[]{}|;:'\",.<>?";

            // Şifre uzunluğunu belirle (min 8 karakter)
           // Örneğin, 8 ila 16 karakter arasında bir uzunluk

            // Her bir karakter setinden en az bir karakteri kullanmaya zorla
            char[] password = new char[passwordLength];
            password[0] = lowercaseLetters[random.Next(lowercaseLetters.Length)];
            password[1] = uppercaseLetters[random.Next(uppercaseLetters.Length)];
            password[2] = digits[random.Next(digits.Length)];
            password[3] = punctuation[random.Next(punctuation.Length)];

            // Kalan karakterleri rastgele seçerek şifreyi oluşturun
            string allCharacters = lowercaseLetters + uppercaseLetters + digits + punctuation;
            for (int i = 4; i < passwordLength; i++)
            {
                password[i] = allCharacters[random.Next(allCharacters.Length)];
            }

            // Oluşturulan şifreyi karıştırarak rastgele sırala
            password = password.OrderBy(c => random.Next()).ToArray();

            return Task.FromResult(new string(password));
        }

    }
}

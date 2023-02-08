using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Quiz.Direction;

namespace Quiz
{
    internal class User
    {
        public string? Name { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? BirthDate { get; set; }
        List<UserTop20> archive = new List<UserTop20>();

        public User(string? nickName, string? login, string? password, string? birthDate)
        {
            Name = nickName;
            Login = login;
            Password = password;
            BirthDate = birthDate;
        }
        public void SaveResults(string nameDirection, int points)
        {
            archive.Add(new nameDirection, points);
        }
        
        public void PrintArchiveResults()
        {
            foreach (var result in archive)
                Console.WriteLine($"Викторина: {result.Key}, Количество набранных баллов: {result.Value}");
        }
    }
}

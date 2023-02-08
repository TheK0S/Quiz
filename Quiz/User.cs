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
            archive.Add(new UserTop20(nameDirection, points));
        }
        
        public void SortArchive()
        {
            UserTop20 temp;
            for (int i = 0; i < archive.Count; i++)
            {
                for (int j = 0; j < archive.Count; j++)
                {
                    if (archive[i].value < archive[j].value)
                    {
                        temp = archive[i];
                        archive[i] = archive[j];
                        archive[j] = temp;
                    }
                }
            }
        }

        public void PrintArchiveResults()
        {
            foreach (var result in archive)
                Console.WriteLine($"Викторина: {result.name}, Количество набранных баллов: {result.value}");
        }
    }
}

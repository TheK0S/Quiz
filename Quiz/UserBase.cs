using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Quiz
{
    internal class UserBase
    {
        List<User> users;
        public UserBase(List<User> users)
        {
            this.users = users;
        }
        public bool Authorization(string userLogin, string userPassword)
        {
            if(users == null || users.Count == 0)
                return false;

            if(userLogin == null || userPassword == null)
                return false;

            foreach (var item in users)
            {
                if (item.Login == userLogin)
                    if (item.Password == userPassword)
                        return true;
            }

            return false;
        }
        public bool Registration(string userLogin, string userPassword, string nickName, string birthDate)
        {
            if (userLogin == null || userLogin.Length < 3 || userPassword == null || userPassword.Length < 3)
                return false;
            if (LoginUniqueCheck(userLogin))
            {
                users.Add(new User(nickName, userLogin, userPassword, birthDate));
                return true;
            }
            else
                return false;            
        }
        
        public bool LoginUniqueCheck(string userLogin)
        {
            if (userLogin == null || userLogin.Length < 3)
                return false;

            foreach (var item in users)
            {
                if (item.Login == userLogin)
                    return false;
            }

            return true;
        }

        public void PrintUserArchive(int index) => users[index].PrintArchiveResults();
    }
}

using Laba_4_Aznabaev_Nadir_BPI_23_01.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_4_Aznabaev_Nadir_BPI_23_01.Model
{
    public class Person
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; }

        public Person() { }

        public Person(int id, int roleId, string firstName, string lastName, string birthday)
        {
            Id = id;
            RoleId = roleId;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }

        public Person CopyFromPersonDPO(PersonDpo personDpo)
        {
            RoleViewModel vmRole = RoleViewModel.Instance;
            int roleId = 0;

            foreach (var r in vmRole.ListRole)
            {
                if (r.NameRole == personDpo.RoleName)
                {
                    roleId = r.Id;
                    break;
                }
            }

            return new Person
            {
                Id = personDpo.Id,
                RoleId = roleId,
                FirstName = personDpo.FirstName,
                LastName = personDpo.LastName,
                Birthday = personDpo.Birthday
            };
        }


    }

}

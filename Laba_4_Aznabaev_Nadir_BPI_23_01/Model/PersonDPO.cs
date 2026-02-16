using JetBrains.Annotations;
using Laba_4_Aznabaev_Nadir_BPI_23_01.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Laba_4_Aznabaev_Nadir_BPI_23_01.Model
{
    public class PersonDpo : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int _roleId;
        public int RoleId
        {
            get { return _roleId; }
            set
            {
                _roleId = value;
                OnPropertyChanged("RoleId");
            }
        }


        private string _roleName;

        public string RoleName
        {
            get { return _roleName; }
            set
            {
                _roleName = value;
                OnPropertyChanged("RoleName");
            }

        }
        public string Role { get; set; }
        private string firstName;
        public string FirstName {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        private string lastName;
        public string LastName {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        private string birthday;
        public string Birthday {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        public PersonDpo() { }

        public PersonDpo(int id, string roleName, string firstName, string lastName, string birthday)
        {
            Id = id;
           // Role = role;
            RoleName = roleName;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }


        public PersonDpo ShallowCopy()
        {
            return (PersonDpo)this.MemberwiseClone();
        }

        public PersonDpo CopyFromPerson(Person person)
        {
            PersonDpo perDpo = new PersonDpo(); 
            RoleViewModel vmRole = RoleViewModel.Instance; 
            string role = string.Empty;
            foreach (var r in vmRole.ListRole)
            {
                if (r.Id == person.RoleId)
                {
                    role = r.NameRole; 
                    break;
                }
            }
            if (role != string.Empty)
            {
                perDpo.Id = person.Id; 
                perDpo.RoleName = role; 
                perDpo.FirstName = person.FirstName;
                perDpo.LastName = person.LastName;
                perDpo.Birthday = person.Birthday;
            }
            return perDpo;
        }
        public event PropertyChangedEventHandler PropertyChanged; [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private object selectedRole;

        public object SelectedRole { get => selectedRole; set => SetProperty(ref selectedRole, value); }
    }

}



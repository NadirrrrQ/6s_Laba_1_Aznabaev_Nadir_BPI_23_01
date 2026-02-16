using Laba_4_Aznabaev_Nadir_BPI_23_01.Helper;
using Laba_4_Aznabaev_Nadir_BPI_23_01.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
namespace Laba_4_Aznabaev_Nadir_BPI_23_01.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged

    {
        readonly string path = @"..\..\DataModels\RoleData.json";
        public ObservableCollection<Role> ListRole { get; set; } = new ObservableCollection<Role>();
        public event PropertyChangedEventHandler PropertyChanged;

        private RelayCommand deleteRole;
        private Role selectedRole;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static RoleViewModel _instance;
        public static RoleViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RoleViewModel();
                }
                return _instance;
            }
        }
        public Role SelectedRole
        {
            get
            {
                return selectedRole;
            }
            set
            {
                selectedRole = value;
                OnPropertyChanged("SelectedRole");
                EditRole.CanExecute(true);
            }
        }


        public RoleViewModel()
        {
            ListRole = LoadRole();
            /*
            if (_instance == null) 
            {
                ListRole.Add(
                new Role
                {
                    Id = 1,
                    NameRole = "Директор"
                }
                );
                ListRole.Add(
                new Role
                {
                    Id = 2,
                    NameRole = "Бухгалтер"
                }
                );
                ListRole.Add(new Role
                {
                    Id = 3,
                    NameRole = "Менеджер"

                });
            }*/

        }
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListRole)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                };
            }
            return max;
        }
        #region command AddRole
        private RelayCommand addRole;
        public RelayCommand AddRole
        {
            get
            {
                return addRole ??
                (addRole = new RelayCommand(obj =>
                {
                    WindowNewRole wnRole = new WindowNewRole
                    {
                        Title = "Новая должность",
                    };

                    int maxIdRole = MaxId() + 1;
                    Role role = new Role { Id = maxIdRole };
                    
                    wnRole.DataContext = role;

                    if (wnRole.ShowDialog() == true)
                    {
                        ListRole.Add(role);
                        SaveChanges(ListRole);
                        SelectedRole = role;

                    }
                }));
            }

        }

        #endregion

        #region Command EditRole
        private RelayCommand editRole; 
        public RelayCommand EditRole
        {
            get
            {
                return editRole ??
                (editRole = new RelayCommand(obj =>
                {
                    WindowNewRole wnRole = new WindowNewRole
                    { 
                        Title = "Редактирование должности",
                    }; 

                    Role role = SelectedRole;
                    Role tempRole = new Role();
                    tempRole = role.ShallowCopy(); 
                    wnRole.DataContext = tempRole; 


                    if (wnRole.ShowDialog() == true)
                    {
                        role.NameRole = tempRole.NameRole;
                        SaveChanges(ListRole);
                        SelectedRole = role;

                    }
                }, (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }
        #endregion

        #region Command DeleteRole
        public RelayCommand DeleteRole
        {
            get
            {
                return deleteRole ??
                (deleteRole = new RelayCommand(obj =>
                {
                    Role role = SelectedRole;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по должности: " + role.NameRole, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        ListRole.Remove(role);
                        SaveChanges(ListRole);
                        SelectedRole = null;
                    }
                }, (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }
        #endregion


        string _jsonRoles = String.Empty;
        public string Error { get; set; }


        public ObservableCollection<Role> LoadRole()
        {
            _jsonRoles = File.ReadAllText(path);
            if (_jsonRoles != null)
            {
                ListRole = JsonConvert.DeserializeObject<ObservableCollection<Role>> (_jsonRoles);
                return ListRole;
            }
            else
            {
                return null;
            }
        }

        private void SaveChanges(ObservableCollection<Role> listRole)
        {
            var jsonRole = JsonConvert.SerializeObject(listRole);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonRole);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла /n" + e.Message;
            }
        }
    }
}

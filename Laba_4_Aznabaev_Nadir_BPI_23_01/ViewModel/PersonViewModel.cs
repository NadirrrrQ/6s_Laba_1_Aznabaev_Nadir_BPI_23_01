using JetBrains.Annotations;
using Laba_4_Aznabaev_Nadir_BPI_23_01.Helper;
using Laba_4_Aznabaev_Nadir_BPI_23_01.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace Laba_4_Aznabaev_Nadir_BPI_23_01.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged

    {
        private PersonDpo selectedPersonDpo;

        readonly string path = @"..\..\DataModels\PersonData.json";
        string _jsonPersons = String.Empty;
        public string Error { get; set; }
        public string Message { get; set; }
        public ObservableCollection<Person> LoadPerson()
        {
            _jsonPersons = File.ReadAllText(path); if (_jsonPersons != null)
            {
                ListPerson = JsonConvert.DeserializeObject<ObservableCollection<Person>>(_jsonPersons);
                return ListPerson;
            }
            else
            {
                return null;
            }
        }

        private void SaveChanges(ObservableCollection<Person> listPersons)
        {
            var jsonPerson = JsonConvert.SerializeObject(listPersons); try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonPerson);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла /n" + e.Message;
            }
        }


        public PersonDpo SelectedPersonDpo
        {
            get { return selectedPersonDpo; }
            set
            {
                selectedPersonDpo = value; OnPropertyChanged("SelectedPersonDpo");
            }

        }

        private static PersonViewModel _instance;
        public static PersonViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PersonViewModel();
                }
                return _instance;
            }
        }
        public ObservableCollection<Person> ListPerson { get; set; } = new ObservableCollection<Person>();

        public ObservableCollection<PersonDpo> ListPersonDpo { get; set; } = new ObservableCollection<PersonDpo>();
        public PersonViewModel()
        {
            ListPerson = new ObservableCollection<Person>();
            ListPersonDpo = new ObservableCollection<PersonDpo>();
            ListPerson = LoadPerson();
            ListPersonDpo = GetListPersonDpo();

        }
        public ObservableCollection<PersonDpo> GetListPersonDpo()
        {
            foreach (var person in ListPerson)
            {
                PersonDpo p = new PersonDpo(); 
                p = p.CopyFromPerson(person);
                ListPersonDpo.Add(p);
            }
            return ListPersonDpo;
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListPerson)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                };
            }
            return max;
        }
        private RelayCommand addPerson;
        public RelayCommand AddPerson
        {
            get
            {
                return addPerson ?? (addPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee
                    {
                        Title = "Новый сотрудник"
                    };
                    {

                        int maxIdPerson = MaxId() + 1;
                        PersonDpo per = new PersonDpo
                        {
                            Id = maxIdPerson,
                            Birthday = DateTime.Now.ToString(),
                        };
                        wnPerson.DataContext = per;


                        if (wnPerson.ShowDialog() == true)
                        {

                            Role r = wnPerson.SelectedRole;
                            per.RoleName = r.NameRole;
                            ListPersonDpo.Add(per);

                            Person p = new Person();
                            p = p.CopyFromPersonDPO(per);
                            ListPerson.Add(p);
                            try
                            {
                                SaveChanges(ListPerson);
                            }
                            catch (Exception e)
                            {
                                Error = "Ошибка добавления данных в json файл\n" +
                                e.Message;
                            }
                        }
                    }
                }, (obj) => true));
            }
            
        }
        private RelayCommand editPerson;
        public RelayCommand EditPerson
        {
            get
            {
                return editPerson ??
                (editPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee()
                    {
                        Title = "Редактирование данных сотрудника",
                    };
                    PersonDpo personDpo = SelectedPersonDpo;
                    PersonDpo tempPerson = new PersonDpo();
                    tempPerson = personDpo.ShallowCopy();
                    wnPerson.DataContext = tempPerson;


                    // wnPerson.CbRole.ItemsSource = new ListRole();
                    if (wnPerson.ShowDialog() == true)
                    {
                        Role r = wnPerson.SelectedRole;
                        personDpo.RoleName = r.NameRole;
                        personDpo.FirstName = tempPerson.FirstName;
                        personDpo.LastName = tempPerson.LastName;
                        personDpo.Birthday = tempPerson.Birthday;





                        FindPerson finder = new FindPerson(personDpo.Id);
                        List<Person> listPerson = ListPerson.ToList();
                        Person p = listPerson.Find(new Predicate<Person>(finder.PersonPredicate));
                        p = p.CopyFromPersonDPO(personDpo);
                        try
                        {
                            SaveChanges(ListPerson);
                        }
                        catch (Exception e)
                        {


                            Error = "Ошибка редактирования данных в json файл\n"
                            + e.Message;
                        }
                    }
                
                }, (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }
        private RelayCommand deletePerson; 
        public RelayCommand DeletePerson
        {
            get
            {
                return deletePerson ??
                (deletePerson = new RelayCommand(obj =>
                {
                    PersonDpo person = SelectedPersonDpo;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по сотруднику: \n" + person.LastName + " " + person.FirstName,
    "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {


                        ListPersonDpo.Remove(person);
                        Person per = new Person();
                        per = per.CopyFromPersonDPO(person);
                        ListPerson.Remove(per);
                        SaveChanges(ListPerson);
                    }

                }, (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged; [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}





using Laba_4_Aznabaev_Nadir_BPI_23_01.Model;
using Laba_4_Aznabaev_Nadir_BPI_23_01.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Laba_4_Aznabaev_Nadir_BPI_23_01
{
    /// <summary>
    /// Логика взаимодействия для WindowNewEmployee.xaml
    /// </summary>
    public partial class WindowNewEmployee : Window
    {


        public Role SelectedRole => CbRole.SelectedItem as Role;

        public WindowNewEmployee()
        {
            InitializeComponent();

            RoleViewModel vmRole = RoleViewModel.Instance;
            CbRole.ItemsSource = vmRole.ListRole;
        }


        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var person = DataContext as PersonDpo;

            if (person == null || string.IsNullOrWhiteSpace(person?.FirstName) ||
        string.IsNullOrWhiteSpace(person?.LastName) ||
        string.IsNullOrWhiteSpace(person?.RoleName))
            {

                MessageBox.Show("Заполните все поля!");
                return;
            }

            DialogResult = true;
            Close();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}

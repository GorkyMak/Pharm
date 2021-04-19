using Pharm.Database;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListUsers.xaml
    /// </summary>
    public partial class ListUsers : Page
    {
        List<Пользователи> Users;
        public ListUsers()
        {
            InitializeComponent();
            GetEmployees();
            DGUsers.ItemsSource = Users;
        }
        private void GetEmployees()
        {
            Users = new List<Пользователи>();
            using (var dbcontext = new АптекаEntities())
            {
                Users = dbcontext.Пользователи.ToList();
            }
        }
    }
}

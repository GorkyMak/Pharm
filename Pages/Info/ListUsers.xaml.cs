using Pharm.Classes;
using Pharm.Database;
using Pharm.Pages.Edit;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Pharm.Pages
{
    public partial class ListUsers : Page
    {
        List<Пользователи> users;
        public ListUsers()
        {
            InitializeComponent();
            RefreshDG();
        }

        private void RefreshDG()
        {
            GetUsers();
            SetUsers();
        }

        private void SetUsers()
        {
            DGUsers.ItemsSource = users;
        }

        private void GetUsers()
        {
            using (var dbcontext = new АптекаEntities())
                users = new List<Пользователи>(dbcontext.Пользователи);
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditUsers(DGUsers.SelectedItem as Пользователи));
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditUsers());
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (var dbcontext = new АптекаEntities())
            {
                var record = DGUsers.SelectedItem as Пользователи;
                dbcontext.Entry(record).State = System.Data.Entity.EntityState.Deleted;

                dbcontext.Пользователи.Remove(record);
                dbcontext.SaveChanges();
            }

            RefreshDG();
        }

        private void BtnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!DataChecker.CheckFieldSearch(SpSearch.Children))
            {
                DGUsers.ItemsSource = users;
                return;
            }

            var searchResult = new List<Пользователи>();
            string searchText = "";

            foreach (var item in SpSearch.Children)
            {
                if (item is CheckBox checkBox && (bool)checkBox.IsChecked)
                {
                    searchResult = searchResult.Union(users
                                               .Where(i => i.Роль == checkBox.Content.ToString()))
                                               .ToList();
                }
                else if (item is TextBox textBox && textBox.Text != "")
                {
                    searchText = textBox.Text.ToLower();
                }
            }

            searchResult = searchResult.Where(i => i.Логин.ToLower().Contains(searchText) ||
                i.Пароль.ToLower().Contains(searchText)).ToList();

            DGUsers.ItemsSource = searchResult;
        }
    }
}
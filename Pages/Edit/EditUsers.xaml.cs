using Pharm.Classes;
using Pharm.Database;
using System.Windows;
using System.Windows.Controls;

namespace Pharm.Pages.Edit
{
    public partial class EditUsers : Page
    {
        Пользователи пользователи;
        public EditUsers()
        {
            InitializeComponent();
            DataContext = new Пользователи();
            BtnAddEdit.Content = "Добавить";
        }

        public EditUsers(Пользователи пользователи)
        {
            InitializeComponent();
            this.пользователи = пользователи;
            DataContext = this.пользователи;
            tb2.Password = пользователи.Пароль;
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!DataChecker.CheckFields(sp.Children))
                return;

            using (var dbcontext = new АптекаEntities())
            {
                if (пользователи == null)
                {
                    пользователи = (Пользователи)DataContext;
                    dbcontext.Entry(пользователи).State = System.Data.Entity.EntityState.Added;
                }
                else
                    dbcontext.Entry(пользователи).State = System.Data.Entity.EntityState.Modified;

                пользователи.Пароль = tb2.Password;

                dbcontext.SaveChanges();
            }

            NavigationService.GoBack();
        }
    }
}
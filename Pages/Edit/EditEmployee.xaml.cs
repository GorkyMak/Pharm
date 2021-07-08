using Pharm.Classes;
using Pharm.Database;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Pharm.Pages.Edit
{
    public partial class EditEmployee : Page
    {
        Сотрудник employees;
        public EditEmployee()
        {
            InitializeComponent();
            GetCmbValues();
            DataContext = new Сотрудник()
            {
                Дата_приёма_на_работу = new System.DateTime(1970, 01, 01),
                Контактные_данные = new Контактные_данные()
                {
                    Адрес = new Адрес()
                },
                Личные_данные = new Личные_данные()
                {
                    Дата_рождения = new System.DateTime(1970, 01, 01)
                },
                Пользователи = new Пользователи()
            };
        }

        private void GetCmbValues()
        {
            using (var dbcontext = new АптекаEntities())
                Cmb11.ItemsSource = dbcontext.Должность.Select(i => i.Название_должности).ToList();
        }

        public EditEmployee(Сотрудник employees)
        {
            InitializeComponent();
            GetCmbValues();
            DataContext = employees;
            this.employees = employees;
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            Regex CorrectDateTime = new Regex(@"^[+]\d[(]\d{3}[)]\d{3}-\d{2}-\d{2}$");

            if (!CorrectDateTime.IsMatch(tb12.Text))
            {
                MessageBox.Show("Неверный формат телефона", "Ошибка");
                return;
            }

            if (!DataChecker.CheckFieldsExcept(sp.Children, tb11.Name))
                return;

            foreach (var item in tb41.Text)
            {
                if(!char.IsDigit(item))
                {
                    MessageBox.Show("В серии должны быть только цифры", "Ошибка");
                    return;
                }
            }

            foreach (var item in tb42.Text)
            {
                if(!char.IsDigit(item))
                {
                    MessageBox.Show("В номере должны быть только цифры", "Ошибка");
                    return;
                }
            }

            if (tb41.Text.Length != 4 || tb42.Text.Length != 6)
            {
                MessageBox.Show("В серии должны быть 4 цифры, а в номере 6", "Ошибка");
                return;
            }

            using (var dbcontext = new АптекаEntities())
            {
                if (employees == null)
                {
                    employees = (Сотрудник)DataContext;
                    dbcontext.Entry(employees).State = EntityState.Added;
                }
                else
                    dbcontext.Entry(employees).State =
                    dbcontext.Entry(employees.Контактные_данные).State =
                    dbcontext.Entry(employees.Контактные_данные.Адрес).State =
                    dbcontext.Entry(employees.Личные_данные).State =
                    dbcontext.Entry(employees.Пользователи).State =
                        EntityState.Modified;

                var post = dbcontext.Должность.FirstOrDefault(i => i.Название_должности == Cmb11.Text);

                employees.Должность = post;
                employees.Код_должности = post.Код_должности;


                dbcontext.SaveChanges();
            }

            NavigationService.GoBack();
        }
    }
}
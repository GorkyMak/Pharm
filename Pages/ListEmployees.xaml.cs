using Pharm.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListEmployees.xaml
    /// </summary>
    public partial class ListEmployees : Page
    {
        List<Employees> Employees;
        public ListEmployees()
        {
            InitializeComponent();

            GetEmployees();
            DGEmpl.ItemsSource = Employees;
        }

        private void GetEmployees()
        {
            Employees = new List<Employees>();
            using (var dbcontext = new АптекаEntities())
            {
                var temp = dbcontext.Сотрудник.ToList();
                foreach (var item in temp)
                {
                    Employees.Add(new Employees() 
                    {
                        Код_сотрудника = item.Код_сотрудника,
                        Название_должности = item.Должность.Название_должности,
                        Оклад__ = item.Должность.Оклад__,
                        ФИО = item.Личные_данные.Фамилия + " " + item.Личные_данные.Имя[0] + "." + item.Личные_данные.Отчество[0] + ".",
                        Серия_и_номер_паспорта = item.Личные_данные.Серия_паспорта + " " + item.Личные_данные.Номер_паспорта,
                        Дата_рождения = item.Личные_данные.Дата_рождения,
                        Образование = item.Личные_данные.Образование,
                        Субъект = item.Контактные_данные.Адрес.Субъект,
                        Город = item.Контактные_данные.Адрес.Город,
                        Улица = item.Контактные_данные.Адрес.Улица,
                        Дом = item.Контактные_данные.Адрес.Дом,
                        Квартира = item.Контактные_данные.Адрес.Квартира,
                        Номер_телефона = item.Контактные_данные.Номер_телефона,
                        Сайт = item.Контактные_данные.Сайт,
                        Электронная_почта = item.Контактные_данные.Электронная_почта,
                        Логин = item.Пользователи.Логин,
                        Пароль = item.Пользователи.Пароль,
                        Роль = item.Пользователи.Роль,
                        Дата_приёма_на_работу = item.Дата_приёма_на_работу
                    });
                }
            }
        }
    }

    class Employees
    {
        public int Код_сотрудника { get; set; }
        public string Название_должности { get; set; }
        public decimal Оклад__ { get; set; }
        public string ФИО { get; set; }
        public string Серия_и_номер_паспорта { get; set; }
        public DateTime Дата_рождения { get; set; }
        public string Образование { get; set; }
        public string Субъект { get; set; }
        public string Город { get; set; }
        public string Улица { get; set; }
        public string Дом { get; set; }
        public string Квартира { get; set; }
        public string Номер_телефона { get; set; }
        public string Сайт { get; set; }
        public string Электронная_почта { get; set; }
        public string Логин { get; set; }
        public string Пароль { get; set; }
        public string Роль { get; set; }
        public DateTime Дата_приёма_на_работу { get; set; }
    }
}

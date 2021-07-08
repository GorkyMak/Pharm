using Pharm.Database;
using Pharm.Pages.Edit;
using System.Data.Entity;
using System.Collections.Generic;
using System.Windows.Controls;
using Pharm.Classes;
using System.Linq;
using System.Windows;

namespace Pharm.Pages
{
    public partial class ListEmployees : Page
    {
        List<Сотрудник> employees;
        public ListEmployees()
        {
            InitializeComponent();
            RefreshDG();
        }

        private void RefreshDG()
        {
            GetEmployees();
            SetEmployees();
        }

        private void SetEmployees()
        {
            DGEmpl.ItemsSource = employees;
        }

        private void GetEmployees()
        {
            using (var dbcontext = new АптекаEntities())
                employees = new List<Сотрудник>(dbcontext.Сотрудник
                    .Include(i => i.Личные_данные)
                    .Include(i => i.Контактные_данные.Адрес)
                    .Include(i => i.Должность)
                    .Include(i => i.Пользователи));
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditEmployee(DGEmpl.SelectedItem as Сотрудник));
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditEmployee());
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var record = DGEmpl.SelectedItem as Сотрудник;
            using (var dbcontext = new АптекаEntities())
            {
                if(dbcontext.Заказ.Any(i => i.Код_сотрудника == record.Код_сотрудника))
                {
                    MessageBox.Show("Данного сотрудника нельзя удалить, так как он участвует в заказах","Ошибка");
                    return;
                }

                dbcontext.Entry(record).State = EntityState.Deleted;

                dbcontext.SaveChanges();
            }

            RefreshDG();
        }

        private void BtnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            List<Сотрудник> searchResult = employees, filterEducation = new List<Сотрудник>();
            string searchText = TbSearchText.Text.ToLower();

            switch (CmbFilterDate.Text)
            {
                case "приёма на работу":
                    searchResult = searchResult.Where(i => i.Дата_приёма_на_работу >= DtPStartDate.SelectedDate && i.Дата_приёма_на_работу <= DtPEndDate.SelectedDate).ToList();
                    break;

                case "рождения":
                    searchResult = searchResult.Where(i => i.Личные_данные.Дата_рождения >= DtPStartDate.SelectedDate && i.Личные_данные.Дата_рождения <= DtPEndDate.SelectedDate).ToList();
                    break;
            }

            if (CmbFilterPost.Text != "" && CmbFilterPost.Text != "Любая")
                searchResult = searchResult.Where(i => i.Должность.Название_должности == CmbFilterPost.Text).ToList();

            searchResult = searchResult.Intersect(filterEducation, new EmployeesComparer()).ToList();

            switch (CmbFilterMedication.Text)
            {
                case "по коду сотрудника":
                    searchResult = searchResult.Where(i => i.Код_сотрудника.ToString().Contains(searchText)).ToList();
                    break;

                case "по ФИО сотрудника":
                    searchResult = searchResult.Where(i => i.Личные_данные.ToString().ToLower().Contains(searchText)).ToList();
                    break;

                case "по серии и номеру паспорта":
                    searchResult = searchResult.Where(i => $"{i.Личные_данные.Серия_паспорта} {i.Личные_данные.Номер_паспорта}".Contains(searchText)).ToList();
                    break;

                case "по адресу":
                    searchResult = searchResult.Where(i => 
                        i.Контактные_данные.Адрес.ToString().ToLower().Contains(searchText)).ToList();
                    break;

                case "по номеру телефона":
                    searchResult = searchResult.Where(i => i.Контактные_данные.Номер_телефона.Contains(searchText)).ToList();
                    break;

                case "по логину":
                    searchResult = searchResult.Where(i => i.Пользователи.Логин.ToLower().Contains(searchText)).ToList();
                    break;
            }

            DGEmpl.ItemsSource = searchResult;
        }
    }
}
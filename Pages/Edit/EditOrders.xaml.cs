using Pharm.Classes;
using Pharm.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Pharm.Pages.Edit
{
    public partial class EditOrders : Page
    {
        Заказ заказ;
        Заказ_Препарат заказ_Препарат;
        bool addOrd = false, addOrdMed = false;
        public EditOrders()
        {
            InitializeComponent();
            BtnAddEditDel.Content = "Добавить";

            заказ = new Заказ()
            {
                Социальная_карта = new Социальная_карта() 
                {
                    Личные_данные = new Личные_данные(),
                    Срок_действия = DateTime.Today
                },
                Заказ_Препарат = new List<Заказ_Препарат>(),
                Дата_и_время_исполнения = DateTime.Now
            };

            addOrd = true;
            GetEmployee();
            DataContext = заказ;

            tb8.TextChanged += tb8_TextChanged;
        }

        private void GetEmployee()
        {
            using (var dbcontext = new АптекаEntities())
            {
                var userCode = dbcontext.Пользователи
                    .AsNoTracking()
                    .First(j => j.Логин == Properties.Settings.Default.UserLogin).Код_пользователя;

                заказ.Сотрудник = dbcontext.Сотрудник.AsNoTracking().First(i => i.Код_пользователя == userCode);
                заказ.Код_сотрудника = заказ.Сотрудник.Код_сотрудника;
            }
        }

        public EditOrders(Заказ заказ)
        {
            InitializeComponent();
            this.заказ = заказ;
            DataContext = this.заказ;

            if (заказ.Социальная_карта != null)
                ChBSocCard.IsChecked = true;

            tb8.TextChanged += tb8_TextChanged;
        }

        private void BtnAddEditDel_Click(object sender, RoutedEventArgs e)
        {
            Regex CorrectDateTime = new Regex(@"^(\d|[0-1]?\d|2[0-3]):([0-5]\d)$");

            if (!CorrectDateTime.IsMatch(tb91.Text))
            {
                MessageBox.Show("Неверный формат времени", "Ошибка");
                return;
            }

            Regex CorrectSocCard = new Regex(@"\d{4} \d{4} \d{4} \d{4}");

            if (!DataChecker.CheckFieldsExcept(sp.Children, "tb10") ||
                !DataChecker.CheckDataGrid(заказ.Заказ_Препарат))
                return;

            using (var dbcontext = new АптекаEntities())
            {
                if((bool)ChBSocCard.IsChecked)
                {
                    if (!DataChecker.CheckFields(SpSocCard.Children))
                        return;

                    if(!CorrectSocCard.IsMatch(tb3.Text))
                    {
                        MessageBox.Show("Неверный формат социальной карты", "Ошибка");
                        return;
                    }

                    var socCard = dbcontext.Социальная_карта.AsNoTracking().FirstOrDefault(i => i.Код_социальной_карты == tb3.Text.ToString());

                    if(socCard != null)
                        dbcontext.Entry(заказ.Социальная_карта).State =
                            EntityState.Modified;
                    else
                        dbcontext.Entry(заказ.Социальная_карта).State =
                            EntityState.Added;

                    заказ.Социальная_карта.Код_социальной_карты = заказ.Код_социальной_карты;
                }
                else
                {
                    заказ.Социальная_карта = null;
                }

                if (addOrd)
                {
                    foreach (var i in заказ.Заказ_Препарат)
                    {
                        dbcontext.Entry(i).State =
                            EntityState.Added;

                        dbcontext.Entry(i.Препарат).State =
                         EntityState.Unchanged;
                    };

                    dbcontext.Entry(заказ).State = EntityState.Added;
                }
                else
                {
                    foreach (var i in заказ.Заказ_Препарат)
                    {
                        dbcontext.Entry(i).State =
                            EntityState.Modified;

                        dbcontext.Entry(i.Препарат).State =
                         EntityState.Unchanged;
                    };

                    dbcontext.Entry(заказ).State =
                        EntityState.Modified;
                }

                dbcontext.Entry(заказ.Сотрудник).State =
                        EntityState.Unchanged;

                заказ.Дата_и_время_исполнения = заказ.Дата_и_время_исполнения
                    .AddHours(DateTime.Parse(tb9.Text).Hour)
                    .AddMinutes(DateTime.Parse(tb9.Text).Minute);

                dbcontext.SaveChanges();
            }

            NavigationService.GoBack();
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BtnAddEditMedDel.Content = "Добавить";
            заказ_Препарат = new Заказ_Препарат()
            {
                Код_заказа = заказ.Код_заказа,
                Заказ = заказ
            };
            addOrdMed = true;
            GetMedNames();
            SetVisibilityEditMedDel();
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DGOrdMed.SelectedItem == null)
                return;

            заказ_Препарат = DGOrdMed.SelectedItem as Заказ_Препарат;
            GetMedNames();
            SetVisibilityEditMedDel();
            BtnAddEditMedDel.Content = "Изменить";

        }

        private void SetVisibilityEditMedDel()
        {
            SpEdit.DataContext = заказ_Препарат;
            SpEdit.Visibility = Visibility.Visible;
            sp.Visibility = DGOrdMed.Visibility = Visibility.Collapsed;
            BtnAddEditDel.Visibility = Visibility.Collapsed;
            BtnAddEditMedDel.Visibility = Visibility.Visible;
        }

        private void SetVisibilityEditDel()
        {
            SpEdit.Visibility = Visibility.Collapsed;
            sp.Visibility = DGOrdMed.Visibility = Visibility.Visible;
            BtnAddEditDel.Visibility = Visibility.Visible;
            BtnAddEditMedDel.Visibility = Visibility.Collapsed;
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DGOrdMed.SelectedItem == null)
                return; 
            
            var record = DGOrdMed.SelectedItem as Заказ_Препарат;
            if (record.Код_заказа_препарата != 0)
                using (var dbcontext = new АптекаEntities())
                {
                    dbcontext.Entry(record).State = EntityState.Deleted;

                    dbcontext.SaveChanges();
                }

            заказ.Заказ_Препарат.Remove(record);
            RefreshDG();
        }

        private void RefreshDG()
        {
            DGOrdMed.ItemsSource = null;
            DGOrdMed.ItemsSource = заказ.Заказ_Препарат;
        }

        private void GetMedNames()
        {
            var listMed = заказ.Заказ_Препарат.Select(j => j.Код_препарата);
            List<string> medNames;

            using (var dbcontext = new АптекаEntities())
                medNames = dbcontext.Препарат
                    .AsNoTracking()
                    .Where(i => i.Кол_во != null && i.Кол_во != 0 && !listMed.Contains(i.Код_препарата))
                    .Select(i => i.Название)
                    .ToList();

            if (!addOrdMed)
                medNames.Add(заказ_Препарат.Препарат.Название);

            Cmb1.ItemsSource = medNames;
        }

        private void BtnAddEditDel_ClickSecond(object sender, RoutedEventArgs e)
        {
            if (!DataChecker.CheckFields(SpEdit.Children))
                return;

            using (var dbcontext = new АптекаEntities())
            {
                var med = dbcontext.Препарат.AsNoTracking().First(i => i.Название == Cmb1.Text);

                if (int.Parse(tb33.Text) > med.Кол_во)
                {
                    MessageBox.Show("На складах нет такого количества препарата", "Ошибка");
                    return;
                }

                заказ_Препарат.Препарат = med;
                заказ_Препарат.Код_препарата = med.Код_препарата;
                заказ.Заказ_Препарат.Add(заказ_Препарат);
            }

            SpEdit.DataContext = null;
            Cmb1.ItemsSource = null;
            addOrdMed = false;
            SetVisibilityEditDel();
            RefreshDG();
            CalcCost();
        }

        private void tb8_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalcCost();
        }

        private void CalcCost()
        {
            try
            {
                decimal cost = 0;
                foreach (var item in заказ.Заказ_Препарат)
                {
                    cost += item.Препарат.Конечная_цена__ * item.Количество_препарата;

                    if ((bool)ChBSocCard.IsChecked && tb8.Text != "")
                    {
                        var discount = decimal.Parse(tb8.Text);
                        cost = discount >= 100 ? 0 : cost * (1 - decimal.Parse(tb8.Text) / 100);
                    }
                }
                tb10.Text = cost.ToString();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
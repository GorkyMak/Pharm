using Pharm.Database;
using Pharm.Pages.Edit;
using System.Data.Entity;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using Pharm.Classes;
using System.Windows;

namespace Pharm.Pages
{
    public partial class ListDeliveries : Page
    {
        List<Поставка> deliveries;
        int? med_DelCount = 0;
        double maxCost = 0;

        public ListDeliveries()
        {
            SetMaxSldValue();
            InitializeComponent();
            SetCmbFilterProperties();
            RefreshDG();

            if (Properties.Settings.Default.UserRole != "" && 
                Properties.Settings.Default.UserRole == "Бухгалтер")
            {
                MIDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void SetCmbFilterProperties()
        {
            CmbFilterDeliveries.SelectionChanged += CmbFilterDeliveries_SelectionChanged;
            CmbFilterDeliveries.SelectedIndex = 0;
        }

        private void CmbFilterDeliveries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((CmbFilterDeliveries.SelectedItem as ComboBoxItem).Content)
            {
                case "по стоимости":
                    SldMin.Value = 0;
                    SldMax.Maximum = maxCost;
                    SldMax.Value = maxCost;
                    SldMin.TickFrequency = 100;
                    SldMax.TickFrequency = 100;
                    break;

                case "по количеству препаратов":
                    SldMin.Value = 0;
                    SldMax.Maximum = double.Parse(med_DelCount.ToString());
                    SldMax.Value = double.Parse(med_DelCount.ToString());
                    SldMin.TickFrequency = 1;
                    SldMax.TickFrequency = 1;
                    break;
            }
        }

        private void SetMaxSldValue()
        {
            using (var dbcontext = new АптекаEntities())
            {
                int count = 0;
                foreach (var item in dbcontext.Поставка)
                {
                    if (item.Препарат_Поставка == null)
                        count = 0;

                    if(count < item.Препарат_Поставка.Count())
                        count = item.Препарат_Поставка.Count();
                }
                if (count == 0)
                    return;

                med_DelCount = count;
                maxCost = (double)dbcontext.Поставка.Max(i => i.Стоимость);
            }
        }

        private void RefreshDG()
        {
            GetDeliveries();
            SetDeliveries();
        }

        private void SetDeliveries()
        {
            DGDeliveries.ItemsSource = deliveries;
        }

        private void GetDeliveries()
        {
            using (var dbcontext = new АптекаEntities())
                deliveries = new List<Поставка>(dbcontext.Поставка
                    .AsNoTracking()
                    .Include(i => i.Поставщик)
                    .Include(i => i.Склад)
                    .Include(i => i.Препарат_Поставка.Select(j => j.Препарат)));
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditDeliveries());
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditDeliveries(DGDeliveries.SelectedItem as Поставка));
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (var dbcontext = new АптекаEntities())
            {
                var record = DGDeliveries.SelectedItem as Поставка;

                while (record.Препарат_Поставка.Count > 0)
                    dbcontext.Entry(record.Препарат_Поставка.ElementAt(0)).State = EntityState.Deleted;

                dbcontext.Entry(record).State = EntityState.Deleted;

                dbcontext.SaveChanges();
            }

            RefreshDG();
        }

        private void BtnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!DataChecker.CheckFieldSearch(SpSearch.Children))
            {
                DGDeliveries.ItemsSource = deliveries;
                return;
            }

            var searchResult = deliveries;
            string searchText = TbSearchText.Text.ToLower();

            switch (CmbFilterDeliveries.Text)
            {
                case "по стоимости":
                    searchResult = searchResult.Where(i => i.Стоимость >= (decimal)SldMin.Value && i.Стоимость <= (decimal)SldMax.Value).ToList();
                    break;

                case "по количеству препаратов":
                    searchResult = searchResult.Where(i =>
                    {
                        var countRecords = i.Препарат_Поставка.Count();

                        return countRecords >= (decimal)SldMin.Value &&
                               countRecords <= (decimal)SldMax.Value;
                    }).ToList();
                    break;
            }



            switch (CmbFilterMedication.Text)
            {
                case "по коду поставки":
                    searchResult = searchResult.Where(i => i.Код_поставки.ToString().Contains(searchText)).ToList();
                    break;

                case "по названию поставщика":
                    searchResult = searchResult.Where(i => i.Поставщик.Название.ToLower().Contains(searchText)).ToList();
                    break;

                case "по коду склада":
                    searchResult = searchResult.Where(i => i.Код_склада.ToString().Contains(searchText)).ToList();
                    break;

                case "по коду препарата":
                    searchResult = searchResult.Where(i => i.Препарат_Поставка
                    .Any(j => j.Код_препарата.ToString().Contains(searchText))).ToList();
                    break;

                case "по названию препарата":
                    searchResult = searchResult.Where(i => i.Препарат_Поставка
                    .Any(j => j.Препарат.Название.ToLower().Contains(searchText))).ToList();
                    break;
            }

            DGDeliveries.ItemsSource = searchResult;
        }
    }
}
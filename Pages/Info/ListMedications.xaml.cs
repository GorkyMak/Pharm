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
    public partial class ListMedications : Page
    {
        List<Препарат> medications;
        public ListMedications()
        {
            InitializeComponent();
            SetMaxCost();
            RefreshDG();
        }

        private void SetMaxCost()
        {
            using (var dbcontext = new АптекаEntities())
                SldMaxCost.Maximum = dbcontext.Препарат.Count() == 0 ? 0 : (double)dbcontext.Препарат.Max(i => i.Конечная_цена__);

            SldMaxCost.Value = SldMaxCost.Maximum;
        }

        private void RefreshDG()
        {
            GetMedications();
            SetMedications();
        }

        private void SetMedications()
        {
            DGMed.ItemsSource = medications;
        }

        private void GetMedications()
        {
            using (var dbcontext = new АптекаEntities())
                medications = new List<Препарат>(dbcontext.Препарат
                    .Include(i => i.Изготовитель));
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditMedications());
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditMedications(DGMed.SelectedItem as Препарат));
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var record = DGMed.SelectedItem as Препарат;
            using (var dbcontext = new АптекаEntities())
            {
                if (dbcontext.Препарат_Поставка.FirstOrDefault(i => i.Код_препарата == record.Код_препарата) != null ||
                   dbcontext.Заказ_Препарат.FirstOrDefault(i => i.Код_препарата == record.Код_препарата) != null)
                {
                    MessageBox.Show("Невозможно удалить препарат","Ошибка");
                    return;
                }

                dbcontext.Entry(record).State = EntityState.Deleted;

                dbcontext.SaveChanges();
            }

            RefreshDG();
        }

        private void BtnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!DataChecker.CheckFieldSearch(SpSearch.Children))
            {
                DGMed.ItemsSource = medications;
                return;
            }

            var searchResult = medications;
            string searchText = TbSearchText.Text.ToLower();

            switch (CmbFilterCost.Text)
            {
                case "по закупочной":
                    searchResult = searchResult.Where(i => i.Закупочная_цена__ >= (decimal)SldMinCost.Value && i.Закупочная_цена__ <= (decimal)SldMaxCost.Value).ToList();
                    break;

                case "по конечной":
                    searchResult = searchResult.Where(i => i.Конечная_цена__ >= (decimal)SldMinCost.Value && i.Конечная_цена__ <= (decimal)SldMaxCost.Value).ToList();
                    break;
            }

            switch (CmbFilterMedication.Text)
            {
                case "по названию препарата":
                    searchResult = searchResult.Where(i => i.Название.ToLower().Contains(searchText)).ToList();
                    break;

                case "по коду препарата":
                    searchResult = searchResult.Where(i => i.Код_препарата.ToString().Contains(searchText)).ToList();
                    break;

                case "по названию изготовителя":
                    searchResult = searchResult.Where(i => i.Изготовитель.Название.ToLower().Contains(searchText)).ToList();
                    break;

                case "по коду изготовителя":
                    searchResult = searchResult.Where(i => i.Код_изготовителя.ToString().Contains(searchText)).ToList();
                    break;

                case "по группе":
                    searchResult = searchResult.Where(i => i.Группа.ToLower().Contains(searchText)).ToList();
                    break;
            }

            DGMed.ItemsSource = searchResult;
        }
    }
}
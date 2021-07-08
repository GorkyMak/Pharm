using Pharm.Classes;
using Pharm.Database;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Pharm.Pages.Edit
{
    public partial class EditDeliveries : Page
    {
        Поставка поставка;
        Препарат_Поставка препарат_Поставка;
        bool addDel = false, addMedDel = false;
        public EditDeliveries()
        {
            InitializeComponent();

            BtnAddEditDel.Content = "Добавить";
            addDel = true;
            поставка = new Поставка()
            {
                Поставщик = new Поставщик(),
                Склад = new Склад()
            };

            DataContext = поставка;
            DGPrep_Med.ItemsSource = поставка.Препарат_Поставка;

            tb2.TextChanged += tb2_TextChanged;
        }

        public EditDeliveries(Поставка поставка)
        {
            InitializeComponent();

            this.поставка = поставка;
            DataContext = this.поставка;

            tb2.TextChanged += tb2_TextChanged;
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!DataChecker.CheckFields(sp.Children) || 
                !DataChecker.CheckDataGrid(поставка.Препарат_Поставка))
                return;

            using (var dbcontext = new АптекаEntities())
            {
                var wareHouse = dbcontext.Склад.AsNoTracking().FirstOrDefault(i => i.Код_склада.ToString() == tb2.Text);

                if (wareHouse != null)
                {
                    поставка.Склад = wareHouse;
                    поставка.Код_склада = wareHouse.Код_склада;

                    dbcontext.Entry(поставка.Склад).State =
                            EntityState.Modified;
                }
                else
                {
                    поставка.Склад.Код_склада = int.Parse(tb2.Text.ToString());
                    поставка.Склад.Площадь_м2 = int.Parse(tb3.Text.ToString());

                    dbcontext.Entry(поставка.Склад).State =
                            EntityState.Added;
                }

                var supplier = dbcontext.Поставщик.AsNoTracking()
                    .FirstOrDefault(i => i.Название == tb1.Text.ToString());

                if (supplier != null)
                {
                    поставка.Поставщик = supplier;
                    поставка.Код_поставщика = supplier.Код_поставщика;

                    dbcontext.Entry(поставка.Поставщик).State =
                            EntityState.Unchanged;
                }
                else
                {
                    поставка.Поставщик.Название = tb1.Text;

                    dbcontext.Entry(поставка.Поставщик).State =
                            EntityState.Added;
                }

                dbcontext.Entry(поставка).State = addDel ? EntityState.Added : EntityState.Modified;

                for (int i = 0; i < поставка.Препарат_Поставка.Count; i++)
                {
                    bool contains = dbcontext.Препарат_Поставка.Select(j => j.Код_препарата_поставки).Contains(поставка.Препарат_Поставка.ElementAt(i).Код_препарата_поставки);

                    dbcontext.Entry(поставка.Препарат_Поставка.ElementAt(i)).State = contains ?
                        EntityState.Modified :
                        EntityState.Added;

                    dbcontext.Entry(поставка.Препарат_Поставка.ElementAt(i).Препарат).State =
                        EntityState.Unchanged;
                };

                dbcontext.SaveChanges();
            }

            NavigationService.Navigate(new ListDeliveries());
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BtnAddEditMedDel.Content = "Добавить";
            препарат_Поставка = new Препарат_Поставка()
            {
                Код_поставки = поставка.Код_поставки,
                Поставка = поставка
            };
            addMedDel = true;
            GetMedNames();
            SetVisibilityEditMedDel();
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            препарат_Поставка = DGPrep_Med.SelectedItem as Препарат_Поставка;
            GetMedNames();
            SetVisibilityEditMedDel();
            BtnAddEditMedDel.Content = "Изменить";

        }

        private void SetVisibilityEditMedDel()
        {
            SpEdit.DataContext = препарат_Поставка;
            SpEdit.Visibility = Visibility.Visible;
            sp.Visibility = DGPrep_Med.Visibility = Visibility.Collapsed;
            BtnAddEditDel.Visibility = Visibility.Collapsed;
            BtnAddEditMedDel.Visibility = Visibility.Visible;
        }

        private void SetVisibilityEditDel()
        {
            SpEdit.Visibility = Visibility.Collapsed;
            sp.Visibility = DGPrep_Med.Visibility = Visibility.Visible;
            BtnAddEditDel.Visibility = Visibility.Visible;
            BtnAddEditMedDel.Visibility = Visibility.Collapsed;
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DGPrep_Med.SelectedItem == null)
                return;

            var record = DGPrep_Med.SelectedItem as Препарат_Поставка;

            if(record.Код_препарата_поставки != 0)
                using (var dbcontext = new АптекаEntities())
                {
                    dbcontext.Entry(record).State = EntityState.Deleted;

                    dbcontext.SaveChanges();
                }
            поставка.Препарат_Поставка.Remove(record);
            RefreshDG();
        }

        private void BtnAddEdit_ClickSecond(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!DataChecker.CheckFields(SpEdit.Children))
                return;

            using (var dbcontext = new АптекаEntities())
            {
                var med = dbcontext.Препарат.AsNoTracking().First(i => i.Название == Cmb1.Text);

                препарат_Поставка.Препарат = med;
                препарат_Поставка.Код_препарата = med.Код_препарата;
                поставка.Препарат_Поставка.Add(препарат_Поставка);
            }

            SpEdit.DataContext = null;
            Cmb1.ItemsSource = null;
            addMedDel = false;
            SetVisibilityEditDel();
            RefreshDG();
        }

        private void GetMedNames()
        {
            var listMed = поставка.Препарат_Поставка.Select(j => j.Код_препарата);
            List<string> medNames;

            using (var dbcontext = new АптекаEntities())
                medNames =
                dbcontext.Препарат
                .AsNoTracking()
                .Where(i => !listMed.Contains(i.Код_препарата))
                .Select(i => i.Название)
                .ToList();

            if (!addMedDel)
                medNames.Add(препарат_Поставка.Препарат.Название);

            Cmb1.ItemsSource = medNames;
        }

        private void RefreshDG()
        {
            DGPrep_Med.ItemsSource = null;
            DGPrep_Med.ItemsSource = поставка.Препарат_Поставка;
        }

        private void tb2_TextChanged(object sender, TextChangedEventArgs e)
        {
            using (var dbcontext = new АптекаEntities())
            {
                var wareHouse = dbcontext.Склад.AsNoTracking()
                    .FirstOrDefault(i => i.Код_склада.ToString() == tb2.Text.ToString());

                if (wareHouse != null)
                {
                    tb3.Text = wareHouse.Площадь_м2.ToString();
                    поставка.Склад = wareHouse;
                    поставка.Код_склада = wareHouse.Код_склада;
                }
                else if (tb3.Text.ToString() != "")
                {
                    поставка.Склад.Площадь_м2 = int.Parse(tb3.Text.ToString());
                    поставка.Склад.Код_склада = int.Parse(tb3.Text.ToString());

                    поставка.Код_склада = int.Parse(tb3.Text.ToString());
                }
            }
        }
    }
}
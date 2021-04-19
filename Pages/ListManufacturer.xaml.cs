using Pharm.Database;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListManufacturer.xaml
    /// </summary>
    public partial class ListManufacturer : Page
    {
        List<Manufacturer> Manufacturers;
        public ListManufacturer()
        {
            InitializeComponent();
            GetEmployees();
            DGManufacturer.ItemsSource = Manufacturers;
        }
        private void GetEmployees()
        {
            Manufacturers = new List<Manufacturer>();
            using (var dbcontext = new АптекаEntities())
            {
                var temp = dbcontext.Изготовитель.ToList();
                foreach (var item in temp)
                {
                    Manufacturers.Add(new Manufacturer()
                    {
                        Код_изготовителя = item.Код_изготовителя,
                        Субъект = item.Контактные_данные.Адрес.Субъект,
                        Город = item.Контактные_данные.Адрес.Город,
                        Улица = item.Контактные_данные.Адрес.Улица,
                        Дом = item.Контактные_данные.Адрес.Дом,
                        Квартира = item.Контактные_данные.Адрес.Квартира,
                        Номер_телефона = item.Контактные_данные.Номер_телефона,
                        Сайт = item.Контактные_данные.Сайт,
                        Электронная_почта = item.Контактные_данные.Электронная_почта,
                        Название = item.Название
                    });
                }
            }
        }
    }

    class Manufacturer
    {
        public int Код_изготовителя { get; set; }
        public string Субъект { get; set; }
        public string Город { get; set; }
        public string Улица { get; set; }
        public string Дом { get; set; }
        public string Квартира { get; set; }
        public string Номер_телефона { get; set; }
        public string Сайт { get; set; }
        public string Электронная_почта { get; set; }
        public string Название { get; set; }
    }
}

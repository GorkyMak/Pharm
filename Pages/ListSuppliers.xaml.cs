using Pharm.Database;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListSuppliers.xaml
    /// </summary>
    public partial class ListSuppliers : Page
    {
        List<Suppliers> Suppliers;
        public ListSuppliers()
        {
            InitializeComponent();
            GetEmployees();
            DGSuppliers.ItemsSource = Suppliers;
        }
        private void GetEmployees()
        {
            Suppliers = new List<Suppliers>();
            using (var dbcontext = new АптекаEntities())
            {
                var temp = dbcontext.Поставщик.ToList();
                foreach (var item in temp)
                {
                    var address = new List<string> 
                    { 
                        item.Контактные_данные.Адрес.Субъект, 
                        "г. " + item.Контактные_данные.Адрес.Город,
                        "ул. " + item.Контактные_данные.Адрес.Улица,
                        "д. " + item.Контактные_данные.Адрес.Дом
                    };

                    if (item.Контактные_данные.Адрес.Квартира != null)
                        address.Add("кв. " + item.Контактные_данные.Адрес.Квартира);

                    Suppliers.Add(new Suppliers()
                    {
                        Код_поставщика = item.Код_поставщика,
                        Название = item.Название,
                        Адрес = string.Join(", ", address),
                        Номер_телефона = item.Контактные_данные.Номер_телефона,
                        Сайт = item.Контактные_данные.Сайт,
                        Электронная_почта = item.Контактные_данные.Электронная_почта,
                    });
                }
            }
        }
    }

    class Suppliers
    {
        public int Код_поставщика { get; set; }
        public string Название { get; set; }
        public string Адрес { get; set; }
        public string Номер_телефона { get; set; }
        public string Сайт { get; set; }
        public string Электронная_почта { get; set; }
    }
}

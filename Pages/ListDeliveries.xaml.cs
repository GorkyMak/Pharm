using Pharm.Classes;
using Pharm.Database;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListDelivery.xaml
    /// </summary>
    public partial class ListDeliveries : Page
    {
        List<Deliveries> Deliveries = new List<Deliveries>();
        public ListDeliveries()
        {
            InitializeComponent();
            GetDeliveries();
            DGDeliveries.ItemsSource = Deliveries;
        }

        private void GetDeliveries()
        {
            using (var dbcontext = new АптекаEntities())
            {
                var temp = dbcontext.Поставка.ToList();

                foreach(var item in temp)
                {
                    var deliveries = new Deliveries()
                    {
                        Код_поставки = item.Код_поставки,
                        Название_поставщика = item.Поставщик.Название,
                        Код_склада = item.Код_склада,
                        Стоимость__ = item.Стоимость__,
                        Препаратs = new List<Middle_Medications>()
                    };

                    var med_deliv = item.Препарат_Поставка.ToList();
                    foreach (var record in med_deliv)
                        deliveries.Препаратs.Add(new Middle_Medications()
                        {
                            Код_препарата = record.Код_препарата,
                            Название_препарата = record.Препарат.Название,
                            Количество_препарата = record.Количество_препарата,
                        });

                    Deliveries.Add(deliveries);
                    
                }
            }    
        }
    }

    class Deliveries
    {
        public int Код_поставки { get; set; }
        public string Название_поставщика { get; set; }
        public int Код_склада { get; set; }
        public decimal Стоимость__ { get; set; }
        public List<Middle_Medications> Препаратs { get; set; }
    }
}

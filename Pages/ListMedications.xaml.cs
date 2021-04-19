using Pharm.Database;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListMedications.xaml
    /// </summary>
    public partial class ListMedications : Page
    {
        List<Medications> Medications;
        public ListMedications()
        {
            InitializeComponent();

            GetEmployees();
            DGMed.ItemsSource = Medications;
        }
        private void GetEmployees()
        {
            Medications = new List<Medications>();
            using (var dbcontext = new АптекаEntities())
            {
                var temp = dbcontext.Препарат.ToList();
                foreach (var item in temp)
                {
                    Medications.Add(new Medications()
                    {
                        Код_препарата = item.Код_препарата,
                        Код_изготовителя = item.Код_изготовителя,
                        Название_изготовителя = item.Изготовитель.Название,
                        Название = item.Название,
                        Группа = item.Группа,
                        Закупочная_цена__ = item.Закупочная_цена__,
                        Конечная_цена__ = item.Конечная_цена__
                    });
                }
            }
        }
    }

    class Medications
    {
        public int Код_препарата { get; set; }
        public int Код_изготовителя { get; set; }
        public string Название_изготовителя { get; set; }
        public string Название { get; set; }
        public string Группа { get; set; }
        public decimal Закупочная_цена__ { get; set; }
        public decimal Конечная_цена__ { get; set; }
    }
}
using Pharm.Database;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListSocCards.xaml
    /// </summary>
    public partial class ListSocCards : Page
    {
        List<SocCards> SocCards;
        public ListSocCards()
        {
            InitializeComponent();
            GetEmployees();
            DGSocCards.ItemsSource = SocCards;
        }
        private void GetEmployees()
        {
            SocCards = new List<SocCards>();
            using (var dbcontext = new АптекаEntities())
            {
                var temp = dbcontext.Социальная_карта.ToList();
                foreach (var item in temp)
                {
                    SocCards.Add(new SocCards()
                    {
                        Код_социальной_карты = item.Код_социальной_карты,
                        ФИО_владельца = item.Фамилия + " " + item.Имя[0] + "." + item.Отчество[0] + ".",
                        Срок_действия = item.Срок_действия,
                        Скидка = item.Скидка
                    });
                }
            }
        }
    }

    class SocCards
    {
        public int Код_социальной_карты { get; set; }
        public string ФИО_владельца { get; set; }
        public System.DateTime Срок_действия { get; set; }
        public decimal Скидка { get; set; }
    }
}

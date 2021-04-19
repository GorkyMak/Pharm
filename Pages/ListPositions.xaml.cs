using Pharm.Database;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListPositions.xaml
    /// </summary>
    public partial class ListPositions : Page
    {
        List<Должность> Positions;
        public ListPositions()
        {
            InitializeComponent();
            GetEmployees();
            DGPositions.ItemsSource = Positions;
        }
        private void GetEmployees()
        {
            Positions = new List<Должность>();
            using (var dbcontext = new АптекаEntities())
            {
                Positions = dbcontext.Должность.ToList();
            }
        }
    }
}

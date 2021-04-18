using Pharm.Database;
using System.Linq;
using System.Windows.Controls;

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListMedications.xaml
    /// </summary>
    public partial class ListMedications : Page
    {
        public ListMedications()
        {
            InitializeComponent();

            using (var dbcontext = new АптекаEntities())
                DGMed.ItemsSource = dbcontext.Препарат.ToList();
        }
    }
}

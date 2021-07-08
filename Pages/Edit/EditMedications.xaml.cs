using Pharm.Classes;
using Pharm.Database;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Pharm.Pages.Edit
{
    public partial class EditMedications : Page
    {
        Препарат medications;
        public EditMedications()
        {
            InitializeComponent();

            DataContext = new Препарат();
        }

        public EditMedications(Препарат medications)
        {
            InitializeComponent();
            DataContext = medications;
            this.medications = medications;
        }

        private void BtnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!DataChecker.CheckFields(sp.Children))
                return;

            using (var dbcontext = new АптекаEntities())
            {
                if (medications == null)
                {
                    medications = (Препарат)DataContext;
                    dbcontext.Entry(medications).State =
                        EntityState.Added;
                }
                else
                {
                    dbcontext.Entry(medications).State =
                        EntityState.Modified;
                }

                var manufacture = dbcontext.Изготовитель.FirstOrDefault(i => i.Название == Cmb1.Text);

                if (manufacture != null)
                {
                    medications.Изготовитель = manufacture;
                    medications.Код_изготовителя = manufacture.Код_изготовителя;
                }
                else
                {
                    manufacture = new Изготовитель() { Название = Cmb1.Text };
                    medications.Изготовитель = manufacture;
                    medications.Код_изготовителя = manufacture.Код_изготовителя;
                }

                dbcontext.SaveChanges();
            }

            NavigationService.GoBack();
        }
    }
}
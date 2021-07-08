using System;
using System.Windows;

namespace Pharm.Windows
{
    public partial class MainWindow : Window
    {
        AboutProgram aboutProgram = new AboutProgram();
        HelpSys helpSys = new HelpSys();
        public MainWindow()
        {
            InitializeComponent();

            if (Properties.Settings.Default.UserLogin != "")
                LUserRole.Content += Properties.Settings.Default.UserLogin;

            if (Properties.Settings.Default.UserRole != "")
                switch (Properties.Settings.Default.UserRole)
                {
                    case "Заведующий аптекой":
                        BtnUser.Visibility = Visibility.Collapsed;
                        BtnMedication.Visibility = Visibility.Collapsed;
                        BtnDelivery.Visibility = Visibility.Collapsed;
                        BtnOrder.Visibility = Visibility.Collapsed;
                        break;

                    case "Бухгалтер":
                        BtnEmployee.Visibility = Visibility.Collapsed;
                        BtnUser.Visibility = Visibility.Collapsed;
                        BtnMedication.Visibility = Visibility.Collapsed;
                        BtnOrder.Visibility = Visibility.Collapsed;
                        break;

                    case "Продавец-фармацевт":
                        BtnMedication.Visibility = Visibility.Collapsed;
                        BtnDelivery.Visibility = Visibility.Collapsed;
                        BtnEmployee.Visibility = Visibility.Collapsed;
                        BtnUser.Visibility = Visibility.Collapsed;
                        break;
                }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (FrMain.CanGoBack)
                FrMain.GoBack();
        }

        private void BtnGo_Click(object sender, RoutedEventArgs e)
        {
            if (FrMain.CanGoForward)
                FrMain.GoForward();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            new Authorization().Show();
            Close();
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/Info/ListOrders.xaml", UriKind.Relative));
        }

        private void BtnEmployee_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/Info/ListEmployees.xaml", UriKind.Relative));
        }

        private void BtnMedication_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/Info/ListMedications.xaml", UriKind.Relative));
        }

        private void BtnDelivery_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/Info/ListDeliveries.xaml", UriKind.Relative));
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/Info/ListUsers.xaml", UriKind.Relative));
        }

        private void BtnAboutProg_Click(object sender, RoutedEventArgs e)
        {
            aboutProgram.Show();
        }

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            helpSys.Show();
        }
    }
}

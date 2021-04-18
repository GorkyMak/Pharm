using Pharm.Database;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace Pharm
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string Path = "UserData.json";
        public MainWindow()
        {
            InitializeComponent();

            //LUserRole.Content = "Здраствуйте, " + Properties.Settings.Default.UserRole;
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
            new Windows.Authorization().Show();
            Close();
        }

        private void BtnRegOrder_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/RegOrder.xaml", UriKind.Relative));
        }

        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/ListOrders.xaml", UriKind.Relative));
        }

        private void BtnEmployee_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/ListEmployees.xaml", UriKind.Relative));
        }

        private void BtnMedication_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/ListMedications.xaml", UriKind.Relative));

        }

        private void BtnSocCard_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/ListSocCards.xaml", UriKind.Relative));

        }
    }
}

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

        private void BtnDelivery_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/ListDeliveries.xaml", UriKind.Relative));
        }

        private void BtnPositions_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/ListPositions.xaml", UriKind.Relative));
        }

        private void BtnManufacturer_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/ListManufacturer.xaml", UriKind.Relative));
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/ListUsers.xaml", UriKind.Relative));
        }

        private void BtnSuppliers_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/ListSuppliers.xaml", UriKind.Relative));
        }

        private void BtnWarehouses_Click(object sender, RoutedEventArgs e)
        {
            FrMain.Navigate(new Uri("Pages/ListWarehouses.xaml", UriKind.Relative));
        }
    }
}

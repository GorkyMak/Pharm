using System.Windows;

namespace Pharm.Windows
{
    public partial class HelpSys : Window
    {
        public HelpSys()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Hidden;
        }
    }
}
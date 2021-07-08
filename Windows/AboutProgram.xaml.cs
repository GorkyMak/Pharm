using System.Windows;

namespace Pharm.Windows
{
    public partial class AboutProgram : Window
    {
        public AboutProgram()
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
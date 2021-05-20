using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_VNC
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MafVnc myMafVnc;

        DispatcherTimer myDispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();

            // HMI timer init
            myDispatcherTimer = new DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            myDispatcherTimer.Tick += MyDispatcherTimer_Tick;
            myDispatcherTimer.Start();

            // Create VNC interface and atatche it to WFH
            myMafVnc = new MafVnc();
            myWindowsFormHost.Child = myMafVnc;
            myMafVnc.vncConnectionOpened += MyVnc_vncConnectionOpened;
            myMafVnc.vncConnectionLost += MyVnc_vncConnectionLost;
            myMafVnc.vncConnectionError += MyVnc_vncConnectionError;
        }

        private void bn1_Click(object sender, RoutedEventArgs e)
        {
            // VNC start
            myMafVnc.Start("192.168.250.150", 5900, "mafepb");
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            // VNC stop
            myMafVnc.Stop();

            // HMI timer stop
            myDispatcherTimer.Stop();

            base.OnClosing(e);
        }

        #region VNC Event
        private void MyVnc_vncConnectionOpened()
        {
            bn1.Visibility = Visibility.Hidden;
        }

        private void MyVnc_vncConnectionLost()
        {
            bn1.Visibility = Visibility.Visible;
        }

        private void MyVnc_vncConnectionError()
        {
            MessageBox.Show("Connection Error!", "Error message", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        #endregion

        #region HMI timer
        private void MyDispatcherTimer_Tick(object sender, EventArgs e)
        {
            labelHeure.Content = DateTime.Now.ToLongTimeString();
        }
        #endregion
    }
}

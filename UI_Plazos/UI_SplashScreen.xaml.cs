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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UI_Plazos
{
    /// <summary>
    /// Lógica de interacción para UI_SplashScreen.xaml
    /// </summary> 
    
    public partial class UI_SplashScreen : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        public UI_SplashScreen()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            timer.Stop();
            this.Close();
        }
    }
}
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
using System.Media;

namespace UI_Plazos
{
    /// <summary>
    /// Lógica de interacción para UI_ToastAlert.xaml
    /// </summary>
    
    public partial class UI_ToastAlert : Window
    {
        DispatcherTimer timer;
        DispatcherTimer closeAlert;

        public UI_ToastAlert(string _title, string _message, AlertType _type)
        {
            InitializeComponent();
            Title.Text = _title;
            Message.Text = _message;
            BrushConverter brush = new BrushConverter();
            switch (_type)
            {
                case AlertType.success:
                    AlertColor.Background = (Brush)brush.ConvertFrom("#33D687");
                    Icon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/success.png"));
                    SystemSounds.Asterisk.Play();
                    break;
                case AlertType.info:
                    AlertColor.Background = (Brush)brush.ConvertFrom("#BEBEBE");
                    Icon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/information.png"));
                    break;
                case AlertType.warning:
                    AlertColor.Background = (Brush)brush.ConvertFrom("#FFDB4A");
                    Icon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/warning.png"));
                    SystemSounds.Hand.Play();
                    break;
                case AlertType.error:
                    AlertColor.Background = (Brush)brush.ConvertFrom("#FF6B6B");
                    Icon.Source = new BitmapImage(new Uri("pack://application:,,,/Assets/error.png"));
                    SystemSounds.Hand.Play();
                    break;
            }
            timer = new DispatcherTimer();
            closeAlert = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += timer_Tick;
            timer.Start();
            closeAlert.Tick += closeAlert_Tick;
            closeAlert.Interval = TimeSpan.FromSeconds(0.1);
        }

        private void closeAlert_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0)
            {
                this.Opacity -= 0.1;
            }
            else
            {
                this.Close();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            closeAlert.Start();
        }

        public static void ShowAlert(string _title, string _message, AlertType _type)
        {
            new UI_Plazos.UI_ToastAlert(_title, _message, _type).Show();
        }

        //Set Position
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopArea = System.Windows.SystemParameters.WorkArea;
            this.Top = desktopArea.Height - this.Height - 30;
            this.Left = desktopArea.Width - this.Width - 30;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            closeAlert.Stop();
            timer.Stop();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            closeAlert.Start();
            timer.Start();
        }   
    }

    public enum AlertType
    {
        success, info, warning, error
    }
}

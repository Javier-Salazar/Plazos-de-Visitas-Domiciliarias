using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace UI_Plazos
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// respaldar y restaurar
    /// </summary>

    public partial class MainWindow : Window
    {
        XMLData xml = new XMLData();
        int aux = -1; //This help us to send the index position on update event
        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                foreach (var dato in xml.Read())
                {
                    UAP.Items.Insert(0, dato); //Insert on top (0).
                }
                int year = DateTime.Today.Year - 1;
                fecha.BlackoutDates.Add(new CalendarDateRange(new DateTime(1, 1, 1), new DateTime(year, 12, 31)));
                for (int i = 2021; i < 3000; i++)
                {
                    fecha.BlackoutDates.Add(new CalendarDateRange(new DateTime(i, 1, 1), new DateTime(i, 1, 1)));
                    fecha.BlackoutDates.Add(new CalendarDateRange(new DateTime(i, 5, 1), new DateTime(i, 5, 1)));
                    fecha.BlackoutDates.Add(new CalendarDateRange(new DateTime(i, 5, 5), new DateTime(i, 5, 5)));
                    fecha.BlackoutDates.Add(new CalendarDateRange(new DateTime(i, 9, 16), new DateTime(i, 9, 16)));
                    fecha.BlackoutDates.Add(new CalendarDateRange(new DateTime(i, 10, 12), new DateTime(i, 10, 12)));
                    fecha.BlackoutDates.Add(new CalendarDateRange(new DateTime(i, 11, 2), new DateTime(i, 11, 2)));
                    fecha.BlackoutDates.Add(new CalendarDateRange(new DateTime(i, 12, 18), new DateTime(i, 12, 31)));
                }
            }
            catch(Exception)
            {
                UI_ToastAlert.ShowAlert("Error al cargar!", "Algunos datos no se cargaron correctamente", AlertType.error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SnackbarThree.IsActive = true;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
            AlertasVencimientoDePlazos();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            SnackbarThree.IsActive = false;
        }

        private void extendido_Checked(object sender, RoutedEventArgs e)
        {
            modoPlazo.Content = "Plazo extendido activado";
            Delete.Visibility = Visibility.Collapsed;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.FileName = "PlazosVisitaDomiciliaria-Backup";
            save.DefaultExt = "xml";
            save.Filter = "Archivo de Datos (*.xml)|*.xml";
            save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (save.ShowDialog() == true)
            {
                FileInfo info = new FileInfo(save.FileName);
                bool saved = xml.FileBackUp(info.DirectoryName);
                if (saved)
                {
                    UI_ToastAlert.ShowAlert("Respaldado!", "Se ha realizado una copia de respaldo.", AlertType.success);
                }
                else
                {
                    UI_ToastAlert.ShowAlert("Error!", "Fallo al intentar respaldar.", AlertType.error);
                }
            }
            //GenerateDates();
        }

        private void extendido_Unchecked(object sender, RoutedEventArgs e)
        {
            modoPlazo.Content = "Plazo extendido desactivado";
            Delete.Visibility = Visibility.Collapsed;
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            Delete.Visibility = Visibility.Collapsed;
            GenerateDates(aux);
            Create.IsEnabled = true;
            Update.Visibility = Visibility.Collapsed;
            Cancel.Visibility = Visibility.Collapsed;
            UI_ToastAlert.ShowAlert("Actualizado!", "Se han actualizado algunos datos", AlertType.success);
            aux = -1;
            ClearFields();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Delete.Visibility = Visibility.Collapsed;
            aux = -1;
            ClearFields();
            Create.IsEnabled = true;
            Update.Visibility = Visibility.Collapsed;
            Cancel.Visibility = Visibility.Collapsed;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Delete.Visibility = Visibility.Collapsed;
            UpdateItem();
        }

        private void ListViewItem_KeyDown(object sender, KeyEventArgs e)
        {
            if((e.Key == Key.LeftShift))
            {
                Delete.Visibility = Visibility.Visible;
            }
        }

        private void fecha_GotFocus(object sender, RoutedEventArgs e)
        {
            Delete.Visibility = Visibility.Collapsed;
        }

        private void nombreContribuyente_GotFocus(object sender, RoutedEventArgs e)
        {
            Delete.Visibility = Visibility.Collapsed;
        }

        private void ListViewItem_GotFocus(object sender, RoutedEventArgs e)
        {
            Delete.Visibility = Visibility.Collapsed;
        }

        private void DialogHost_DialogClosing(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            Delete.Visibility = Visibility.Collapsed;
        }

        private void deleteItem_Click(object sender, RoutedEventArgs e)
        {
            
            int index = UAP.Items.IndexOf(UAP.SelectedItem);
            xml.Delete(index);
            UAP.Items.RemoveAt(index);
            UI_ToastAlert.ShowAlert("Borrado Exitosamente!", "Se ha eliminado el contribuyente.", AlertType.success);
        }

        //Main method to generate all dates
        private void GenerateDates(int _index = -1)
        {
            Delete.Visibility = Visibility.Collapsed;
            try
            {
                bool pass = ComprobarCampos();
                if (pass)
                {
                    Items item = new Items();
                    bool selection = Convert.ToBoolean(extendido.IsChecked);

                    item.contribuyente = nombreContribuyente.Text;
                    item.fecha_inicio = fecha.SelectedDate.Value.ToLongDateString();
                    int dia = Convert.ToInt32(fecha.SelectedDate.Value.Day);
                    int mes = Convert.ToInt32(fecha.SelectedDate.Value.Month);
                    int año = Convert.ToInt32(fecha.SelectedDate.Value.Year);
                    DateTime date = new DateTime(año, mes, dia);

                    DayOfWeek check_day = fecha.SelectedDate.Value.DayOfWeek;
                    date = CheckDaySelected(date, check_day, item);
                    item.vence_plazo = FechaPlazoMeses(date, selection).ToString("D");
                    item.plazo_extendido = (selection) ? "Sí" : "No";
                    item.comite = FechaComite(date, selection).ToString("D");
                    date = Convert.ToDateTime(item.comite);
                    item.entrega_expediente = date.AddDays(-7).ToString("D");
                    item.notificacion_atenta = FechaNotificacionAtenta(date).ToString("D");
                    date = Convert.ToDateTime(item.notificacion_atenta);
                    item.vencimiento_10_dias = FechaNotificacion10DiasAtenta(date).ToString("D");
                    item.entrega_borrador_uap = FechaEntregaBorradorUAP(date).ToString("D");
                    date = Convert.ToDateTime(item.entrega_borrador_uap);
                    item.entrega_uap = FechaEntregaUAP(date).ToString("D");
                    date = Convert.ToDateTime(item.entrega_uap);
                    item.recepcion_uap = FechaRecepcionUAP(date).ToString("D");
                    date = Convert.ToDateTime(item.recepcion_uap);
                    item.sengunda_vuelta_uap = FechaSegundaVueltaUAP(date).ToString("D");
                    date = Convert.ToDateTime(item.sengunda_vuelta_uap);
                    item.levantamiento_uap = FechaLevantamientoUAP(date).ToString("D");
                    date = Convert.ToDateTime(item.levantamiento_uap);
                    item.vence_uap = FechaVenceUAP(date).ToString("D");
                    date = Convert.ToDateTime(item.vence_uap);
                    item.levantamiento_acta_final = FechaLevantamientoActaFinal(date).ToString("D");
                    date = Convert.ToDateTime(item.levantamiento_acta_final);
                    item.dias_estrados = FechaDiasEstresados(date);
                    item.liquidacion = FechaLiquidacion(date).ToString("D");
                    item.entrega_borrador_liquidacion = FechaEntregaBorradorLiquidacion(date).ToString("D");
                    item.plazo_prodecon = FechaPlazoProdecon(date).ToString("D");
                    date = Convert.ToDateTime(item.entrega_borrador_liquidacion);
                    item.entrega_liquidacion = FechaEntregaLiquidacion(date).ToString("D");
                    date = Convert.ToDateTime(item.entrega_liquidacion);
                    item.fecha_cierre = FechaCierre(date).ToString("D");
                    item.recepcion_liquidacion = FechaRecepcionLiquidacion(date).ToString("D");
                    date = Convert.ToDateTime(item.recepcion_liquidacion);
                    item.segunda_vuelta_liquidacion = FechaSegundaVueltaLiquidacion(date).ToString("D");
                    date = Convert.ToDateTime(item.segunda_vuelta_liquidacion);
                    item.impresion_firma = FechaImpresionFirma(date).ToString("D");

                    DateTime first_date = Convert.ToDateTime(item.levantamiento_acta_final);
                    DateTime second_date = Convert.ToDateTime(item.vence_plazo);
                    item.dias_sobrantes = FechaDiasSobrantes(first_date, second_date);

                    AddItem(_index, item.contribuyente, item.fecha_inicio, item.plazo_extendido, item.entrega_expediente, item.comite,
                        item.notificacion_atenta, item.vencimiento_10_dias, item.entrega_borrador_uap, item.entrega_uap,
                        item.recepcion_uap, item.sengunda_vuelta_uap, item.levantamiento_uap, item.vence_uap,
                        item.levantamiento_acta_final, item.dias_estrados, item.vence_plazo, item.dias_sobrantes,
                        item.liquidacion, item.entrega_borrador_liquidacion, item.entrega_liquidacion,
                        item.recepcion_liquidacion, item.segunda_vuelta_liquidacion, item.impresion_firma, item.fecha_cierre,
                        item.plazo_prodecon);

                    //AutoRwsize Columns.
                    foreach (GridViewColumn c in header.Columns)
                    {
                        c.Width = 0; //Set it to no width.
                        c.Width = double.NaN; //Resize it automatically.
                    }

                    nombreEmpty.Visibility = Visibility.Collapsed;
                    pickerEmpty.Visibility = Visibility.Collapsed;

                    UI_ToastAlert.ShowAlert("Fecha Agregada!", "La fecha se ha agregado exitosamente", AlertType.success);
                    ClearFields();
                }
                else
                {
                    if (String.IsNullOrEmpty(nombreContribuyente.Text) && String.IsNullOrEmpty(Convert.ToString(fecha.SelectedDate)))
                    {
                        nombreEmpty.Visibility = Visibility.Visible;
                        pickerEmpty.Visibility = Visibility.Visible;
                    }
                    else if (String.IsNullOrEmpty(nombreContribuyente.Text))
                    {
                        nombreEmpty.Visibility = Visibility.Visible;
                        pickerEmpty.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        nombreEmpty.Visibility = Visibility.Collapsed;
                        pickerEmpty.Visibility = Visibility.Visible;
                    }

                    UI_ToastAlert.ShowAlert("Faltan datos!", "Llene todos los campos marcados", AlertType.error);
                }
            }
            catch (Exception)
            {
                UI_ToastAlert.ShowAlert("Error!", "No se pudieron agregar los datos", AlertType.error);
            }
        }

        //Check that is not null or empty fields.
        private bool ComprobarCampos()
        {
            bool pass = false;
            if (String.IsNullOrEmpty(nombreContribuyente.Text) || String.IsNullOrEmpty(Convert.ToString(fecha.SelectedDate)))
            {
                pass = false;
            }
            else
            {
                pass = true;
            }
            return pass;
        }

        //Check if the date selected are not weekend or day off.
        private DateTime CheckDaySelected(DateTime _date, DayOfWeek _check_day, Items _item)
        {
            if (_check_day == DayOfWeek.Saturday)
            {
                _item.fecha_inicio = _date.AddDays(2).ToString("D"); //Move to Tuesday.
                _date = Convert.ToDateTime(_item.fecha_inicio);
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    //If Tuesday is day off, move to next day.
                    _item.fecha_inicio = _date.AddDays(1).ToString("D");
                    _date = Convert.ToDateTime(_item.fecha_inicio);
                }
                fecha.SelectedDate = Convert.ToDateTime(_item.fecha_inicio);
            }
            else if (_check_day == DayOfWeek.Sunday)
            {
                _item.fecha_inicio = _date.AddDays(1).ToString("D"); //Move to Thursday.
                _date = Convert.ToDateTime(_item.fecha_inicio);
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    //If Thursday is day off, move to next day
                    _item.fecha_inicio = _date.AddDays(1).ToString("D");
                    _date = Convert.ToDateTime(_item.fecha_inicio);
                }
                fecha.SelectedDate = Convert.ToDateTime(_item.fecha_inicio);
            }
            return _date;
        }

        //Get the date 4 or 8 months later, but only Tuesday and Thursday.
        private DateTime FechaComite(DateTime _date, bool _selection)
        {
            if (_selection)
            {
                _date = _date.AddMonths(8);
            }
            else
            {
                _date = _date.AddMonths(4);
            }
            DayOfWeek day = _date.DayOfWeek;
            if (day == DayOfWeek.Monday || day == DayOfWeek.Wednesday)
            {
                _date = _date.AddDays(1);
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    if (day == DayOfWeek.Monday)
                    {
                        _date = _date.AddDays(2);
                    }
                    else if (day == DayOfWeek.Wednesday)
                    {
                        _date = _date.AddDays(5);
                    }
                }
            }
            else if (day == DayOfWeek.Friday)
            {
                _date = _date.AddDays(4);
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    _date = _date.AddDays(2);
                }
            }
            else if (day == DayOfWeek.Saturday)
            {
                _date = _date.AddDays(3);
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    _date = _date.AddDays(2);
                }
            }
            else if (day == DayOfWeek.Sunday)
            {
                _date = _date.AddDays(2);
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    _date = _date.AddDays(2);
                    if (fecha.BlackoutDates.Contains(_date) == true)
                    {
                        _date = _date.AddDays(5);

                    }
                }
            }
            else
            {
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    if (day == DayOfWeek.Tuesday)
                    {
                        _date = _date.AddDays(2);
                    }
                    else if (day == DayOfWeek.Thursday)
                    {
                        _date = _date.AddDays(5);
                    }
                }
            }
            return _date;
        }

        private DateTime FechaNotificacionAtenta(DateTime _date)
        {
            _date = _date.AddDays(1);
            DayOfWeek day = _date.DayOfWeek;
            if (fecha.BlackoutDates.Contains(_date) == true)
            {
                if (day == DayOfWeek.Wednesday)
                {
                    _date = _date.AddDays(1);
                }
                else if (day == DayOfWeek.Friday)
                {
                    _date = _date.AddDays(3);
                }
            }
            return _date;
        }

        //Get the date 11 days after comité date, but don't count weekends and days off.
        private DateTime FechaNotificacion10DiasAtenta(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(11);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaEntregaBorradorUAP(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(5);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaEntregaUAP(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(1);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaRecepcionUAP(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(8);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaSegundaVueltaUAP(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(2);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaLevantamientoUAP(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(1);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaVenceUAP(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(20);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaLevantamientoActaFinal(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(2);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private String FechaDiasEstresados(DateTime _date)
        {
            DateTime fecha_inicial = _date.AddDays(1);
            DateTime fecha_final = _date.AddDays(16);
            DateTime aux;
            int dias_faltantes = DiasHabilesFaltantes(fecha_inicial, fecha_final);
            aux = fecha_final;
            fecha_final = fecha_final.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, fecha_final);
                aux = fecha_final;
                fecha_final = fecha_final.AddDays(dias_faltantes);
            }
            string date = String.Format("{0} - {1}", fecha_inicial.ToString("D"), fecha_final.ToString("D"));
            return date;
        }

        private DateTime FechaPlazoMeses(DateTime _date, bool _selection)
        {
            if (_selection)
            {
                _date = _date.AddMonths(12);
            }
            else
            {
                _date = _date.AddMonths(8);
            }
            DayOfWeek day = _date.DayOfWeek;
            if (day == DayOfWeek.Monday || day == DayOfWeek.Wednesday)
            {
                _date = _date.AddDays(1);
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    if (day == DayOfWeek.Monday)
                    {
                        _date = _date.AddDays(2);
                    }
                    else if (day == DayOfWeek.Wednesday)
                    {
                        _date = _date.AddDays(5);
                    }
                }
            }
            else if (day == DayOfWeek.Friday)
            {
                _date = _date.AddDays(4);
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    _date = _date.AddDays(2);
                }
            }
            else if (day == DayOfWeek.Saturday)
            {
                _date = _date.AddDays(3);
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    _date = _date.AddDays(2);
                }
            }
            else if (day == DayOfWeek.Sunday)
            {
                _date = _date.AddDays(2);
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    _date = _date.AddDays(2);
                    if (fecha.BlackoutDates.Contains(_date) == true)
                    {
                        _date = _date.AddDays(5);

                    }
                }
            }
            else
            {
                if (fecha.BlackoutDates.Contains(_date) == true)
                {
                    if (day == DayOfWeek.Tuesday)
                    {
                        _date = _date.AddDays(2);
                    }
                    else if (day == DayOfWeek.Thursday)
                    {
                        _date = _date.AddDays(5);
                    }
                }
            }
            return _date;
        }

        private string FechaDiasSobrantes(DateTime first_date, DateTime second_date)
        {
            TimeSpan total_days = second_date.Subtract(first_date);
            int dias_faltantes = DiasHabilesFaltantes(first_date, second_date);
            int total = Math.Abs(Convert.ToInt32(total_days.Days) - dias_faltantes);
            string date = String.Format("{0} días naturales sobrantes", total);
            return date;
        }

        private DateTime FechaLiquidacion(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddMonths(2);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaEntregaBorradorLiquidacion(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(5);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaEntregaLiquidacion(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(5);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaRecepcionLiquidacion(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(8);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaImpresionFirma(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(5);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaSegundaVueltaLiquidacion(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(2);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaPlazoProdecon(DateTime _date)
        {
            DateTime aux = _date;
            _date = _date.AddDays(20);
            int dias_faltantes = DiasHabilesFaltantes(aux, _date);
            aux = _date;
            _date = _date.AddDays(dias_faltantes);
            while (dias_faltantes > 0)
            {
                dias_faltantes = DiasHabilesFaltantes(aux, _date);
                aux = _date;
                _date = _date.AddDays(dias_faltantes);
            }
            return _date;
        }

        private DateTime FechaCierre(DateTime _date)
        {
            int month = _date.Month;
            DateTime today = DateTime.Now;
            DateTime[] fecha_cierre = new DateTime[] 
            {
                new DateTime(today.Year, 01, 25), 
                new DateTime(today.Year, 02, 22),
                new DateTime(today.Year, 03, 25), 
                new DateTime(today.Year, 04, 25), 
                new DateTime(today.Year, 05, 25), 
                new DateTime(today.Year, 06, 25), 
                new DateTime(today.Year, 07, 25), 
                new DateTime(today.Year, 08, 25), 
                new DateTime(today.Year, 09, 25), 
                new DateTime(today.Year, 10, 25), 
                new DateTime(today.Year, 11, 25), 
                new DateTime(today.Year, 12, 25)
            };

            switch (month)
            {
                case 1:
                    _date = fecha_cierre[0];
                    break;
                case 2:
                    _date = fecha_cierre[1];
                    break;
                case 3:
                    _date = fecha_cierre[2];
                    break;
                case 4:
                    _date = fecha_cierre[3];
                    break;
                case 5:
                    _date = fecha_cierre[4];
                    break;
                case 6:
                    _date = fecha_cierre[5];
                    break;
                case 7:
                    _date = fecha_cierre[6];
                    break;
                case 8:
                    _date = fecha_cierre[7];
                    break;
                case 9:
                    _date = fecha_cierre[8];
                    break;
                case 10:
                    _date = fecha_cierre[9];
                    break;
                case 11:
                    _date = fecha_cierre[10];
                    break;
                case 12:
                    _date = fecha_cierre[11];
                    break;
            }
            return _date;
        }

        //Get all weekends and days off for delete those days.
        private int DiasHabilesFaltantes(DateTime _aux, DateTime _date)
        {
            DayOfWeek day;
            int saturday = 0, sunday = 0, blackout = 0, total_days;
            while (_aux <= _date.AddDays(-1))
            {
                _aux = _aux + new TimeSpan(1, 0, 0, 0);
                day = _aux.DayOfWeek;
                if (day == DayOfWeek.Saturday)
                {
                    saturday++;
                }
                else if (day == DayOfWeek.Sunday)
                {
                    sunday++;
                }
                else if (fecha.BlackoutDates.Contains(_aux) == true)
                {
                    blackout++;
                }
            }
            total_days = saturday + sunday + blackout;
            return total_days;
        }

        //Method that add all final dates to the ListView.
        private void AddItem(int _index, string _contribuyente, string _fecha_inicio, string _plazo_extendido,
            string _entrega_expediente, string _comite, string _notificacion_atenta, string _vencimiento_10_dias,
            string _entrega_borrador_uap, string _entrega_uap, string _recepcion_uap, string _sengunda_vuelta_uap,
            string _levantamiento_uap, string _vence_uap, string _levantamiento_acta_final, string _dias_estrados,
            string _vence_plazo, string _dias_sobrantes, string _liquidacion, string _entrega_borrador_liquidacion,
            string _entrega_liquidacion, string _recepcion_liquidacion, string _segunda_vuelta_liquidacion,
            string _impresion_firma, string _fecha_cierre, string _plazo_prodecon)
        {
            if(_index == -1)
            {
                //Index 0 to add on top.
                UAP.Items.Insert(0, new Items
                {
                    id = _index,
                    contribuyente = _contribuyente,
                    fecha_inicio = _fecha_inicio,
                    plazo_extendido = _plazo_extendido,
                    entrega_expediente = _entrega_expediente,
                    comite = _comite,
                    notificacion_atenta = _notificacion_atenta,
                    vencimiento_10_dias = _vencimiento_10_dias,
                    entrega_borrador_uap = _entrega_borrador_uap,
                    entrega_uap = _entrega_uap,
                    recepcion_uap = _recepcion_uap,
                    sengunda_vuelta_uap = _sengunda_vuelta_uap,
                    levantamiento_uap = _levantamiento_uap,
                    vence_uap = _vence_uap,
                    levantamiento_acta_final = _levantamiento_acta_final,
                    dias_estrados = _dias_estrados,
                    vence_plazo = _vence_plazo,
                    dias_sobrantes = _dias_sobrantes,
                    liquidacion = _liquidacion,
                    entrega_borrador_liquidacion = _entrega_borrador_liquidacion,
                    entrega_liquidacion = _entrega_liquidacion,
                    recepcion_liquidacion = _recepcion_liquidacion,
                    segunda_vuelta_liquidacion = _segunda_vuelta_liquidacion,
                    impresion_firma = _impresion_firma,
                    fecha_cierre = _fecha_cierre,
                    plazo_prodecon = _plazo_prodecon
                });

                xml.Add(xml.ReadIndex(), _contribuyente, _fecha_inicio, _plazo_extendido, _entrega_expediente, _comite, 
                    _notificacion_atenta, _vencimiento_10_dias, _entrega_borrador_uap, _entrega_uap, _recepcion_uap, 
                    _sengunda_vuelta_uap, _levantamiento_uap, _vence_uap, _levantamiento_acta_final, _dias_estrados, 
                    _vence_plazo, _dias_sobrantes, _liquidacion, _entrega_borrador_liquidacion, _entrega_liquidacion, 
                    _recepcion_liquidacion, _segunda_vuelta_liquidacion, _impresion_firma,
                    _fecha_cierre, _plazo_prodecon);
            }
            else
            {
                //Index to add on a especific position.
                UAP.Items.Insert(_index, new Items
                {
                    contribuyente = _contribuyente,
                    fecha_inicio = _fecha_inicio,
                    plazo_extendido = _plazo_extendido,
                    entrega_expediente = _entrega_expediente,
                    comite = _comite,
                    notificacion_atenta = _notificacion_atenta,
                    vencimiento_10_dias = _vencimiento_10_dias,
                    entrega_borrador_uap = _entrega_borrador_uap,
                    entrega_uap = _entrega_uap,
                    recepcion_uap = _recepcion_uap,
                    sengunda_vuelta_uap = _sengunda_vuelta_uap,
                    levantamiento_uap = _levantamiento_uap,
                    vence_uap = _vence_uap,
                    levantamiento_acta_final = _levantamiento_acta_final,
                    dias_estrados = _dias_estrados,
                    vence_plazo = _vence_plazo,
                    dias_sobrantes = _dias_sobrantes,
                    liquidacion = _liquidacion,
                    entrega_borrador_liquidacion = _entrega_borrador_liquidacion,
                    entrega_liquidacion = _entrega_liquidacion,
                    recepcion_liquidacion = _recepcion_liquidacion,
                    segunda_vuelta_liquidacion = _segunda_vuelta_liquidacion,
                    impresion_firma = _impresion_firma,
                    fecha_cierre = _fecha_cierre,
                    plazo_prodecon = _plazo_prodecon
                });

                UAP.Items.Remove(UAP.SelectedItem);

                xml.Update(_index, _contribuyente, _fecha_inicio, _plazo_extendido, _entrega_expediente, _comite,
                    _notificacion_atenta, _vencimiento_10_dias, _entrega_borrador_uap, _entrega_uap, _recepcion_uap,
                    _sengunda_vuelta_uap, _levantamiento_uap, _vence_uap, _levantamiento_acta_final, _dias_estrados,
                    _vence_plazo, _dias_sobrantes, _liquidacion, _entrega_borrador_liquidacion, _entrega_liquidacion,
                    _recepcion_liquidacion, _segunda_vuelta_liquidacion, _impresion_firma, _fecha_cierre, _plazo_prodecon);
            }
            
        }

        //Update the row selected
        private void UpdateItem()
        {
            Create.IsEnabled = false;
            Update.Visibility = Visibility.Visible;
            Cancel.Visibility = Visibility.Visible;
            nombreContribuyente.Text = ((Items)UAP.SelectedItem).contribuyente;
            fecha.SelectedDate = Convert.ToDateTime(((Items)UAP.SelectedItem).fecha_inicio.ToString());
            bool status;
            if((((Items)UAP.SelectedItem).plazo_extendido) == "Sí")
            {
                status = true;
            }
            else
            {
                status = false;
            }
            extendido.IsChecked = status;
            int index = UAP.Items.IndexOf(UAP.SelectedItem);
            aux = index;
        }

        //Clean all fields
        private void ClearFields()
        {
            nombreContribuyente.Clear();
            fecha.SelectedDate = null;
            extendido.IsChecked = false;
            Delete.Visibility = Visibility.Collapsed;
        }

        //Add days left
        private void AlertasVencimientoDePlazos()
        {
            int red_alert = 0;
            int yellow_alert = 0;
            int green_alert = 0;
            BrushConverter brush = new BrushConverter();
            DateTime today = DateTime.Now;

            foreach (AddLabel date in xml.ReadVencimientoPlazos())
            {
                try
                {
                    int days_left = date.vence_plazo.Subtract(today).Days + 1;
                    if (days_left == -1 || days_left == 0)
                    {
                        AddLabelContribuyente(date.contribuyente, true);
                        AddLabelVencio(Convert.ToString(date.vence_plazo.ToString("D")));
                        red_alert++;
                    }
                    else if (days_left > 0 && days_left <= 28)
                    {
                        AddLabelContribuyente(date.contribuyente, false);
                        AddLabelPorVencer(Convert.ToString(date.vence_plazo.ToString("D")));
                        yellow_alert++;
                    }
                    else
                    {
                        green_alert++;
                    }
                }
                catch (Exception)
                {
                    UI_ToastAlert.ShowAlert("Acceso fallido", "No se pudo acceder a algunas fechas", AlertType.error);
                }
            }

            if((yellow_alert > 0) || (red_alert > 0))
            {
                todoBien.Visibility = Visibility.Collapsed;
                if ((yellow_alert > 0) && (red_alert > 0))
                {
                    vencidoTitle.Visibility = Visibility.Visible;
                    porVencerTitle.Visibility = Visibility.Visible;
                    status.Background = (Brush)brush.ConvertFrom("#F93F3F");
                    status.BorderBrush = (Brush)brush.ConvertFrom("#F93F3F");
                    UI_ToastAlert.ShowAlert("Vencimiento", "Hay información de fechas disponible", AlertType.info);
                }
                else if(yellow_alert > 0)
                {
                    porVencerTitle.Visibility = Visibility.Visible;
                    vencidoTitle.Visibility = Visibility.Collapsed;
                    status.Background = (Brush)brush.ConvertFrom("#FFD423");
                    status.BorderBrush = (Brush)brush.ConvertFrom("#FFD423");
                    UI_ToastAlert.ShowAlert("Advertencia de plazos!", "Algunos plazos estan por vencer", AlertType.warning);
                }
                else
                {
                    porVencerTitle.Visibility = Visibility.Collapsed;
                    vencidoTitle.Visibility = Visibility.Visible;
                    status.Background = (Brush)brush.ConvertFrom("#F93F3F");
                    status.BorderBrush = (Brush)brush.ConvertFrom("#F93F3F");
                    UI_ToastAlert.ShowAlert("Plazos vencidos!", "Algunos plazos ya han vencido", AlertType.error);
                }
            }
            else if(green_alert > 0)
            {
                vencidoTitle.Visibility = Visibility.Collapsed;
                porVencerTitle.Visibility = Visibility.Collapsed;
                todoBien.Visibility = Visibility.Visible;
                status.Background = (Brush)brush.ConvertFrom("#10CB70");
                status.BorderBrush = (Brush)brush.ConvertFrom("#10CB70");
            }
            else
            {
                vencidoTitle.Visibility = Visibility.Collapsed;
                porVencerTitle.Visibility = Visibility.Collapsed;
                todoBien.Visibility = Visibility.Collapsed;
                status.Background = (Brush)brush.ConvertFrom("#BEBEBE");
                status.BorderBrush = (Brush)brush.ConvertFrom("#BEBEBE");
            }
        }

        //Add new label to panel
        private void AddLabelContribuyente(string _content, bool _vencio)
        {
            Label new_label = new Label();
            new_label.Content = String.Format("* {0}", _content);
            new_label.Foreground = new SolidColorBrush(Colors.Black);
            new_label.FontSize = 14;
            new_label.FontFamily = new FontFamily("Century Gothic");
            if (_vencio)
            {
                vencio.Children.Add(new_label);
            }
            else
            {
                porVencer.Children.Add(new_label);
            }
        }

        //Add new label to panel
        private void AddLabelPorVencer(string _content)
        {
            Label new_label = new Label();
            new_label.Content = String.Format("{0}", _content);
            new_label.Foreground = new SolidColorBrush(Colors.Black);
            new_label.FontSize = 14;
            new_label.FontFamily = new FontFamily("Century Gothic");
            porVencer.Children.Add(new_label);
        }

        //Add new label to panel
        private void AddLabelVencio(string _content)
        {
            Label new_label = new Label();
            new_label.Content = String.Format("{0}", _content);
            new_label.Foreground = new SolidColorBrush(Colors.Black);
            new_label.FontSize = 14;
            new_label.FontFamily = new FontFamily("Century Gothic");
            vencio.Children.Add(new_label);
        }
    }
}

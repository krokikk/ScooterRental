using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScooterRental.Pages
{
    public partial class DiagrammPage : System.Windows.Controls.Page
    {
        private Chart ChartPayments;

        public DiagrammPage()
        {
            InitializeComponent();

            ChartPayments = new Chart();

            ChartArea area = new ChartArea("Main");
            area.AxisX.Title = "Поездки пользователей";
            area.AxisY.Title = "Сумма поездки";
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisY.Minimum = 0;

            ChartPayments.ChartAreas.Add(area);
            ChartPayments.Legends.Add(new Legend("MainLegend"));

            ChartHost.Child = ChartPayments;

            CmbDiagram.ItemsSource = new SeriesChartType[]
            {
                SeriesChartType.Column,
                SeriesChartType.Bar,
                SeriesChartType.Line,
                SeriesChartType.Pie,
                SeriesChartType.Doughnut
            };

            CmbDiagram.SelectedItem = SeriesChartType.Column;
        }

        private void UpdateChart(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (CmbDiagram.SelectedItem == null)
                return;

            SeriesChartType chartType =
                (SeriesChartType)CmbDiagram.SelectedItem;

            ChartPayments.Series.Clear();

            Series series = new Series("Суммы поездок");
            series.ChartType = chartType;
            series.IsValueShownAsLabel = true;
            series.ChartArea = "Main";
            series.Legend = "MainLegend";

            ChartPayments.Series.Add(series);

            // Получаем все поездки всех пользователей
            var trips = Core.Context.Rentals
                .OrderBy(r => r.Start_time)
                .ToList();

            foreach (var trip in trips)
            {
                string userName = trip.User != null
                    ? trip.User.FIO
                    : "Неизвестный пользователь";

                int pointIndex = series.Points.AddXY(
                    "№" + trip.ID + " " + userName,
                    trip.Sum);

                series.Points[pointIndex].ToolTip =
                    "Поездка №" + trip.ID +
                    "\nПользователь: " + userName +
                    "\nСумма: " + trip.Sum +
                    "\nДата: " +
                    trip.Start_time.ToString("dd.MM.yyyy HH:mm");
            }

            ChartPayments.Titles.Clear();
            ChartPayments.Titles.Add("Суммы всех поездок пользователей");

            if (trips.Count == 0)
            {
                ChartPayments.Titles.Add("Поездок пока нет");
            }

            ChartPayments.ChartAreas["Main"].RecalculateAxesScale();
            ChartPayments.Invalidate();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null &&
                NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
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
using Microsoft.Win32;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace ScooterRental.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
        }


        private void WordButton_Click(object sender, RoutedEventArgs e)
        {
            var allUsers = Core.Context.User.ToList();

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Документ Word (*.docx)|*.docx",
                FileName = "Отчет_ScooterRental.docx"
            };

            if (dialog.ShowDialog() != true)
                return;

            Word.Application application = new Word.Application();
            Word.Document document = application.Documents.Add();

            Word.Paragraph title = document.Paragraphs.Add();
            title.Range.Text = "Отчёт ScooterRental";
            title.Range.Font.Name = "Times New Roman";
            title.Range.Font.Size = 18;
            title.Range.Font.Bold = 1;
            title.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            title.Range.InsertParagraphAfter();

            Word.Paragraph dateParagraph = document.Paragraphs.Add();
            dateParagraph.Range.Text = "Дата формирования: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            dateParagraph.Range.Font.Name = "Times New Roman";
            dateParagraph.Range.Font.Size = 11;
            dateParagraph.Range.InsertParagraphAfter();

            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;

            Word.Table table = document.Tables.Add(tableRange, allUsers.Count + 1, 6);

            table.Borders.InsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;

            string[] headers = {"ID", "ФИО", "Логин", "Телефон", "Роль", "Подписка"};

            for (int col = 0; col < headers.Length; col++)
            {
                table.Cell(1, col + 1).Range.Text = headers[col];
                table.Cell(1, col + 1).Range.Font.Bold = 1;
                table.Cell(1, col + 1).Range.Font.Size = 11;
            }

            for (int i = 0; i < allUsers.Count; i++)
            {
                var user = allUsers[i];

                table.Cell(i + 2, 1).Range.Text = user.ID.ToString();
                table.Cell(i + 2, 2).Range.Text = user.FIO ?? "";
                table.Cell(i + 2, 3).Range.Text = user.Login ?? "";
                table.Cell(i + 2, 4).Range.Text = user.Phone_number ?? "";
                table.Cell(i + 2, 5).Range.Text = user.Roles?.Name ?? "";
                table.Cell(i + 2, 6).Range.Text = user.Subscription?.Name ?? "";
            }

            table.Range.Font.Name = "Times New Roman";
            table.Range.Font.Size = 10;

            foreach (Word.Section section in document.Sections)
            {
                Word.HeaderFooter header = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];

                header.Range.Text = "ScooterRental | Отчёт от " + DateTime.Now.ToString("dd.MM.yyyy");
                header.Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight;
                header.Range.Font.Size = 9;

                Word.HeaderFooter footer = section.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];

                footer.PageNumbers.Add(Word.WdPageNumberAlignment.wdAlignPageNumberCenter);
            }

            document.SaveAs2(dialog.FileName);
            application.Visible = true;
        }


        private void ExcelButton_Click(object sender, RoutedEventArgs e)
        {
            var users = Core.Context.User.ToList();
            var trips = Core.Context.Rentals.ToList();

            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = "Книга Excel (*.xlsx)|*.xlsx",
                FileName = "Отчет_ScooterRental.xlsx"
            };

            if (dialog.ShowDialog() != true)
                return;

            Excel.Application application = new Excel.Application();
            Excel.Workbook workbook = null;

                workbook = application.Workbooks.Add();
                while (workbook.Worksheets.Count > 1)
                {
                    Excel.Worksheet sheet = (Excel.Worksheet)workbook.Worksheets[workbook.Worksheets.Count];

                    sheet.Delete();
                }

                Excel.Worksheet usersSheet = (Excel.Worksheet)workbook.Worksheets[1];

                usersSheet.Name = "Пользователи";

                usersSheet.Cells[1, 1] = "Отчёт по пользователям";
                usersSheet.Cells[1, 1].Font.Bold = true;
                usersSheet.Cells[1, 1].Font.Size = 16;

                string[] userHeaders =
                { "ID", "ФИО", "Логин", "Телефон", "Роль", "Подписка", "Статус", "Есть штраф", "Сумма штрафа"
        };

                for (int col = 0; col < userHeaders.Length; col++)
                    usersSheet.Cells[3, col + 1] = userHeaders[col];

                for (int i = 0; i < users.Count; i++)
                {
                    var user = users[i];
                    int row = i + 4;

                    usersSheet.Cells[row, 1] = user.ID;
                    usersSheet.Cells[row, 2] = user.FIO ?? "";
                    usersSheet.Cells[row, 3] = user.Login ?? "";
                    usersSheet.Cells[row, 4] = user.Phone_number ?? "";
                    usersSheet.Cells[row, 5] = user.Roles?.Name ?? "";
                    usersSheet.Cells[row, 6] = user.Subscription?.Name ?? "";
                    usersSheet.Cells[row, 7] = user.UserStatus.ToString();
                    usersSheet.Cells[row, 8] =
                        user.IsFine == true ? "Да" : "Нет";
                    usersSheet.Cells[row, 9] =
                        Convert.ToDecimal(user.FineSum);
                }

                Excel.Range userHeaderRange = usersSheet.Range[
                    usersSheet.Cells[3, 1],
                    usersSheet.Cells[3, userHeaders.Length]];

                userHeaderRange.Font.Bold = true;
                userHeaderRange.WrapText = true;

                if (users.Count > 0)
                {
                    Excel.Range userTableRange = usersSheet.Range[
                        usersSheet.Cells[3, 1],
                        usersSheet.Cells[users.Count + 3, userHeaders.Length]];

                    userTableRange.Borders.LineStyle =
                        Excel.XlLineStyle.xlContinuous;
                }

                usersSheet.Columns.AutoFit();

                Excel.Worksheet tripsSheet = (Excel.Worksheet)workbook.Worksheets.Add(Type.Missing,usersSheet);

                tripsSheet.Name = "Поездки";

                tripsSheet.Cells[1, 1] = "Отчёт по поездкам";
                tripsSheet.Cells[1, 1].Font.Bold = true;
                tripsSheet.Cells[1, 1].Font.Size = 16;

                string[] tripHeaders =
                { "ID поездки", "ID пользователя", "ФИО пользователя", "ID самоката", "Самокат", "Время начала", "Время окончания", "ID тарифа", "Тариф", "Сумма", "Статус оплаты"
        };

                for (int col = 0; col < tripHeaders.Length; col++)
                    tripsSheet.Cells[3, col + 1] = tripHeaders[col];


                for (int i = 0; i < trips.Count; i++)
                {
                    var trip = trips[i];
                    int row = i + 4;

                    tripsSheet.Cells[row, 1] = trip.ID;
                    tripsSheet.Cells[row, 2] = trip.UserID;
                    tripsSheet.Cells[row, 3] = trip.User?.FIO ?? "";
                    tripsSheet.Cells[row, 4] = trip.VechicleID;
                    tripsSheet.Cells[row, 5] = trip.VechicleID;
                    tripsSheet.Cells[row, 6] = trip.Start_time.ToString("dd.MM.yyyy HH:mm");
                    tripsSheet.Cells[row, 7] = trip.End_time.ToString("dd.MM.yyyy HH:mm");
                    tripsSheet.Cells[row, 8] = trip.TarifID;
                    tripsSheet.Cells[row, 9] = trip.TarifID;               
                    tripsSheet.Cells[row, 10] = trip.Sum;      
                    tripsSheet.Cells[row, 11] = trip.PayStatusID;
                }


                Excel.Range tripHeaderRange = tripsSheet.Range[
                    tripsSheet.Cells[3, 1],
                    tripsSheet.Cells[3, tripHeaders.Length]];

                tripHeaderRange.Font.Bold = true;
                tripHeaderRange.WrapText = true;

                if (trips.Count > 0)
                {
                    Excel.Range tripTableRange = tripsSheet.Range[
                        tripsSheet.Cells[3, 1],
                        tripsSheet.Cells[trips.Count + 3, tripHeaders.Length]];

                    tripTableRange.Borders.LineStyle =
                        Excel.XlLineStyle.xlContinuous;
                }

                tripsSheet.Columns.AutoFit();
                

                workbook.SaveAs( dialog.FileName, Excel.XlFileFormat.xlOpenXMLWorkbook);
 
                application.Visible = true;

                MessageBox.Show("Отчёт Excel успешно создан!");
            
            
        }




        private void ChartButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DiagrammPage());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

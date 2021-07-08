using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace Pharm.Classes
{
    class ExcelHelper
    {
        Excel.Application application;
        Task CreateNewApp;
        Excel.Workbook workbook;
        Excel.Worksheet sheets;
        DataTable ReadDataTable = new DataTable();

        public ExcelHelper()
        {
            if (application == null)
                CreateNewApp = Task.Run(delegate ()
                {
                    application = new Excel.Application();
                });
        }

        public void CloseExcelFile()
        {
            application.Quit();
        }

        public async void Write(DataTable dataTable, string[] columns, string path)
        {
            try
            {
                await Task.Run(delegate ()
                {
                    CreateNewApp.Wait();
                    workbook = application.Workbooks.Add();
                    sheets = workbook.ActiveSheet;

                    Parallel.Invoke(() =>
                    {
                        Parallel.For(1, dataTable.Columns.Count + 1, (int j) =>
                        {
                            sheets.Cells[1, j] = columns[j - 1];
                            (sheets.Columns[j] as Excel.Range).AutoFit();
                        });
                    },
                    () =>
                    {
                        Parallel.For(2, dataTable.Rows.Count + 2, (int i) =>
                        {
                            Parallel.For(1, dataTable.Columns.Count + 1, (int j) =>
                            {
                                sheets.Cells[i, j] = dataTable.Rows[i - 2].ItemArray[j - 1].ToString();
                                (sheets.Columns[j] as Excel.Range).AutoFit();
                            });
                        });
                    });

                    Excel.Range Range = sheets.UsedRange;
                    Range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    Range.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                    Excel.ChartObjects chartObjs = (Excel.ChartObjects)sheets.ChartObjects();
                    Excel.ChartObject chartObj = chartObjs.Add(1500, 10, 300, 200);
                    Excel.Chart xlChart = chartObj.Chart;
                    //Range = sheets.Range[$"M1:N{dataTable.Rows.Count + 1}"];

                    Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)xlChart.SeriesCollection(Type.Missing);
                    Excel.Series series = seriesCollection.NewSeries();
                    series.XValues = sheets.get_Range("K2", $"K{dataTable.Columns.Count}");
                    series.Values = sheets.get_Range("I2", $"I{dataTable.Rows.Count + 1}");

                    xlChart.ChartType = Excel.XlChartType.xl3DColumn;
                    //xlChart.SetSourceData(Range);
                    xlChart.HasLegend = true;

                    workbook.SaveAs(path);

                    workbook.Close(false);
                    CloseExcelFile();

                    MessageBox.Show($"Файл успешно сохранён!", "Результат");
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}

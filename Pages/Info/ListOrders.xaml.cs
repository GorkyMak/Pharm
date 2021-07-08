using Microsoft.Win32;
using Pharm.Classes;
using Pharm.Database;
using Pharm.Pages.Edit;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using System.Windows.Controls;
using System.Windows;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace Pharm.Pages
{
    public partial class ListOrders : System.Windows.Controls.Page
    {
        List<Заказ> orders, searchResult;
        double maxCost = 0, maxDiscount = 100;
        DateTime maxValiditySocCard = new DateTime();
        word.Application app;
        word.Document doc;
        string[] columnsFirst = new string[] { "Наименование","Документ", "Сумма, руб., коп.",
        "Отметки бухгалтерии"},
        columnsSecond = new string[] { "дата", "номер", "товара", "тары" },
        columnsThird = new string[] { "Код заказа", "Код сотрудника", "ФИО сотрудника", "Код социальной карты", "ФИО владельца",
        "Срок действия", "Скидка", "Дата и время исполнения", "Стоимость", "Код препарата", "Название препарата", 
            "Количество препарата" };
        string startDateRange = "", endDateRange = "";


        public ListOrders()
        {
            InitializeComponent();

            if(Properties.Settings.Default.UserRole != "")
            {
                if(Properties.Settings.Default.UserRole != "Администратор" && Properties.Settings.Default.UserRole != "Заведующий аптекой")
                {
                    BtnCreateOtchetWord.Visibility = Visibility.Collapsed;
                    BtnCreateOtchetExcel.Visibility = Visibility.Collapsed;
                    MIDelete.Visibility = Visibility.Collapsed;
                }
            }

            SetMaxValue();
            SetCmbFilterProperties();
            GetOrders();
            SetOrders();
        }

        private void SetCmbFilterProperties()
        {
            CmbFilterOrders.SelectionChanged += CmbFilterOrders_SelectionChanged;
            CmbFilterOrders.SelectedIndex = 0;
        }

        private void CmbFilterOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((CmbFilterOrders.SelectedItem as ComboBoxItem).Content)
            {
                case "по стоимости":
                    SldMax.Maximum = maxCost;
                    SldMax.Value = maxCost;
                    SldMin.TickFrequency = 100;
                    SldMax.TickFrequency = 100;
                    break;

                case "по сроку действия карты":
                    DtPcEndDate.SelectedDate = maxValiditySocCard;
                    break;

                case "по скидке":
                    SldMax.Maximum = maxDiscount;
                    SldMax.Value = maxDiscount;
                    SldMin.TickFrequency = 5;
                    SldMax.TickFrequency = 5;
                    break;

                case "по дате и времени исполнения":
                    DtPcEndDate.SelectedDate = DateTime.Today;
                    break;
            }
        }

        private void SetMaxValue()
        {
            using (var dbcontext = new АптекаEntities())
            {
                maxValiditySocCard = dbcontext.Социальная_карта.Count() == 0 ? DateTime.Today : 
                    dbcontext.Социальная_карта.Max(i => i.Срок_действия);
                maxCost = dbcontext.Заказ.Count() == 0 ? 0 :
                    (double)dbcontext.Заказ.Max(i => i.Стоимость__);
            }
        }

        private void RefreshDG()
        {
            GetOrders();
            SetOrders();
        }

        private void SetOrders()
        {
            DGOrders.ItemsSource = orders;
        }

        private void GetOrders()
        {
            using (var dbcontext = new АптекаEntities())
                orders = new List<Заказ>(dbcontext.Заказ
                    .AsNoTracking()
                    .Include(i => i.Сотрудник.Личные_данные)
                    .Include(i => i.Социальная_карта.Личные_данные)
                    .Include(i => i.Заказ_Препарат.Select(j => j.Препарат)));
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditOrders());
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DGOrders.SelectedItem == null)
                return; 
            
            NavigationService.Navigate(new EditOrders(DGOrders.SelectedItem as Заказ));
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DGOrders.SelectedItem == null)
                return;

            using (var dbcontext = new АптекаEntities())
            {
                var record = DGOrders.SelectedItem as Заказ;
                dbcontext.Entry(record).State = EntityState.Deleted;

                dbcontext.Заказ.Remove(record);
                dbcontext.SaveChanges();
            }

            RefreshDG();
        }

        private void BtnCreateOtchetExcel_Click(object sender, RoutedEventArgs e)
        {
            var dataTableExcel = GetDataTableExcel();

            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить файл как..";
            savedialog.OverwritePrompt = false;
            savedialog.Filter = "Книга Excel (*.xlsx)|*.xlsx";

            if (savedialog.ShowDialog() != true)
                return;

            ExcelHelper excelHelper = new ExcelHelper();
            excelHelper.Write(dataTableExcel, columnsThird, savedialog.FileName);
        }

        private System.Data.DataTable GetDataTableExcel()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            DataRow dataRow;

            while (dataTable.Columns.Count < 12)
                dataTable.Columns.Add();

            for (int i = 0; i < orders.Count; i++)
            {
                dataRow = dataTable.NewRow();
                var order_Med = orders[i].Заказ_Препарат.ToList();
                for (int j = 0; j < order_Med.Count; j++)
                {
                    dataRow.ItemArray = new string[]
                    {
                        orders[i].Код_заказа.ToString(),
                        orders[i].Код_сотрудника.ToString(),
                        orders[i].Сотрудник.Личные_данные.ToString(),
                        orders[i].Социальная_карта?.Код_социальной_карты == null ? "" : orders[i].Социальная_карта.Код_социальной_карты,
                        orders[i].Социальная_карта?.ToString() == null ? "" : orders[i].Социальная_карта.Личные_данные.ToString(),
                        orders[i].Социальная_карта?.Срок_действия.ToString() == null ? "" : orders[i].Социальная_карта.Срок_действия.ToString(),
                        orders[i].Социальная_карта?.Скидка.ToString() == null ? "" : orders[i].Социальная_карта.Скидка.ToString(),
                        orders[i].Дата_и_время_исполнения.ToString(),
                        orders[i].Стоимость__.ToString().Replace(',','.'),
                        order_Med[j].Код_препарата.ToString(),
                        order_Med[j].Препарат.Название.ToString(),
                        order_Med[j].Количество_препарата.ToString()
                    };

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        private void MICreateOtchet_Click(object sender, RoutedEventArgs e)
        {
            System.Data.DataTable dataTableExp = GetDataTable();
            Save(dataTableExp);
        }

        private System.Data.DataTable GetDataTable()
        {
            System.Data.DataTable dataTable = new System.Data.DataTable();
            DataRow dataRow;

            while (dataTable.Columns.Count < 7)
                dataTable.Columns.Add();

            for (int i = 0; i < orders.Count; i++)
            {
                dataRow = dataTable.NewRow();
                dataRow.ItemArray = new string[]
                {
                    "Продано в розницу",
                    orders[i].Дата_и_время_исполнения.ToShortDateString(),
                    orders[i].Код_заказа.ToString(),
                    orders[i].Стоимость__.ToString(),
                    "—",
                    "",
                    ""
                };

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        void Save(System.Data.DataTable dataTable)
        {
            word.Bookmarks bookmarks = null;
            if (searchResult == null)
                searchResult = orders;

            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить файл как...";
            savedialog.Filter = "Документ Word (*.docx)|*.docx|" +
                "Документ Word 97-2003 (*.doc)|*.doc|" +
                "PDF (*.pdf)|*.pdf|" +
                "Текст в формате RTF (*.rtf)|*.rtf|" +
                "Текст OpenDocument (*.odt)|*.odt";

            try
            {
                System.Threading.Tasks.Task TaskCreateNewApp = System.Threading.Tasks.Task.Run(() =>
                    app = new word.Application());

                if (savedialog.ShowDialog() == false)
                {
                    if (doc != null)
                    {
                        doc.Close(false);
                        doc = null;
                    }
                    TaskCreateNewApp.Wait();
                    app.Quit();
                    return;
                }

                TaskCreateNewApp.Wait();

                string source = $@"{Directory.GetCurrentDirectory()}/Шаблон отчёта.docx";
                Сотрудник заведующий;
                doc = app.Documents.Open(source);
                doc.Activate();
                bookmarks = doc.Bookmarks;
                Random random = new Random();

                using (var dbcontext = new АптекаEntities())
                    заведующий = dbcontext.Сотрудник
                        .Include(i => i.Личные_данные)
                        .First(i => i.Должность.Название_должности == "Заведующий аптекой");

                bookmarks["Номер_документа"].Range.Text = random.Next(10000000, 99999999).ToString();
                bookmarks["Дата_состовления"].Range.Text = DateTime.Now.ToShortDateString();
                bookmarks["Отчётный_период_с"].Range.Text = searchResult.Min(i => i.Дата_и_время_исполнения).ToShortDateString();
                bookmarks["Отчётный_период_по"].Range.Text = searchResult.Max(i => i.Дата_и_время_исполнения).ToShortDateString();
                bookmarks["Табельный_номер"].Range.Text = заведующий.Код_сотрудника.ToString();
                bookmarks["Ответсвенное_лицо"].Range.Text = заведующий.Личные_данные.ToString();
                bookmarks["Отчёт_принял"].Range.Text = заведующий.Личные_данные.ToString();
                bookmarks["Матер_отвт_лицо"].Range.Text = заведующий.Личные_данные.ToString();
                word.Range range = bookmarks["Таблица"].Range;


                var Table = doc.Tables.Add(range, searchResult.Count + 5, 7, 7);


                Table.Cell(1, 1).Range.Text = columnsFirst[0];
                Table.Cell(1, 2).Range.Text = columnsFirst[1];
                Table.Cell(1, 4).Range.Text = columnsFirst[2];
                Table.Cell(1, 6).Range.Text = columnsFirst[3];
                
                Table.Cell(2, 2).Range.Text = columnsSecond[0];
                Table.Cell(2, 3).Range.Text = columnsSecond[1];
                Table.Cell(2, 4).Range.Text = columnsSecond[2];
                Table.Cell(2, 5).Range.Text = columnsSecond[3];

                Table.Cell(1, 2).Merge(Table.Cell(1, 3));
                Table.Cell(1, 3).Merge(Table.Cell(1, 4));
                Table.Cell(1, 4).Merge(Table.Cell(1, 5));
                Table.Cell(2, 6).Merge(Table.Cell(2, 7));

                Table.Cell(1, 1).Merge(Table.Cell(2, 1));
                Table.Cell(1, 4).Merge(Table.Cell(2, 6));

                Table.Cell(3, 1).Range.Text = "Расход";
                Table.Cell(3, 2).Range.Text = "X";
                Table.Cell(3, 3).Range.Text = "X";

                Parallel.Invoke(() =>
                {
                    Parallel.For(1, 8, (int i) =>
                    {
                        Table.Cell(3, i).Range.Text = i.ToString();
                    });
                },
                () =>
                {
                    Parallel.For(0, dataTable.Rows.Count, (int i) =>
                    {
                        Parallel.For(0, dataTable.Columns.Count, (int j) =>
                        {
                            Table.Cell(i + 5, j + 1).Range.Text = dataTable.Rows[i].ItemArray.ElementAt(j).ToString();
                        });
                    });
                });

                Table.Cell(searchResult.Count + 5, 1).Range.Text = "Итого по расходу";
                Table.Cell(searchResult.Count + 5, 2).Range.Text = "X";
                Table.Cell(searchResult.Count + 5, 3).Range.Text = "X";
                Table.Cell(searchResult.Count + 5, 4).Range.Text = orders.Sum(i => i.Стоимость__).ToString();
                Table.Cell(searchResult.Count + 5, 5).Range.Text = "—";

                if (doc != null && app != null)
                {
                    string format = savedialog.FileName.Substring(savedialog.FileName.Length - 4);

                    if (format == ".pdf")
                        doc.SaveAs2(savedialog.FileName, WdSaveFormat.wdFormatPDF);
                    else if (format == ".doc")
                        doc.SaveAs2(savedialog.FileName, WdSaveFormat.wdFormatDocument);
                    else if (format == ".rtf")
                        doc.SaveAs2(savedialog.FileName, WdSaveFormat.wdFormatRTF);
                    else if (format == ".odt")
                        doc.SaveAs2(savedialog.FileName, WdSaveFormat.wdFormatOpenDocumentText);
                    else
                        doc.SaveAs2(savedialog.FileName);
                    MessageBox.Show($"Отчёт успешно создан!", "Результат");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            finally
            {
                if (doc != null && app != null)
                {
                    doc.Close(false);
                    doc = null;
                    app.Quit();
                }
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            searchResult = orders;
            string searchText = TbSearchText.Text.ToLower();

            switch (CmbFilterOrders.Text)
            {
                case "по стоимости":
                    searchResult = searchResult.Where(i =>
                    i.Стоимость__ >= (decimal)SldMin.Value &&
                    i.Стоимость__ <= (decimal)SldMax.Value).ToList();
                    break;

                case "по сроку действия карты":
                    searchResult = searchResult.Where(i =>
                    i.Социальная_карта != null &&
                    i.Социальная_карта.Срок_действия >= DtPcStartDate.SelectedDate &&
                    i.Социальная_карта.Срок_действия <= DtPcEndDate.SelectedDate).ToList();
                    break;

                case "по скидке":
                    searchResult = searchResult.Where(i =>
                    i.Социальная_карта != null &&
                    i.Социальная_карта.Скидка >= (decimal)SldMin.Value &&
                    i.Социальная_карта.Скидка <= (decimal)SldMax.Value).ToList();
                    break;

                case "по дате и времени исполнения":
                    if (!DataChecker.CheckCorrectTime(TbStartTime.Text, TbEndTime.Text))
                    {
                        MessageBox.Show("Неверный формат времени", "Ошибка");
                        return;
                    }

                    var startDateTime = DtPcStartDate.SelectedDate.Value
                                        .AddHours(DateTime.Parse(TbStartTime.Text).Hour)
                                        .AddMinutes(DateTime.Parse(TbStartTime.Text).Minute);
                    var endDateTime = DtPcEndDate.SelectedDate.Value
                                      .AddHours(DateTime.Parse(TbEndTime.Text).Hour)
                                      .AddMinutes(DateTime.Parse(TbEndTime.Text).Minute);

                    searchResult = searchResult.Where(i =>
                    i.Дата_и_время_исполнения >= startDateTime &&
                    i.Дата_и_время_исполнения <= endDateTime).ToList();
                    break;
            }

            switch (CmbSearchOrders.Text)
            {
                case "по коду заказа":
                    searchResult = searchResult.Where(i => i.Код_заказа.ToString().Contains(searchText)).ToList();
                    break;

                case "по коду сотрудника":
                    searchResult = searchResult.Where(i => i.Код_сотрудника.ToString().Contains(searchText)).ToList();
                    break;

                case "по ФИО сотрудника":
                    searchResult = searchResult.Where(i => i.Сотрудник.Личные_данные.ToString().ToLower().Contains(searchText)).ToList();
                    break;

                case "по коду социальной карты":
                    searchResult = searchResult.Where(i => i.Социальная_карта != null &&
                    i.Социальная_карта.Код_социальной_карты.Contains(searchText)).ToList();
                    break;

                case "по ФИО владельца":
                    searchResult = searchResult.Where(i => i.Социальная_карта != null &&
                    i.Социальная_карта.ToString().ToLower().Contains(searchText)).ToList();
                    break;
            }

            DGOrders.ItemsSource = searchResult;

            if (searchResult.Count == 0)
                return;

            startDateRange = searchResult.Min(i => i.Дата_и_время_исполнения).ToString();
            endDateRange = searchResult.Max(i => i.Дата_и_время_исполнения).ToString();
        }
    }
}
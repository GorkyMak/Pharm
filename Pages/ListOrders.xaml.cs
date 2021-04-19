using Pharm.Classes;
using Pharm.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListOrders.xaml
    /// </summary>
    public partial class ListOrders : Page
    {
        List<Orders> Orders;
        public ListOrders()
        {
            InitializeComponent();
            GetEmployees();
            DGOrders.ItemsSource = Orders;
        }
        private void GetEmployees()
        {
            Orders = new List<Orders>();
            using (var dbcontext = new АптекаEntities())
            {
                var temp = dbcontext.Заказ.ToList();
                foreach (var item in temp)
                {
                    var order = new Orders() 
                    {
                        Код_заказа = item.Код_заказа,
                        Код_сотрудника = item.Код_сотрудника,
                        ФИО_сотрудника = item.Сотрудник.Личные_данные.Фамилия + " " +
                            item.Сотрудник.Личные_данные.Имя[0] + "." +
                            item.Сотрудник.Личные_данные.Отчество[0] + ".",
                        Дата_и_время_исполнения = item.Дата_и_время_исполнения,
                        Стоимость__ = item.Стоимость__,
                        Препаратs = new List<Middle_Medications>(),
                    };

                    if(item.Код_социальной_карты != null)
                    {
                        order.Код_социальной_карты = item.Код_социальной_карты;
                        order.ФИО_владельца = item.Социальная_карта.Фамилия + " " +
                            item.Социальная_карта.Имя[0] + "." +
                            item.Социальная_карта.Отчество[0] + ".";
                        order.Срок_действия = item.Социальная_карта.Срок_действия;
                        order.Скидка = item.Социальная_карта.Скидка;
                    }

                    var ord_meds = item.Заказ_Препарат.ToList();
                    foreach (var record in ord_meds)
                        order.Препаратs.Add(new Middle_Medications() 
                        {
                            Код_препарата = record.Код_препарата,
                            Название_препарата = record.Препарат.Название,
                            Количество_препарата = record.Количество_препарата,
                        });

                    Orders.Add(order);
                }
            }
        }
    }
    class Orders
    {
        public int Код_заказа { get; set; }
        public int Код_сотрудника { get; set; }
        public string ФИО_сотрудника { get; set; }
        public int? Код_социальной_карты { get; set; }
        public string ФИО_владельца { get; set; }
        public DateTime Срок_действия { get; set; }
        public decimal Скидка { get; set; }
        public DateTime Дата_и_время_исполнения { get; set; }
        public decimal? Стоимость__ { get; set; }
        public List<Middle_Medications> Препаратs { get; set; }
    }
}
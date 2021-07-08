using Pharm.Database;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Pharm.Classes
{
    static public class DataChecker
    {
        public static bool CheckFields(UIElementCollection collection)
        {
            foreach (var item in collection)
                if (!CheckField(item))
                    return false;

            return true;
        }

        public static bool CheckFieldsExcept(UIElementCollection collection, params string[] exceptNames)
        {
            foreach (var item in collection)
            {
                if (Contains(GetNameObject(item), ref exceptNames))
                    continue;

                if (item is TextBox textBox && textBox.Text == "")
                    if (!CheckField(item))
                        return false;
            }

            return true;
        }

        private static string GetNameObject(object item) =>
            (item as FrameworkElement).Name;

        private static bool CheckField(object item)
        {
            if
            (
                item is TextBox textBox && (textBox.Text == "" || textBox.Text == "0") ||
                item is ComboBox comboBox && comboBox.Text == ""
            )
            {
                MessageBox.Show("Заполните поля", "Ошибка");
                return false;
            }

            return true;
        }

        private static bool Contains(string name, ref string[] exceptNames)
        {
            foreach (var i in exceptNames)
                if (name == i)
                    return true;

            return false;
        }

        public static bool CheckDataGrid(ICollection<Препарат_Поставка> препарат_Поставка)
        {
            if (препарат_Поставка.Count != 0)
                return true;

            MessageBox.Show("Заполните список препаратов", "Ошибка");
            return false;
        }

        public static bool CheckDataGrid(ICollection<Заказ_Препарат> заказ_Препарат)
        {
            if (заказ_Препарат.Count != 0)
                return true;

            MessageBox.Show("Заполните список препаратов", "Ошибка");
            return false;
        }

        public static bool CheckFieldSearch(UIElementCollection collection)
        {
            foreach (var item in collection)
            {
                if (item is ComboBox comboBox && comboBox.Text != "" ||
                    item is TextBox textBox && textBox.Text != "" ||
                    item is CheckBox checkBox && (bool)checkBox.IsChecked)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckCorrectTime(params string[] times)
        {
            Regex CorrectDateTime = new Regex(@"^(\d|[0-1]?\d|2[0-3]):([0-5]\d)$");

            foreach (var item in times)
                if (!CorrectDateTime.IsMatch(item))
                    return false;

            return true;
        }
    }
}

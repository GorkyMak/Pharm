﻿using Pharm.Database;
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

namespace Pharm.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListWarehouses.xaml
    /// </summary>
    public partial class ListWarehouses : Page
    {
        public ListWarehouses()
        {
            InitializeComponent();
            using (var dbcontext = new АптекаEntities())
                DGWarehouses.ItemsSource = dbcontext.Склад.ToList();
        }
    }
}

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
using AutoBook.Entities;

namespace AutoBook.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get data from somewhere and fill in my local ArrayList
            var myDataList = LoadListBoxData();
            // Bind ArrayList with the ListBox
            MyListBox.ItemsSource = myDataList;
        }

        /// <summary>
        /// Generate data. This method can bring data from a database or XML file
        /// or from a Web service or generate data dynamically
        /// </summary>
        /// <returns></returns>
        private IList<TynemouthBookingSlot> LoadListBoxData()
        {
            Court courts = new Court();
            courts.LoadData();

            return courts.Slots;
        }
    }
}

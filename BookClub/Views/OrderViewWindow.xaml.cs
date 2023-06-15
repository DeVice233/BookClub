using BookClub.Models;
using BookClub.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookClub.Views
{
    /// <summary>
    /// Логика взаимодействия для OrderViewWindow.xaml
    /// </summary>
    public partial class OrderViewWindow : Window
    {
        public OrderViewWindow(ObservableCollection<ProductOrder> productOrders)
        {
            InitializeComponent();
            DataContext = new OrderViewModel(productOrders);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = (TextBox)sender;
            ProductOrder item = (ProductOrder)text.DataContext;
            if (item != null)
            {
                if (item.Quantity == 0)
                {
                    OrderViewModel viewModel = (OrderViewModel)this.DataContext;
                    viewModel.RemoveItem(item);
                }
            }
        }
    }
}

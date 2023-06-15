using BookClub.Core;
using BookClub.Models;
using BookClub.Views;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookClub.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        BookClubContext context = new BookClubContext();

        private List<Product> products;
        public List<Product> Products
        {
            get { return products; }
            set { 
                products = value;
                SignalChanged();
            }
        }
        private ObservableCollection<ProductOrder> productOrders = new ObservableCollection<ProductOrder>();
        public ObservableCollection<ProductOrder> ProductOrders
        {
            get { return productOrders; }
            set
            {
                productOrders = value;
                SignalChanged();
            }
        }
        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set {
                selectedProduct = value;
                SignalChanged(); 
            }
        }

        private Visibility buttonVisibility = Visibility.Hidden;
        public Visibility ButtonVisibility
        {
            get { return buttonVisibility; }
            set
            {
                buttonVisibility = value;
                SignalChanged();
            }
        }

        public CustomCommand AddToOrder { get; set; }
        public CustomCommand ViewOrder { get; set; }

        public MainViewModel()
        {
            Products = context.Products.Include(s=>s.Manufacturer).ToList();

            ProductOrders.CollectionChanged += ProductOrders_CollectionChanged;

            AddToOrder = new CustomCommand(() =>
            {
                if (ProductOrders.Select(s=>s.Product).Any(s=>s.Id == SelectedProduct.Id))
                {
                    return;
                }
                if (SelectedProduct != null)
                {
                    ProductOrders.Add(new ProductOrder { ProductId = selectedProduct.Id, Product = selectedProduct, Quantity = 1 });
                }
            });

            ViewOrder = new CustomCommand(() =>
            {
                OrderViewWindow orderViewWindow = new OrderViewWindow(ProductOrders);
                orderViewWindow.ShowDialog();
            });
        }

        private void ProductOrders_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (ProductOrders.Count == 0)
            {
                ButtonVisibility = Visibility.Hidden;
            }
            else
            {
                ButtonVisibility = Visibility.Visible;
            }
        }
    }
}

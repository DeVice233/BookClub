using BookClub.Core;
using BookClub.Models;
using BookClub.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookClub.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        BookClubContext context = new BookClubContext();

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

        private decimal costSum;
        public decimal CostSum
        {
            get { return costSum; }
            set
            {
                costSum = value;
                SignalChanged();
            }
        }
        private decimal? discountSum = 0;
        public decimal? DiscountSum
        {
            get { return discountSum; }
            set
            {
                discountSum = value;
                SignalChanged();
            }
        }

        private ProductOrder selectedProductOrder;
        public ProductOrder SelectedProductOrder
        {
            get { return selectedProductOrder; }
            set
            {
                selectedProductOrder = value;
                SignalChanged();
            }
        }

        private List<TakePoint> takePoints;
        public List<TakePoint> TakePoints
        {
            get { return takePoints; }
            set
            {
                takePoints = value;
                SignalChanged();
            }
        }

        private TakePoint selectedTakePoint;
        public TakePoint SelectedTakePoint
        {
            get { return selectedTakePoint; }
            set
            {
                selectedTakePoint = value;
                SignalChanged();
            }
        }
        public CustomCommand Remove { get; set; }
        public CustomCommand GenerateOrder { get; set; }

        public OrderViewModel(ObservableCollection<ProductOrder> productOrders)
        {
            ProductOrders = productOrders;
            TakePoints = context.TakePoints.ToList();
            SelectedTakePoint = TakePoints.First();
            ProductOrders.CollectionChanged += ProductOrders_CollectionChanged;
            Calculate();

            Remove = new CustomCommand(() =>
            {
                if (SelectedProductOrder != null)
                {
                    RemoveItem(SelectedProductOrder);
                }
            });
            GenerateOrder = new CustomCommand(() =>
            {
                CreateOrder();
                ProductOrders.Clear();
                
                MessageBox.Show("Заказ создан успешно");
            });
        }
        private void CreateOrder()
        {
            Order order = new Order();
            order.Number = context.Orders.Count() + 1;
            order.DateOfOrder = DateTime.Now;
            Random random = new Random();
            order.Code = Convert.ToInt32(random.Next(1, 999).ToString("D3"));
            foreach (var productOrder in ProductOrders)
            {
                productOrder.Product = null;
                productOrder.OrderId = order.Number;
            }
            order.StatusId = context.Statuses.First(s => s.Name == "Новый").Id;
            order.CostSum = CostSum;
            order.DiscountSum = DiscountSum ?? 0;
            order.TakePointId = SelectedTakePoint.Id;
            order.ProductOrders = ProductOrders;
            try
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            TalonWindow talonWindow = new TalonWindow(order);
            talonWindow.ShowDialog();
        }
        private void ProductOrders_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Calculate();
        }
        public void RemoveItem(ProductOrder productOrder)
        {
            ProductOrders.Remove(productOrder);
        } 
        private void Calculate()
        {
            CostSum = ProductOrders.Sum(s => s.Product.Cost);
            DiscountSum = ProductOrders.Sum(s => s.Product.Discount);

        }
    }
}

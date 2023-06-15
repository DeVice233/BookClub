using BookClub.Core;
using BookClub.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookClub.ViewModels
{
    public class TalonViewModel : BaseViewModel
    {
        private Order order;
        public Order Order
        {
            get { return order; }
            set { 
                order = value;
                SignalChanged();
            }
        }

        private List<Product> products = new List<Product>();
        public List<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
                SignalChanged();
            }
        }
        public string TakePointName { get; set; }

        BookClubContext context = new BookClubContext();

        public CustomCommand SaveToPdf { get; set; }

        public TalonViewModel(Order order)
        {
            //
            Order = order;
            TakePointName = context.TakePoints.First(s => s.Id == Order.TakePointId).Name;
            foreach (var productOrder in Order.ProductOrders)
            {
                Products.Add(context.Products.First(s => s.Id == productOrder.ProductId));
            }
            SaveToPdf = new CustomCommand(() =>
            {
               Document document = new Document();
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Environment.CurrentDirectory + "\\Text3.pdf", FileMode.Create));
                BaseFont baseFont = BaseFont.CreateFont("C:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, 12, Font.NORMAL);
                Font boldFont = new Font(baseFont, 30, Font.BOLD);
                document.Open();

                document.Add(new Paragraph($"Заказ {Order.Number}",font));
                document.Add(new Paragraph($"Дата {Order.DateOfOrder}", font));
                document.Add(new Paragraph($"Состав", font));
                foreach (var product in Products)
                {
                    document.Add(new Paragraph($"- {product.Name}", font));
                }
                document.Add(new Paragraph($"Сумма {Order.CostSum}", font));
                document.Add(new Paragraph($"Скидка {Order.DiscountSum}", font));
                document.Add(new Paragraph($"Пункт выдачи {TakePointName}", font));
                document.Add(new Paragraph($"Код {Order.Code}", boldFont));

                document.Close();
            });
        }
    }
}

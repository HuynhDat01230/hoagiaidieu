using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopBanHoa.Models
{
    public class ProductItem
    {
        int productId;
        string title;        // Tiêu đề
        int quantity;        // số lượng
        double price;        // Giá bán
        string coverPage;    // Hình bìa        
        double total;        // Thành tiền
        // Propeties
        public int ProductId { get => productId; set => productId = value; }
        public string Title { get => title; set => title = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public double Price { get => price; set => price = value; }
        public string CoverPage { get => coverPage; set => coverPage = value; }
        // Tổng = đơn giá * số lượng
        public double Total { get => price * Quantity; }
        // Constructor
        public ProductItem() { }
        public ProductItem(int id, string title, int quality, double price, string img)
        {
            this.productId = id;
            this.title = title;
            this.quantity = quality;
            this.price = price;
            this.coverPage = img;
            this.total = quality * price;
        }
    }
}
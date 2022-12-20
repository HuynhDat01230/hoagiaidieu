using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopBanHoa.Models;

namespace ShopBanHoa.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        List<ProductItem> cart = new List<ProductItem>();
        QLBHEntities db = new QLBHEntities();
        // GET: Cart;
        public ActionResult Index()
        {
            List<ProductItem> cart = Session["cart"] as List<ProductItem>;
            return View(cart);
        }

        public RedirectToRouteResult AddToCart(int id)
        {
            // Nếu giỏ hàng chưa được khởi tạo
            if (Session["cart"] == null)
            {
                // Khởi tạo Session["giohang"] là 1 List<BookItem>
                Session["cart"] = new List<ProductItem>();
            }
            // Gán qua biến giohang cho dễ code
            cart = Session["cart"] as List<ProductItem>;
            // Kiểm tra xem sách khách đang chọn đã có trong giỏ hàng chưa ?
            // Nếu chưa có trong giỏ hàng
            if (cart.FirstOrDefault(m => m.ProductId == id) == null)
            {
                // Tìm sách theo id
                Product product = db.Products.Find(id);
                // Tạo ra 1 sách chọn (BookItem) mới
                ProductItem newItem = new ProductItem(id, product.PName, 1, (double)product.PPrice, product.Img);
                // Thêm BookItem vào giỏ
                cart.Add(newItem);
            }
            // Nếu sách đã có trong giỏ hàng
            else
            {
                // Không thêm vào giỏ nữa mà tăng số lượng lên.
                ProductItem productItem = cart.FirstOrDefault(m => m.ProductId == id);
                productItem.Quantity++;
            }
            // Action này sẽ chuyển hướng về trang danh mục để khách chọn tiếp
            // có thể thay bằng lệnh return Redirect(Request.UrlReferrer.ToString());
            return RedirectToAction("Index", "Products");
        }

        public RedirectToRouteResult UpdateQuantity(int? ProductId, int newQuantity)
        {
            // gán Session cho biến giohang cho dễ code
            List<ProductItem> cart = Session["cart"] as List<ProductItem>;
            //tìm Bookitem muốn sửa và gọi là itemUpdate
            ProductItem itemUpdate = cart.FirstOrDefault(m => m.ProductId == ProductId);
            // Nếu itemUpdate không null
            if (itemUpdate != null)
            {
                itemUpdate.Quantity = newQuantity; //gán số lượng mới
            }
            // Quay về trang danh mục chọn sản phẩm
            return RedirectToAction("Index", "Cart");
        }

        public RedirectToRouteResult RemoveCart(int id)
        {
            List<ProductItem> cart = Session["cart"] as List<ProductItem>;
            // Tìm sách có BookId = id và gọi là itemDelete
            ProductItem itemDelete = cart.FirstOrDefault(m => m.ProductId == id);
            if (itemDelete != null)
            {
                cart.Remove(itemDelete);
            }
            // Quay về trang danh mục chọn sách
            return RedirectToAction("Index", "Cart");
        }

        // GET: Carts/Create: Thanh toán
        public ActionResult ToPay(double sumtotal = 0)
        {	// Nhận giohang từ View truyền sang
            List<ProductItem> giohang = Session["cart"] as List<ProductItem>;
            Cart myCart = new Cart();
            myCart.Total = sumtotal; // để thể hiện default value
            Session["sumtotal"] = sumtotal; // lưu vào Session để truyền sang view

            return View();// gọi View với Models.Cart
        }

        [HttpPost]
        public ActionResult ToPays(int CartId = 0, string Datebuy = "", string CustomName = "", double Total = 0)
        {
            // Nhận giohang từ View truyền sang
            List<ProductItem> giohang = Session["cart"] as List<ProductItem>;
            Cart cart = new Cart();
            var listCart = db.Carts;
            int tmp = 0;
            for (int i = 1; i <= listCart.ToList().Count; i++)
            {
                Cart user = db.Carts.Find(i);
                if (user == null) tmp = i;
            }
            cart.CartID = tmp;
            Session["cartID"] = cart.CartID.ToString();
            cart.Datebuy = DateTime.Parse(Datebuy);
            cart.CustomName = CustomName; cart.Total = Total;


            if (ModelState.IsValid)
            {   // Ghi vào Cart
                db.Carts.Add(cart);
                db.SaveChanges();
                Response.Write("<script>alert('Item Already Exists')</script>");

                // Ghi chi tiết vào CartDetail                
                if (giohang.Count > 0)
                {
                    foreach (var item in giohang)
                    {
                        CartDetail cdetail = new CartDetail();
                        var listDetailCart = db.CartDetails;
                        int tmpDetail = 0;
                        for (int i = 1; i <= listDetailCart.ToList().Count; i++)
                        {
                            CartDetail user = db.CartDetails.Find(i);
                            if (user == null) tmpDetail = i;
                        }
                        cdetail.CartDetailID = tmpDetail;
                        cdetail.CartID = CartId;
                        cdetail.ProductID = item.ProductId;
                        cdetail.Quantity = item.Quantity; cdetail.Price = item.Price;
                        db.CartDetails.Add(cdetail);
                        db.SaveChanges();
                    }
                    // Xóa giỏ hàng
                    giohang.Clear();
                }
            }
            return RedirectToAction("Index", "Products"); // về lại page ban đầu ListBook
        }

        

    }
}
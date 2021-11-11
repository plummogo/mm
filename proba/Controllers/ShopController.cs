using proba.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace proba.Controllers
{
    public class ShopController : Controller
    {
        MMEntities shopEntity = new MMEntities();


        public ActionResult BrakeDrum(string sort,string search,string filter, int? page)
        {
            if (Session["LoggedId"] != null)
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "cname" : "";
                ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "ctype" : "";
                ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "produc" : "";
                if(search != null)
                    page = 1;
                else
                    search = filter;
                ViewBag.CurrentFilter = search;

                var drum = from s in shopEntity.CarsProducers.Where(bd => bd.Producer.Category.categoryname.Equals("Fékdob")).ToList()
                               select s;
                if (!String.IsNullOrEmpty(search))
                    drum = drum.Where(s => s.CarType.CarName.carname1.Contains(search.ToUpper()) || s.CarType.cartype1.Contains(search));
                switch (sort)
                {
                    case "cname":
                        drum = drum.OrderBy(s => s.CarType.CarName.carname1);
                        break;
                    case "ctype":
                        drum = drum.OrderBy(s => s.CarType.cartype1);
                        break;
                    case "produc":
                        drum = drum.OrderBy(s => s.Producer.producer1);
                        break;
                    default:
                        drum = drum.OrderBy(s => s.CarType.CarName.carname1);
                        break;
                }
                int pSize = 15;
                int pNumber = (page ?? 1);
                return View(drum.ToPagedList(pNumber,pSize));
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult BrakeLining(string sort, string search,string filter,int? page)
        {
            if (Session["LoggedId"] != null)
            {
                ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "cname" : "";
                ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "ctype" : "";
                ViewBag.NameSortParm = String.IsNullOrEmpty(sort) ? "produc" : "";
                if (search != null)
                    page = 1;
                else
                    search = filter;
                ViewBag.CurrentFilter = search;

                var lining = from s in shopEntity.CarsProducers.Where(bd => bd.Producer.Category.categoryname.Equals("Fékbetét")).ToList()
                           select s;
                if (!String.IsNullOrEmpty(search))
                    lining = lining.Where(s => s.CarType.CarName.carname1.Contains(search.ToUpper()) || s.CarType.cartype1.Contains(search));
                switch (sort)
                {
                    case "cname":
                        lining = lining.OrderBy(s => s.CarType.CarName.carname1);
                        break;
                    case "ctype":
                        lining = lining.OrderBy(s => s.CarType.cartype1);
                        break;
                    case "produc":
                        lining = lining.OrderBy(s => s.Producer.producer1);
                        break;
                    default:
                        lining = lining.OrderBy(s => s.CarType.CarName.carname1);
                        break;
                }
                int pSize = 15;
                int pNumber = (page ?? 1);
                return View(lining.ToPagedList(pNumber, pSize));
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Product(int? id)
        {
            if (Session["LoggedId"] != null)
            {
                CarsProducers carProducer = shopEntity.CarsProducers.Find(id);
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);
                Product product = shopEntity.CarsProducers.Find(carProducer.id).Product.FirstOrDefault();

                if (product != null)
                    return View(product);
                return HttpNotFound();
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult ProductCart(int? id)
        {
            if (Session["LoggedId"] != null)
            {
                ProductCart productCart = new ProductCart();
                productCart.productid = shopEntity.Product.Find(id).id;
                shopEntity.ProductCart.Add(productCart);
                shopEntity.SaveChanges();
                if (id != null)
                {
                    return RedirectToAction("Cart", new { id = productCart.id });
                }
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Cart(int id)
        {
            if (Session["LoggedId"] != null)
            {
                DateTime tempDate = DateTime.Now;

                int selectedCustomerId = int.Parse(Session["LoggedId"].ToString());

                ProductCart product = shopEntity.ProductCart.Find(id);

                Cart cart = new Cart();

                cart.shoppingdate = tempDate;

                cart.items_list = product.Product.CarsProducers.Producer.producer1;
                int records = shopEntity.ProductCart.ToList().Where(customer => customer.id.Equals(selectedCustomerId)).Count();
                for (int i = 0; i < records; i++)
                {
                    cart.cartitems++;
                    cart.sum_price += product.Product.price;
                }
                cart.producCartId = id;
                cart.customerId = int.Parse(Session["LoggedId"].ToString());
                shopEntity.Cart.Add(cart);
                shopEntity.SaveChanges();
                return View(shopEntity.Cart.ToList().Where(customer => customer.customerId.Equals(selectedCustomerId)));
            }
            return RedirectToAction("Login", "Home");
        }
        public ActionResult RemoveFromCart(int? id)
        {
            if (Session["LoggedId"] != null)
            {
                if (id == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);
                Cart cart = shopEntity.Cart.Find(id);
                if (cart == null)
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);
                shopEntity.Cart.Remove(cart);
                shopEntity.SaveChanges();
                return RedirectToAction("CartContent", "Home");
            }
            return RedirectToAction("Login", "Home");
        }
        
        public async Task<ActionResult> Paying()
        {
            if (Session["LoggedId"] != null)
            {
                int id = int.Parse(Session["LoggedId"].ToString());
                Customer customer = shopEntity.Customer.Where(currentUser => currentUser.id.Equals(id)).FirstOrDefault();
                Cart customerCart = shopEntity.Cart.FirstOrDefault();

                if (customerCart.sum_price == 0)
                    RedirectToAction("Webshop", "Shop");

                if (customer.address == null || customer.dateofbirth == null || customer.email == null || customer.paid == null || customer.price == null)
                    return RedirectToAction("BeforePay", "Shop", new { id = Session["LoggedId"] });
                else
                {
                    var message = new MailMessage();

                    message.BodyEncoding = Encoding.UTF8;
                    message.SubjectEncoding = Encoding.UTF8;

                    message.To.Add(new MailAddress(customer.email));
                    
                    message.From = new MailAddress("szilvasi.peter93@gmail.com");
                    
                    message.Subject = string.Format("Mazsik Műhely Vásárlás infó",Encoding.UTF8);
                    
                    message.Body = "<p>Kedves "+customer.name+",</p> <p>Köszönjük hogy nálunk vásárolt, alábbiakban küldjük a rendelési visszaigazolást.<br>"+
                    "<p>Ez egy automatikus üzenet, kérem ne válaszoljon rá! Ha mégsem Ön rendelt nálunk kérem, írjon nekünk panaszlevelet.</p>"+"<p>Üdvözlettel,<br> Mazsik Műhely</p>";
                    message.BodyEncoding = Encoding.UTF8;
                    message.SubjectEncoding = Encoding.UTF8;
                    message.IsBodyHtml = true;
                    using (var smtp = new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = "mazsikmuhely@gmail.com", //ide saját email pw
                            Password = "Diszpinty2016"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                    RemoveFromCart(customerCart.id);
                    return View();
                }
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult BeforePay(int? id)
        {
            if (Session["LoggedId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                }
                Customer customer = shopEntity.Customer.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }
                return View(customer);
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BeforePay(Customer updatedCustomer)
        {
            Customer loggedCustomer = shopEntity.Customer.Find(updatedCustomer.id);
            int count = loggedCustomer.Cart.ToList().Count; decimal temp = 0;
            if (Session["LoggedId"] != null)
            {
                if (ModelState.IsValid)
                {
                    loggedCustomer.address = updatedCustomer.address;
                    loggedCustomer.dateofbirth = updatedCustomer.dateofbirth;
                    loggedCustomer.paid = true;
                    for (int i = 0; i < count; i++)
                        temp += shopEntity.Cart.ToList()[i].sum_price;
                    loggedCustomer.price = temp;
                    shopEntity.SaveChanges();
                    return RedirectToAction("Paying", "Shop");
                }
                return RedirectToAction("BeforePay", "Shop");
            }
            return RedirectToAction("Login", "Home");
        }
    }
}

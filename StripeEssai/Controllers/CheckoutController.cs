using Microsoft.AspNetCore.Mvc;
using StripeEssai.Models;
using Stripe.Checkout;

namespace StripeEssai.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product = "Transparent Laptop",
                    Rate= 1200,
                    Quantity = 2,
                    ImagePath= "img/lenovo.jpeg"
                },
                new ProductEntity
                {
                    Product = "ps5",
                    Rate= 499,
                    Quantity = 2,
                    ImagePath= "img/ps5.jpeg"
                }


            };
            return View(productList);
        }
        /*public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus =="Paid")
            {
                var transaction = session.PaymentIntentId.ToString();
                return View("Success");
            }
            return View("Login");
        }
        public IActionResult Success() 
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }*/
        public IActionResult Checkout()
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>
            {
                new ProductEntity
                {
                    Product = "Transparent Laptop",
                    Rate= 1200,
                    Quantity = 2,
                    ImagePath= "img/lenovo.jpeg"
                },
                new ProductEntity
                {
                    Product = "ps5",
                    Rate= 499,
                    Quantity = 2,
                    ImagePath= "img/ps5.jpeg"
                }
            };
            var domain = "http://localhost:7164/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"Checkout/OrderConfirmation",
                CancelUrl = domain + $"Checkout/Login",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment"
            };
            foreach(var item in productList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Rate * item.Quantity),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ToString(),
                        }
                    },
                    Quantity = item.Quantity
                };
                options.LineItems.Add(sessionListItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            TempData["Session"] = session.Id;
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
}

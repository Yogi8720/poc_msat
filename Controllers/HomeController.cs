using Microsoft.AspNetCore.Mvc;
using poc_msat.Models;
using System.Diagnostics;

namespace poc_msat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CosmosDBContext _DBContext;

        public HomeController(ILogger<HomeController> logger, CosmosDBContext dBContext)
        {
            _logger = logger;
            _DBContext = dBContext;
        }

        public IActionResult Index1()
        {
            //var lastIndex = _DBContext.Orders.ToList().Max<Order>(x=> Convert.ToInt32(x.id));
            //for (int i = lastIndex; i < lastIndex+5; i++)
            //{
            //    Order order = new Order
            //    {
            //        id = (i+1).ToString(),
            //        PartitionKey = (i + 1).ToString(),
            //        Date = DateTime.Now,
            //        Customer = new Customer
            //        {
            //            Mobile = "9718126965",
            //            Name = "Deepak Kumar Maurya",
            //            Address = new Address
            //            {
            //                AddressLine1 = "918",
            //                City = "New Delhi",
            //                Pincode = "110023",
            //                State = "Delhi"
            //            }
            //        },
            //        OrderItems = new List<OrderItem>
            //    {
            //        new OrderItem
            //        {
            //            Name="Item 1",
            //            OrderItemID="1",
            //            MRP="100",
            //            Price="80"
            //        },
            //        new OrderItem
            //        {
            //            Name="Item 2",
            //            OrderItemID="2",
            //            MRP="90",
            //            Price="87"
            //        }
            //    }
            //    };

            //    //var order = _DBContext.Orders.FirstOrDefault(x=> x.PartitionKey == "1");

            //    _DBContext.Orders.Add(order);
            //}

            //_DBContext.SaveChanges();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Index()
        {
            GetOrderViewModel model = new GetOrderViewModel();
            model.Orders = _DBContext.Orders.ToList();

            model.SelectListItems = Constant.SelectListItems;

            return View("getorders", model);
        }

        [HttpPost]
        public IActionResult Index(GetOrderViewModel model)
        {
            if (model == null)
                model = new GetOrderViewModel();
            else
            {
                var orders = _DBContext.Orders.AsEnumerable()
                    .Where(x => x.OrderItems.Any(o=> model.SelectedItems.Any(s=> s == o.OrderItemID))).ToList();
                model.Orders = orders;
            }
            model.SelectListItems = Constant.SelectListItems;

            return View("getorders", model);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var order = _DBContext.Orders.FirstOrDefault(x => x.id == id);

            if (order == null)
                ModelState.AddModelError("error", "Invalid id");

            return View(order);
        }

        [HttpGet]
        public IActionResult Create()
        {
            OrderViewModel order = new OrderViewModel();
            order.SelectListItems = Constant.SelectListItems;

            return View(order);
        }

        [HttpPost]
        public IActionResult Create(OrderViewModel model)
        {
            Order order = GetOrderFromModel(model);

            _DBContext.Orders.Add(order);
            _DBContext.SaveChanges();

            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var order = _DBContext.Orders.FirstOrDefault(x => x.id == id);

            if (order == null)
                ModelState.AddModelError("error", "Invalid id");

            OrderViewModel orderViewModel = new OrderViewModel()
            {
                Order = order,
                SelectListItems = Constant.SelectListItems,
                SelectedItems = order.OrderItems?.Select(x => x.OrderItemID).ToList()
            };
            return View(orderViewModel);
        }

        [HttpPost]
        public IActionResult Edit(string id, OrderViewModel model)
        {
            if (model == null)
                return View(model);

            var oldorder = _DBContext.Orders.FirstOrDefault(x => x.id == id);

            if (oldorder != null)
            {
                oldorder.Customer.Name = model.Order.Customer.Name;
                oldorder.Customer.Mobile = model.Order.Customer.Mobile;
                oldorder.OrderItems = GetOrderItemsFromModel(model);

                _DBContext.Orders.Update(oldorder);
                _DBContext.SaveChanges();
            }
            else
                return View(model);

            return RedirectToAction("index");
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var order = _DBContext.Orders.FirstOrDefault(x => x.id == id);

            if (order == null)
                ModelState.AddModelError("error", "Invalid id");

            return View(order);
        }

        [HttpPost]
        public IActionResult Delete(Order order)
        {
            var oldorder = _DBContext.Orders.FirstOrDefault(x => x.id == order.id);
            if (oldorder != null)
            {
                _DBContext.Orders.Remove(oldorder);
                _DBContext.SaveChanges();
            }

            return RedirectToAction("index");
            //return View();
        }

        protected Order GetOrderFromModel(OrderViewModel model)
        {
            Order order = new Order();
            var orders = _DBContext.Orders.ToList();
            int lastIndex = 0;
            if (orders.Count > 0)
                lastIndex = orders.Max<Order>(x => Convert.ToInt32(x.id));

            order.id = (lastIndex + 1).ToString();
            order.PartitionKey = (lastIndex + 1).ToString();
            order.Date = DateTime.Now;
            order.Customer = model.Order.Customer;
            order.OrderItems = GetOrderItemsFromModel(model);

            return order;
        }

        protected List<OrderItem> GetOrderItemsFromModel(OrderViewModel model)
        {
            var selectedOrderItems = Constant.OrderItems.Where(x => model.SelectedItems.Contains(x.OrderItemID));

            //var items = selectedOrderItems.Select(x => new OrderItem
            //{
            //    Name = x.Text,
            //    OrderItemID = x.Value,
            //    IsSelected = true,
            //    MRP="0",
            //    Price="0",
            //}).ToList();

            return selectedOrderItems.ToList();
        }

        protected OrderViewModel GetModelFromOrder(Order order)
        {
            OrderViewModel model = new OrderViewModel() { Order = order };

            return model;
        }
    }
}
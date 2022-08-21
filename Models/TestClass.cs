using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace poc_msat.Models
{

    public class Constant
    {
        public static List<OrderItem> OrderItems = new List<OrderItem>
        {
            new OrderItem
            {
                Name = "Order item 1",
                MRP = "100",
                OrderItemID = "1",
                Price = "90",
                IsSelected = false,
            },
            new OrderItem
            {
                Name = "Order item 2",
                MRP = "90",
                OrderItemID = "2",
                Price = "85",
                IsSelected = false,
            },
            new OrderItem
            {
                Name = "Order item 3",
                MRP = "40",
                OrderItemID = "3",
                Price = "38",
                IsSelected = false,
            },
            new OrderItem
            {
                Name = "Order item 4",
                MRP = "55",
                OrderItemID = "4",
                Price = "52",
                IsSelected = false,
            },
            new OrderItem
            {
                Name = "Order item 5",
                MRP = "87",
                OrderItemID = "5",
                Price = "84",
                IsSelected = false,
            },
            new OrderItem
            {
                Name = "Order item 6",
                MRP = "56",
                OrderItemID = "6",
                Price = "50",
                IsSelected = false,
            },
        };

        public static List<SelectListItem> SelectListItems = OrderItems.Select(x => new SelectListItem
        {
            Value = x.OrderItemID,
            Text = x.Name,
            Selected = x.IsSelected,
        }).ToList();
    }

    public class GetOrderViewModel
    {
        public List<Order> Orders { get; set; }
        public List<SelectListItem> SelectListItems { get; set; }
        public List<string> SelectedItems { get; set; }
    }

    public class OrderViewModel
    {
        public Order Order { get; set; }
        public List<SelectListItem> SelectListItems { get; set; }
        public List<string> SelectedItems { get; set; }
    }

    public class Order
    {
        public string id { get; set; }
        public string PartitionKey { get; set; }
        public DateTime Date { get; set; }
        public Customer Customer { get; set; }
        //public List<SelectListItem> OrderItems { get; set; } = Constant.SelectListItems;
        [DisplayName("Order Items")]
        public List<OrderItem> OrderItems { get; set; }
        //public List<string> SelectedOrderItems { get; set; }
    }

    public class Customer
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public Address Address { get; set; }
    }

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
    }

    public class OrderItem
    {
        public string OrderItemID { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string MRP { get; set; }
        public bool IsSelected { get; set; }
    }
}

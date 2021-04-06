using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Inclass10
{

    public class Products
    {
        public int Id { get; set; }
        public String ProductName { get; set; }
        public int InstockQty { get; set; }
        public double UnitPrice { get; set; }
        public IList<OrderDetails>PlacedProducts { get; set; }
    }

    public class Orders
    {
        public int Id { get; set; }
        public String CustId { get; set; }
        public DateTime orderdate { get; set; }
        public int TotalQty{ get; set; }
        public ICollection<OrderDetails> OrderedProducts { get; set; }
    }
    public class OrderDetails
    {
        public int ID { get; set; }
        public Orders order { get; set; }
        public Products product { get; set; }
        public int TotalQty { get; set; }
    }
    class OrderDetailContext : DbContext
    {
        public DbSet<Products> products { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<OrderDetails> orderDetails { get; set; }

        string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=InClassDB;Trusted_Connection=True;MultipleActiveResultSets=true";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            using (OrderDetailContext context = new OrderDetailContext())
            {
                context.Database.EnsureCreated();
                Products[] productlist = new Products[]
               {
                    new Products{ProductName="Apples",InstockQty=100,UnitPrice=100.00},
                    new Products{ProductName="Sugar",InstockQty=100,UnitPrice=10.00},
                    new Products{ProductName="Yogurt",InstockQty=100,UnitPrice=12.00},
                    new Products{ProductName="Kiwi",InstockQty=100,UnitPrice=4.00},
                    new Products{ProductName="Coke",InstockQty=100,UnitPrice=3.00},
                    new Products{ProductName="passion fruit",InstockQty=100,UnitPrice=5.00 },
                    new Products{ProductName="Avocado",InstockQty=100,UnitPrice=5.00 },
                    new Products{ProductName="Orange",InstockQty=100,UnitPrice=5.00 }
               };


                

                Orders[] orderlist = new Orders[]
                    {
                    new Orders{CustId="001",orderdate=DateTime.Parse("2021-03-06"),TotalQty=9},
                    new Orders{CustId="002",orderdate=DateTime.Parse("2021-02-24"),TotalQty=8},
                    new Orders{CustId="003",orderdate=DateTime.Parse("2021-03-13"),TotalQty=7},
                    new Orders{CustId="004",orderdate=DateTime.Parse("2021-04-04"),TotalQty=6},
                    new Orders{CustId="005",orderdate=DateTime.Parse("2021-03-23"),TotalQty=5},
                    new Orders{CustId="005",orderdate=DateTime.Parse("2021-03-23"),TotalQty=4},
                    new Orders{CustId="005",orderdate=DateTime.Parse("2021-03-23"),TotalQty=3},
                    new Orders{CustId="005",orderdate=DateTime.Parse("2021-03-23"),TotalQty=4}
                };

                OrderDetails[] detailslist = new OrderDetails[]
               {
                    new OrderDetails{order=orderlist[0],product=productlist[0],TotalQty=9},
                    new OrderDetails{order=orderlist[0],product=productlist[2],TotalQty=8},
                    new OrderDetails{order=orderlist[0],product=productlist[3],TotalQty=7},
                    new OrderDetails{order=orderlist[1],product=productlist[0],TotalQty=2},
                    new OrderDetails{order=orderlist[1],product=productlist[4],TotalQty=1},
                    new OrderDetails{order=orderlist[2],product=productlist[1],TotalQty=4},
                    new OrderDetails{order=orderlist[2],product=productlist[3],TotalQty=1},
                    new OrderDetails{order=orderlist[2],product=productlist[4],TotalQty=5},
                    new OrderDetails{order=orderlist[3],product=productlist[0],TotalQty=1},
                    new OrderDetails{order=orderlist[3],product=productlist[1],TotalQty=4},
                    new OrderDetails{order=orderlist[3],product=productlist[2],TotalQty=3},
                    new OrderDetails{order=orderlist[3],product=productlist[3],TotalQty=4},
                    new OrderDetails{order=orderlist[3],product=productlist[4],TotalQty=5},
               };
                if (!context.orders.Any())
                {
                    foreach (Orders o in orderlist)
                    {
                        context.orders.Add(o);
                    }

                    context.SaveChanges();
                }

                if (!context.products.Any())
                {
                    foreach (Products p in productlist)
                    {
                        context.products.Add(p);
                    }
                    context.SaveChanges();
                }
                


                if (!context.orderDetails.Any())
                {
                    foreach (OrderDetails d in detailslist)
                    {
                        context.orderDetails.Add(d);
                    }
                    context.SaveChanges();
                }

                
                // Display all orders where a product is sold
                var a = context.orders
                    .Include(c => c.OrderedProducts)
                    .Where(c => c.OrderedProducts.Count != 0);
                Console.WriteLine("--------------Order List of sold products-----------------");
                foreach (var i in a)
                {
                    Console.WriteLine("OrderID={0},OrderDate={1}", i.Id, i.orderdate);
                }

                // For a given product, find the order where it is sold the maximum.
                Orders output = context.orderDetails
                    .Where(c => c.product.ProductName == "Kiwi")
                    .OrderByDescending(c => c.TotalQty)
                    .Select(c => c.order)
                    .First();
                Console.WriteLine("----------------------Order where maximum amount of Kiwi has been sold---------------------------");
                Console.WriteLine("OrderID={0},CustId={1},OrderDate={2}", output.Id,output.CustId,output.orderdate);
            }
        }
    }
}

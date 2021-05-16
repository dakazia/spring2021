using System;
using System.Collections.Generic;
using Task1.DoNotChange;
using System.Linq;

namespace Task1
{
    public static class LinqTask
    {
        public static IEnumerable<Customer> Linq1(IEnumerable<Customer> customers, decimal limit)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers.Where(c => c.Orders.Sum(o => o.Total) > limit);
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }
            if (suppliers is null)
            {
                throw new ArgumentNullException(nameof(suppliers));
            }

            return customers.Select(c => (c, suppliers.Where(s => c.Country == s.Country && c.City == s.City)));
        }

        public static IEnumerable<(Customer customer, IEnumerable<Supplier> suppliers)> Linq2UsingGroup(
            IEnumerable<Customer> customers,
            IEnumerable<Supplier> suppliers
        )
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }
            if (suppliers is null)
            {
                throw new ArgumentNullException(nameof(suppliers));
            }

            return customers.GroupJoin(suppliers, c => c.City, s => s.City, (c, s) => (c, s));
        }

        public static IEnumerable<Customer> Linq3(IEnumerable<Customer> customers, decimal limit)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers.Where(c => c.Orders.Any(o => o.Total > limit));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq4(
            IEnumerable<Customer> customers
        )
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers.Where(c => c.Orders.Any())
                            .Select(c => (customer: c, dateOfEntry: c.Orders.Min(o => o.OrderDate)));
        }

        public static IEnumerable<(Customer customer, DateTime dateOfEntry)> Linq5(
            IEnumerable<Customer> customers
        )
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers.Where(c => c.Orders.Any())
                .Select(c => (customer: c, dateOfEntry: c.Orders.Min(o => o.OrderDate)))
                .OrderBy(query => query.dateOfEntry.Year)
                .ThenBy(query => query.dateOfEntry.Month)
                .ThenByDescending(query => query.customer.Orders.Sum(o => o.Total))
                .ThenBy(query => query.customer.CompanyName);

        }

        public static IEnumerable<Customer> Linq6(IEnumerable<Customer> customers)
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers.Where(c => c.PostalCode.Any(p => !char.IsDigit(p))
                                        || string.IsNullOrEmpty(c.Region)
                                        || !c.Phone.StartsWith('('));
        }

        public static IEnumerable<Linq7CategoryGroup> Linq7(IEnumerable<Product> products)
        {
            /* example of Linq7result

             category - Beverages
	            UnitsInStock - 39
		            price - 18.0000
		            price - 19.0000
	            UnitsInStock - 17
		            price - 18.0000
		            price - 19.0000
             */
            if (products is null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            var result = products.GroupBy(prod => prod.Category, (cat, prods) =>
                new Linq7CategoryGroup()
                {
                    Category = cat,
                    UnitsInStockGroup = prods.GroupBy(prod => prod.UnitsInStock, (stock, p) =>
                        new Linq7UnitsInStockGroup()
                        {
                            UnitsInStock = stock,
                            Prices = p.Select(prod => prod.UnitPrice).OrderBy(price => price)
                        })
                });

            return result;
        }

        public static IEnumerable<(decimal category, IEnumerable<Product> products)> Linq8(
            IEnumerable<Product> products,
            decimal cheap,
            decimal middle,
            decimal expensive
        )
        {
            if (products is null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            var ranges = new[] { cheap, middle, expensive };
            var result = products.GroupBy(prod => ranges.FirstOrDefault(range => range >= prod.UnitPrice),
                (cat, prods) => (
                    category: cat,
                    products: prods
                ));

            return result;
        }

        public static IEnumerable<(string city, int averageIncome, int averageIntensity)> Linq9(
            IEnumerable<Customer> customers
        )
        {
            if (customers is null)
            {
                throw new ArgumentNullException(nameof(customers));
            }

            return customers.GroupBy(cust => cust.City)
                .Select(g => (
                    g.Key, 
                    (int)Math.Round(g.Average(cust => cust.Orders.Sum(o => o.Total))), 
                    (int)Math.Round(g.Average(cust => cust.Orders.Length))
                    ));
        }

        public static string Linq10(IEnumerable<Supplier> suppliers)
        {
            if (suppliers is null)
            {
                throw new ArgumentNullException(nameof(suppliers));
            }

            return string.Join("",
                suppliers.Select(s => s.Country).Distinct().OrderBy(c => c.Length).ThenBy(c => c[0]));
        }
    }
}
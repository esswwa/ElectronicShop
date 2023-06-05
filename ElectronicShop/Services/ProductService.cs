using ElectronicShop.Data;
using ElectronicShop.Data.Model;
using ElectronicShop.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.Services
{
    public class ProductService
    {
        private readonly ElectronickshopContext _electronickshopContext;
        public ProductService(ElectronickshopContext electronickshopContext)
        {
            _electronickshopContext = electronickshopContext;
        }

        public async Task<List<Product>> GetProducts()
        {
            List<Product> products = new();
            try
            {
                _electronickshopContext.Statuses.ToList();
                _electronickshopContext.Firms.ToList();
                _electronickshopContext.Categories.ToList();
                var product = await _electronickshopContext.Products.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var item in product)
                    {
                        products.Add(new Product
                        {
                            IdProduct = item.IdProduct,
                            NameProduct = item.NameProduct,
                            ImgProduct = Path.GetFullPath($@"Resources\Image\{item.ImgProduct}"),
                            FirmProduct = item.FirmProduct,
                            CostProduct = item.CostProduct,
                            CategoryProduct = item.CategoryProduct,
                            ReitingProduct = item.ReitingProduct,
                            CountProduct = item.CountProduct,
                            Status = item.Status,
                            Description = item.Description,
                            SecondNameProduct = item.SecondNameProduct,
                            Article = item.Article,
                            CategoryProductNavigation = item.CategoryProductNavigation,
                            FirmProductNavigation = item.FirmProductNavigation, 
                            StatusNavigation = item.StatusNavigation

                        }); ;
                    }
                });

            }
            catch { }
            return products;
        }

        public async Task<List<Product>> GetFavouriteProducts()
        {
            List<Product> products = new();
            try
            {
                _electronickshopContext.Statuses.ToList();
                _electronickshopContext.Firms.ToList();
                _electronickshopContext.Categories.ToList();
                var product = await _electronickshopContext.Products.ToListAsync();
                var productFavoutite = await _electronickshopContext.Favourities.Where(i => i.IdUser == Settings.Default.idUser).ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var item in product)
                    {
                        foreach (var item1 in productFavoutite) {
                            if (item.IdProduct == item1.IdProduct)
                                products.Add(new Product
                                {
                                    IdProduct = item.IdProduct,
                                    NameProduct = item.NameProduct,
                                    ImgProduct = Path.GetFullPath($@"Resources\Image\{item.ImgProduct}"),
                                    FirmProduct = item.FirmProduct,
                                    CostProduct = item.CostProduct,
                                    CategoryProduct = item.CategoryProduct,
                                    ReitingProduct = item.ReitingProduct,
                                    CountProduct = item.CountProduct,
                                    Status = item.Status,
                                    Description = item.Description,
                                    SecondNameProduct = item.SecondNameProduct,
                                    Article = item.Article,
                                    CategoryProductNavigation = item.CategoryProductNavigation,
                                    FirmProductNavigation = item.FirmProductNavigation,
                                    StatusNavigation = item.StatusNavigation

                                }); ;
                        }
                       
                    }
                });

            }
            catch { }
            return products;
        }

        public async Task AddHelperBasket(int idHelper_basket, int idBasket, int idProduct, double cost)
        {
            await _electronickshopContext.HelperBaskets.AddAsync(new HelperBasket
            {
                IdhelperBasket = idHelper_basket,
                IdBasket = idBasket,
                IdProduct = idProduct,
                Count = 1,
                Cost = cost
            });
            await _electronickshopContext.SaveChangesAsync();
        }

        public async Task editHelperBasket(HelperBasket SelectedHelper, bool z)
        {
            ObservableCollection<HelperBasket> helperBasket = getAllHelperBasket();
            var item = helperBasket.First(i => i.IdhelperBasket == SelectedHelper.IdhelperBasket);
            var index = helperBasket.IndexOf(item);
            if (z == true) {
                item.Count = item.Count + 1;
                item.Cost = item.Cost + SelectedHelper.IdProductNavigation.CostProduct;
                helperBasket.RemoveAt(index);
                helperBasket.Insert(index, item);
            }
            else {
                if (item.Count > 1)
                {
                    item.Count = item.Count - 1;
                    item.Cost = item.Cost - SelectedHelper.IdProductNavigation.CostProduct;
                    helperBasket.RemoveAt(index);
                    helperBasket.Insert(index, item);
                    
                }
                else {
                    helperBasket.RemoveAt(index);
                }
                
            }
            await _electronickshopContext.SaveChangesAsync();
        }

        public ObservableCollection<HelperBasket> getAllHelperBasket()
        {
            return _electronickshopContext.HelperBaskets.ToObservableCollection<HelperBasket>();
        }

        public HelperBasket getUserHelperBasket(Product SelectedProduct)
        {
            return _electronickshopContext.HelperBaskets.Where(i => i.IdBasket == Settings.Default.idUser && i.IdProduct == SelectedProduct.IdProduct).First();
        }

        public bool getUserHelper(Product product) {

           List<HelperBasket> help = _electronickshopContext.HelperBaskets.Where(i => i.IdBasketNavigation.IdUser == Settings.Default.idUser).ToList();
            if (help.Where(i => i.IdProduct == product.IdProduct).FirstOrDefault() == null)
                return true;
            else
                return false;
        }


        public int GetMaxHelper()
        {

            return _electronickshopContext.HelperBaskets.Max(u => u.IdhelperBasket);
        }

        //public bool checkProduct() { 

        //}

        //public async Task<List<Product>> getBasket()
        //{

        //    List<Product> a = new();
        //    var b = await GetProducts();

        //    foreach (var item in Global.CurrentCart)
        //    {
        //        var product = b.SingleOrDefault(c => c.Article.Equals(item.ArticleName));
        //        if (product != null)
        //        {
        //            product.Count = Global.CurrentCart.Single(a => a.ArticleName.Equals(product.Article)).Count;
        //            a.Add(product);
        //        }
        //    }
        //    return a;
        //}
        //public async Task<List<Point>> GetPoints() => await _electronickshopContext.Pickuppoints.AsNoTracking().ToListAsync();
        //public async Task<int> AddOrder(Order order)
        //{
        //    await _electronickshopContext.Orderusers.AddAsync(order);
        //    await _electronickshopContext.SaveChangesAsync();

        //    foreach (var item in Global.CurrentCart)
        //    {
        //        await _electronickshopContext.Orderproducts.AddAsync(new Orderproduct
        //        {
        //            OrderId = order.OrderId,
        //            ProductArticleNumber = item.ArticleName,
        //            ProductCount = item.Count
        //        });
        //        await _electronickshopContext.SaveChangesAsync();
        //    }
        //    return order.OrderId;
        //}

        //public async Task<List<Product>> GetListFullInformation()
        //{
        //    var currentProduct = await GetProducts();
        //    List<Product> product = new();
        //    foreach (var item in currentProduct)
        //    {
        //        if (Global.CurrentCart.FirstOrDefault(c => c.ArticleName.Equals(item.Article)) != null)
        //            product.Add(item);
        //    }
        //    return product;
        //}
    }





}

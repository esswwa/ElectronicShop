using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using ElectronicShop.Data;
using ElectronicShop.Data.Model;
using ElectronicShop.Models;
using ElectronicShop.Properties;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            List<Product> products1 = new();
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
                        products1.Add(new Product
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

                        });
                    }
                });

            }
            catch { }
            return products1;
        }

        public async Task<List<Product>> GetFavouriteProducts()
        {
            List<Product> products = new();
            try
            {
                _electronickshopContext.Statuses.ToList();
                _electronickshopContext.Firms.ToList();
                _electronickshopContext.Categories.ToList();
                var product = _electronickshopContext.Products.ToList();
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

                                });
                        }

                    }
                });

            }
            catch { }
            return products;
        }

        public async void UpdateProductReiting()
        {
            //var currentOrders = await GetProducts1();
            //Users = new ObservableCollection<User>(currentOrders);
            //var item = Users.First(i => i.Iduser == Settings.Default.idUser);
            //var index = Users.IndexOf(item);
            //item.ExitCheck = Settings.Default.exitCheck;
            //Users.RemoveAt(index);
            //Users.Insert(index, item);
            //await _electronickshopContext.SaveChangesAsync();
        }

        public async Task<List<Feedback>> getFeedbackProduct()
        {
            List<Feedback> feedbacks = new List<Feedback>();
            try
            {
                _electronickshopContext.Users.ToList();
                _electronickshopContext.Feedbacks.ToList();
                _electronickshopContext.Categories.ToList();
                var feedbacks1 = await _electronickshopContext.Feedbacks.Where(i => i.IdProduct == SelectProduct.product.IdProduct).ToListAsync();
                feedbacks = feedbacks1;
                return feedbacks;

            }
            catch { return feedbacks; }
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

        public async Task addFeedback(string downside, string dignities, string comment, float value, int idProduct)
        {
            int z = _electronickshopContext.Feedbacks.Max(i => i.Idfeedback) + 1;
            var check = _electronickshopContext.Feedbacks.Select(i => i.IdProduct == idProduct && i.IdUser == Settings.Default.idUser).First();
            if (check == false)
            {
                await _electronickshopContext.Feedbacks.AddAsync(new Feedback
                {
                    Idfeedback = z,
                    IdProduct = idProduct,
                    IdUser = Settings.Default.idUser,
                    Feedbacks = comment,
                    Reiting = value,
                    Downside = downside,
                    Dignities = dignities

                });

            }
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

        public HelperBasket getUserHelperBasket(Product IdProduct)
        {
            return _electronickshopContext.HelperBaskets.Where(i => i.IdBasket == Settings.Default.idUser && i.IdProduct == IdProduct.IdProduct).First();
        }

        public async Task<List<HelperBasket>> getAllHelperBasketUser() {

            return await _electronickshopContext.HelperBaskets.Where(i => i.IdBasket == Settings.Default.idUser).ToListAsync();
        }

        public async Task<List<Product>> getUserHelperBasketList()
        {
            List<Product> products = new();
            try
            {
                var z = _electronickshopContext.HelperBaskets.Where(i => i.IdBasket == Settings.Default.idUser).ToList();
                _electronickshopContext.Statuses.ToList();
                _electronickshopContext.Firms.ToList();
                _electronickshopContext.Categories.ToList();
                var product = _electronickshopContext.Products.ToList();
                await Task.Run(() =>
                {
                    foreach (var item in product)
                    {
                        foreach (var item1 in z)
                        {
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

                                    });
                        }

                    }
                });

            }
            catch { }
            return products;
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

        public async Task deleteBasketProduct(HelperBasket SelectedProduct)
        {
            ObservableCollection<HelperBasket> Helpers = _electronickshopContext.HelperBaskets.ToObservableCollection();

            var item = Helpers.First(i => i.IdhelperBasket == SelectedProduct.IdhelperBasket);
            var index = Helpers.IndexOf(item);
            if (item.Count > 1)
            {
                double x = item.Cost;
                x = x / (double)item.Count;
                item.Count = item.Count - 1;
                item.Cost = item.Cost - x;
                Helpers.RemoveAt(index);
                Helpers.Insert(index, item);
            }
            else {
                System.Windows.MessageBox.Show("Remove");
                Helpers.RemoveAt(index);
            }
            await _electronickshopContext.SaveChangesAsync();
        }
    }
}

﻿using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using AutoMapper;
using ElectronicShop.Models;
using ElectronicShop.Data.Model;

namespace ElectronicShop.Services
{
    public class ProductService
    {
        private readonly IMapper _mapper;
        private readonly ElectronickshopContext _electronickshopContext;
        public ProductService(ElectronickshopContext electronickshopContext)
        {
            _electronickshopContext = electronickshopContext;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, DbProduct>();
            }).CreateMapper();

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
                        foreach (var item1 in productFavoutite)
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

        //public async Task<List<DbProduct>> GetProducts2()
        //{
        //    List<DbProduct> dbProduct = new();
        //    try
        //    {
        //        await _electronickshopContext.Statuses.ToListAsync();
        //        await _electronickshopContext.Firms.ToListAsync();
        //        await _electronickshopContext.Categories.ToListAsync();
        //        dbProduct = _mapper.Map<List<DbProduct>>(await _electronickshopContext.Products.ToListAsync());
        //    }
        //    catch { }
        //    return dbProduct;
        //}

        //public async Task<List<DbProduct>> GetFavouriteProducts()
        //{
        //    List<DbProduct> dbProducts = new();
        //    var currentProducts = await GetProducts2();
        //    await Task.Delay(200);
        //    var productFavoutite = await _electronickshopContext.Favourities.Where(i => i.IdUser == Settings.Default.idUser).AsNoTracking().ToListAsync();
        //    foreach (var product in currentProducts)
        //    {
        //        foreach (var item in productFavoutite)
        //        {
        //            var productBasketCheck = await getUserHelperBasket(product);
        //                if (item.IdProduct == product.IdProduct)
        //                {
        //                    product.IsChekedFavourite = true;
        //                    if (productBasketCheck != null)
        //                        product.IsChekedBasket = true;
        //                    else
        //                        product.IsChekedBasket = false;
        //                    dbProducts.Add(product);
        //                }
        //        }
        //    }

        //    return dbProducts;
        //}

        public async void UpdateProductReiting(int idProduct)
        {
            List<Feedback> cur = _electronickshopContext.Feedbacks.Where(i => i.IdProduct == idProduct).ToList();
            double z = 0;
            foreach (var i in cur)
            {
                z += (double)i.Reiting;
            }
            z = Math.Round(z / cur.Count, 2);
            ObservableCollection<Product> prod = _electronickshopContext.Products.ToObservableCollection();
            var item = prod.First(i => i.IdProduct == idProduct);
            var index = prod.IndexOf(item);
            item.ReitingProduct = (float)z;
            prod.RemoveAt(index);
            prod.Insert(index, item);
            await _electronickshopContext.SaveChangesAsync();
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
        public async Task AddFavourites(int idProduct)
        {
            var idHelp = _electronickshopContext.Favourities.Max(i => i.Idfavourities);
            await _electronickshopContext.Favourities.AddAsync(new Favourity
            {
                Idfavourities = idHelp + 1,
                IdUser = Settings.Default.idUser,
                IdProduct = idProduct
            });
            await _electronickshopContext.SaveChangesAsync();
        }

        public bool getAddFavourites(int idProduct)
        {
            var z = _electronickshopContext.Favourities.ToList();
            if (z.Where(i => i.IdProduct == idProduct && i.IdUser == Settings.Default.idUser).FirstOrDefault() == null)
                return true;
            else
                return false;
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

        public async Task deleteFavourite(Product SelectedFavourite)
        {
            ObservableCollection<Favourity> Favourite = _electronickshopContext.Favourities.ToObservableCollection();
            var item = Favourite.First(i => i.IdProduct == SelectedFavourite.IdProduct && Settings.Default.idUser == i.IdUser);
            if(item != null)
            {
                var index = Favourite.IndexOf(item);
                Favourite.RemoveAt(index);
                _electronickshopContext.Favourities.Remove(item);
                await _electronickshopContext.SaveChangesAsync();
            }
            
        }

        public async Task editUser()
        {
            ObservableCollection<User> Users = _electronickshopContext.Users.ToObservableCollection();
            var item = Users.First(i => i.Iduser == Settings.Default.idUser);
            var index = Users.IndexOf(item);
            item.Login = Settings.Default.login;
            item.Email = Settings.Default.email;
            item.Adress = Settings.Default.Adress;
            Users.RemoveAt(index);
            Users.Insert(index, item);
            await _electronickshopContext.SaveChangesAsync();
        }

        public async Task editOrderStatus(Order OrderHelper)
        {
            ObservableCollection<Order> OrderHelper1 = _electronickshopContext.Orders.ToObservableCollection();
            var item = OrderHelper1.First(i => i.IdStatusOrder == OrderHelper.IdStatusOrder);
            var index = OrderHelper1.IndexOf(item);
            item.IdStatusOrder = 4;
            OrderHelper1.RemoveAt(index);
            OrderHelper1.Insert(index, item);
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
                    _electronickshopContext.HelperBaskets.Remove(item);
                }

            }
            await _electronickshopContext.SaveChangesAsync();
        }

        public ObservableCollection<HelperBasket> getAllHelperBasket()
        {
            return _electronickshopContext.HelperBaskets.ToObservableCollection<HelperBasket>();
        }

        public async Task<HelperBasket> getUserHelperBasket(Product IdProduct)
        {
            return await _electronickshopContext.HelperBaskets.Where(i => i.IdBasket == Settings.Default.idUser && i.IdProduct == IdProduct.IdProduct).FirstOrDefaultAsync();
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


        public async Task<List<Order>> getOrderHelper()
        {
            List<Order> feedbacks1 = new List<Order>();
            try
            {
                _electronickshopContext.Users.ToList();
                _electronickshopContext.Products.ToList();
                _electronickshopContext.StatusOrders.ToList();
                var feedbacks = await _electronickshopContext.Orders.Where(i => i.IdStatusOrderNavigation.NameStatus != "Выдан покупателю" && i.IdStatusOrderNavigation.NameStatus != "Отменен").ToListAsync();
                feedbacks1 = feedbacks;
                return feedbacks1;

            }
            catch { return feedbacks1; }
        }

        public async Task<List<Order>> getOrderHelper1()
        {
            List<Order> feedbacks1 = new List<Order>();
            try
            {
                _electronickshopContext.Users.ToList();
                _electronickshopContext.Products.ToList();
                _electronickshopContext.StatusOrders.ToList();
                var feedbacks = await _electronickshopContext.Orders.Where(i => i.IdStatusOrderNavigation.NameStatus == "Выдан покупателю" || i.IdStatusOrderNavigation.NameStatus == "Отменен").ToListAsync();
                feedbacks1 = feedbacks;
                return feedbacks1;

            }
            catch { return feedbacks1; }
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
            var z = _electronickshopContext.HelperBaskets.Max(u => u.IdhelperBasket);
            if (z != 0)
            {
                var b = _electronickshopContext.HelperBaskets.Max(u => u.IdhelperBasket);
                return b;
            }
            else
                return 0;
        }

        //public async Task deleteBasketProduct(HelperBasket SelectedProduct)
        //{
        //    ObservableCollection<HelperBasket> Helpers = getAllHelperBasket();

        //    var item = Helpers.First(i => i.IdhelperBasket == SelectedProduct.IdhelperBasket);
        //    var index = Helpers.IndexOf(item);
        //    if (item.Count > 1)
        //    {
        //        double x = item.Cost;
        //        x = x / (double)item.Count;
        //        item.Count = item.Count - 1;
        //        item.Cost = item.Cost - x;
        //        item.Cost = Math.Round(item.Cost, 2);
        //        Helpers.RemoveAt(index);
        //        Helpers.Insert(index, item);
        //    }
        //    else {
        //        System.Windows.MessageBox.Show("Remove");
        //        Helpers.RemoveAt(index);
        //    }
        //    await _electronickshopContext.SaveChangesAsync();
        //}
    }
}

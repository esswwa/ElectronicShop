using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using AutoMapper;
using ElectronicShop.Models;
using ElectronicShop.Data.Model;
using System.Windows.Forms;

namespace ElectronicShop.Services
{
    public class ProductService
    {
        private readonly IMapper _mapper;
        private readonly ElectronickshopContext _electronickshopContext;
        public ProductService(ElectronickshopContext electronickshopContext)
        {
            _electronickshopContext = electronickshopContext;
            //_mapper = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Product, DbProduct>();
            //}).CreateMapper();

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
            var item = OrderHelper1.First(i => i.Idorder == OrderHelper.Idorder);
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


        public List<Firm> getFirms()
        {
            return _electronickshopContext.Firms.ToList();
        }

        public List<Status> getStatuses() { 

            return _electronickshopContext.Statuses.ToList();
        }

        public List<Category> getCategories()
        {
            return _electronickshopContext.Categories.ToList();
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
                var feedbacks = await _electronickshopContext.Orders.Where(i => i.IdStatusOrderNavigation.NameStatus != "Выдан покупателю" && i.IdStatusOrderNavigation.NameStatus != "Отменен" && i.IdUser == Settings.Default.idUser).ToListAsync();
                feedbacks1 = feedbacks;
                return feedbacks1;

            }
            catch { return feedbacks1; }
        }

        public async Task<List<Order>> getOrderAdminHelper()
        {
            List<Order> feedbacks1 = new List<Order>();
            try
            {
                _electronickshopContext.Users.ToList();
                _electronickshopContext.Products.ToList();
                _electronickshopContext.StatusOrders.ToList();
                var feedbacks = await _electronickshopContext.Orders.ToListAsync();
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
                var feedbacks = await _electronickshopContext.Orders.Where(i => (i.IdStatusOrderNavigation.NameStatus == "Выдан покупателю" || i.IdStatusOrderNavigation.NameStatus == "Отменен") && i.IdUser == Settings.Default.idUser).ToListAsync();
                feedbacks1 = feedbacks;
                return feedbacks1;

            }
            catch { return feedbacks1; }
        }

        public List<StatusOrder> getAllStatuses()
        {
            return _electronickshopContext.StatusOrders.ToList();
        }

        public async Task editOrder(Order order)
        {
            ObservableCollection<Order> Orders = _electronickshopContext.Orders.ToObservableCollection();
            var item = Orders.First(i => i.Idorder == order.Idorder);
            var index = Orders.IndexOf(item);

            item.DateReceipt = order.DateReceipt;
            item.IdStatusOrder = order.IdStatusOrder;

            Orders.RemoveAt(index);
            Orders.Insert(index, item);
            await _electronickshopContext.SaveChangesAsync();
        }

        public async Task editProductCount()
        {
            ObservableCollection<Product> Products = _electronickshopContext.Products.ToObservableCollection();
            var orders = await _electronickshopContext.HelperBaskets.Where(i => i.IdBasket == Settings.Default.idUser).ToListAsync();
            foreach(var it in orders)
            {
                var item = Products.First(i => i.IdProduct == it.IdProduct);
                var index = Products.IndexOf(item);

                item.CountProduct -= it.Count;
                if (item.CountProduct == 0)
                    item.Status = 1;
                Products.RemoveAt(index);
                Products.Insert(index, item);
            }
            await _electronickshopContext.SaveChangesAsync();
        }

        

        public async Task editProduct(string Article, string NameProduct, string SecondNameProduct, string ImgProduct, Firm FirmProduct, int CostProduct, Category CategoryProduct, float ReitingProduct, int CountProduct, Status Status, string Description)
        {
            ObservableCollection<Product> Products = _electronickshopContext.Products.ToObservableCollection();
            var item = Products.First(i => i.Article == Article);
            var index = Products.IndexOf(item);

            item.Article = Article;
            item.NameProduct = NameProduct;
            item.SecondNameProduct = SecondNameProduct;
            item.ImgProduct = ImgProduct;
            item.FirmProduct = FirmProduct.Idfirms;
            item.CostProduct = CostProduct;
            item.CategoryProduct = CategoryProduct.Idcategory;
            item.ReitingProduct = ReitingProduct;
            item.CountProduct = CountProduct;
            item.Status = Status.Idstatus;
            item.Description = Description;

            Products.RemoveAt(index);
            Products.Insert(index, item);
            await _electronickshopContext.SaveChangesAsync();
        }
        public async Task editFirm(int IdFirms, string Firm)
        {
            ObservableCollection<Firm> Firms = _electronickshopContext.Firms.ToObservableCollection();
            var item = Firms.First(i => i.Idfirms == IdFirms);
            var index = Firms.IndexOf(item);

            item.Firm1 = Firm;

            Firms.RemoveAt(index);
            Firms.Insert(index, item);
            await _electronickshopContext.SaveChangesAsync();
        }

        public async Task deleteProduct(Product product)
        {
            ObservableCollection<Product> Products = _electronickshopContext.Products.ToObservableCollection();
            var item = Products.First(i => i.IdProduct == product.IdProduct);
            var index = Products.IndexOf(item);

            item.Status = 2;

            Products.RemoveAt(index);
            Products.Insert(index, item);

            await _electronickshopContext.SaveChangesAsync();
        }

        public async Task editCategory(int idcategory, string categoryName, string categoryNameDeep)
        {
            ObservableCollection<Category> Categories = _electronickshopContext.Categories.ToObservableCollection();
            var item = Categories.First(i => i.Idcategory == idcategory);
            var index = Categories.IndexOf(item);

            item.CategoryName = categoryName;
            item.CategoryNameDeep = categoryNameDeep;

            Categories.RemoveAt(index);
            Categories.Insert(index, item);
            await _electronickshopContext.SaveChangesAsync();
        }
        public async Task AddCategory(string categoryName, string categoryNameDeep)
        {
            var idCategory = _electronickshopContext.Categories.Max(i => i.Idcategory);
            await _electronickshopContext.Categories.AddAsync(new Category
            {
                Idcategory = idCategory + 1,
                CategoryName = categoryName,
                CategoryNameDeep = categoryNameDeep

            });
            await _electronickshopContext.SaveChangesAsync();
        }
        public async Task AddFirm(string Firm)
        {
            var idFirm = _electronickshopContext.Firms.Max(i => i.Idfirms);
            await _electronickshopContext.Firms.AddAsync(new Firm
            {
                Idfirms = idFirm + 1,
                Firm1 = Firm

            });
            await _electronickshopContext.SaveChangesAsync();
        }


        public async Task AddProduct(string Article, string NameProduct, string SecondNameProduct, string ImgProduct, Firm FirmProduct, int CostProduct, Category CategoryProduct, float ReitingProduct, int CountProduct, Status Status, string Description)
        {
            var idProduct = _electronickshopContext.Products.Max(i => i.IdProduct);
            await _electronickshopContext.Products.AddAsync(new Product
            {
                IdProduct = idProduct + 1,
                Article = Article,
                NameProduct = NameProduct,
                SecondNameProduct = SecondNameProduct,
                ImgProduct = ImgProduct,
                FirmProduct = FirmProduct.Idfirms,
                CostProduct = CostProduct,
                CategoryProduct = CategoryProduct.Idcategory,
                ReitingProduct = ReitingProduct,
                CountProduct = CountProduct,
                Status = Status.Idstatus,
                Description = Description

            });
            await _electronickshopContext.SaveChangesAsync();
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

        private static readonly Random rnd = new();

        public int GetMaxOrderHelper()
        {
            var b = _electronickshopContext.Orders.Max(i => i.Idorder) + 1;
            return b;
        }

        public int GetMaxOrderHelperWithOut()
        {
            var b = _electronickshopContext.Orders.Max(i => i.Idorder);
            return b;
        }

        public bool checkFavourite(int idProduct)
        {
            var b = _electronickshopContext.Favourities.Where(i => i.IdUser == Settings.Default.idUser && i.IdProduct == idProduct).FirstOrDefault();
            if (b != null)
                return true;
            else
                return false;
        }

        

        public async Task addOrder(List<Product> product)
        {
            var helper = _electronickshopContext.HelperBaskets.Where(i => i.IdBasket == Settings.Default.idUser).ToList();

            List<double> cost = new List<double>();

            List<int> count = new List<int>();

            if (helper != null) {
                foreach (var item in helper)
                {

                    cost.Add(item.Cost);
                    count.Add(item.Count);
                }
            }
            ListProduct.counts = count;

            int z = _electronickshopContext.Orders.Max(i => i.Idorder) + 1;
            int z1 = 0;
            int code = rnd.Next(2,14);

            double allCost = 0;
            foreach (var item in cost) {
                allCost += item;
            }
            ListProduct.cost = allCost;
            await _electronickshopContext.Orders.AddAsync(new Order
            {
                Idorder = z,
                DateOrder = DateOnly.FromDateTime(DateTime.Now),
                IdUser = Settings.Default.idUser,
                IdStatusOrder = 0,
                DateReceipt = DateOnly.FromDateTime(DateTime.Now.AddDays(code)),
                IdOrderHelper = z,
                AllCost = allCost

            });
            int l = 0;
            foreach (var item in product) {
                if (z1 == 0)
                    z1 = _electronickshopContext.OrderHelpers.Max(i => i.IdorderHelper) + 1;
                else
                    z1 = z1 + 1;
                await _electronickshopContext.OrderHelpers.AddAsync(new OrderHelper
                {
                    IdorderHelper = z1,
                    IdOrder = z,
                    IdProduct = item.IdProduct,
                    Count = count[l],
                    Cost = cost[l]
                });
                l++;
            }

            ObservableCollection<HelperBasket> Helpers = getAllHelperBasket();
            foreach (var item2 in helper)
            {
                var item = Helpers.First(i => i.IdhelperBasket == item2.IdhelperBasket);
                var index = Helpers.IndexOf(item);
                Helpers.RemoveAt(index);
                _electronickshopContext.HelperBaskets.Remove(item);
            }
                await _electronickshopContext.SaveChangesAsync();


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

using ElectronicShop.Models;
using ElectronicShop.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.ViewModels
{
    public class FavouriteViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly UserService _userService;

        public string Login { get; set; }
        public string LoginBack { get; set; }
        public string Email { get; set; }

        public string Adress { get; set; }
        public bool CheckBox { get; set; }
        public DelegateCommand<string> commandCategories { get; set; }


        public List<string> Sorts { get; set; } = new() {
            "По возрастанию (Цена)",
            "По убыванию (Цена)"
        };

        public List<string> Filters { get; set; } = new() {
            "Персональный компьютер",
            "Ноутбук",
            "Периферия",
            "Смартфон",
            "Фототехника",
            "Телевизор",
            "Консоль",
            "Аудио",
            "Видеокарта",
            "Процессор",
            "Материнская плата",
            "Оперативная память",
            "Корпус",
            "Блок питания",
            "Накопитель",
            "Wi-Fi Роутер",
            "Видеонаблюдение",
            "Духовая печь",
            "Стиральная машина",
            "Холодильник"

        };
        public List<string> FiltersStatus { get; set; } = new() {
            "В наличии",
            "Нет в наличии",
            "Снят с продаж"

        };
        public bool IsEnabledCart { get; set; }

        public List<Product> Products { get; set; }

        public Product SelectedProduct { get; set; }

        public string FullName { get; set; } = Settings.Default.login == string.Empty ? "Гость" : Settings.Default.login;

        public bool IsCheckedFavourite { get; set; }

        public bool IsCheckedBasket { get; set; }

        public string textHeader { get; set; }

        public string SelectedSort
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }

        public string SelectedFilter
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }

        public string Search
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }

        public FavouriteViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;
            SelectedFilter = "Все диапазоны";
            if (Settings.Default.login != "")
            {
                Login = Settings.Default.login;
                LoginBack = Settings.Default.login;
                Email = Settings.Default.email;
            }
            UpdateProduct();
            commandCategories = new DelegateCommand<string>(TheoryMethod);
            textHeader = "Избранные";
            IsCheckedFavourite = true;
        }
        List<string> strings = new List<string>();

        private void TheoryMethod(string parametr)
        {

            if (strings != null)
            {
                bool z = false;
                foreach (var c in strings)
                {
                    if (parametr == c)
                    {

                        int b = strings.IndexOf(c);
                        z = true;
                        break;
                    }
                }
                if (z && strings.Count > 1)
                {
                    strings.Remove(parametr);
                }
                else if (z && strings.Count == 1)
                {
                    strings.Remove(parametr);
                }
                else
                {
                    strings.Add(parametr);
                }
            }
            else
            {
                strings.Add(parametr);
            }
            UpdateProduct();
        }

        List<Product> currentProduct;

        private async void UpdateProduct()
        {
            currentProduct = await _productService.GetFavouriteProducts();
            if (currentProduct != null)
                ListProduct.products = currentProduct;
            else
                currentProduct = ListProduct.products;
            if (!string.IsNullOrEmpty(Search))
            {
                currentProduct = currentProduct.Where(p => p.NameProduct.ToLower().Contains(Search.ToLower()) || p.Article.ToLower() == Search.ToLower()).ToList();
            }
            int z = 0;
            int b = 0;
            List<Product> newProduct = new List<Product>();
            var checkProduct = currentProduct;

            foreach (var strings1 in strings)
            {
                if (!string.IsNullOrEmpty(strings1))
                {
                    if (strings1 == "В наличии" || strings1 == "Нет в наличии" || strings1 == "Снят с продаж")
                    {
                        for (int i = 0; i < FiltersStatus.Count - 1; i++)
                        {
                            if (strings1 == FiltersStatus[i])
                            {
                                if (b == 0 && strings.Count <= 1)
                                {
                                    currentProduct = currentProduct.Where(p => p.Status == i).ToList();
                                }
                                else if (b == 0 && strings.Count > 1)
                                {
                                    currentProduct = currentProduct.Where(p => p.Status == i).ToList();
                                    b++;
                                }
                                else if (b > 0 && strings.Count > 1)
                                {
                                    newProduct = checkProduct.Where(p => p.Status == i).ToList();
                                    currentProduct.AddRange(newProduct);
                                    b++;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Filters.Count - 1; i++)
                        {
                            if (strings1 == Filters[i])
                            {
                                if (z == 0 && strings.Count <= 1)
                                {
                                    currentProduct = currentProduct.Where(p => p.CategoryProduct == i).ToList();
                                }
                                else if (z == 0 && strings.Count > 1)
                                {
                                    currentProduct = currentProduct.Where(p => p.CategoryProduct == i).ToList();
                                    z++;
                                }
                                else if (z > 0 && strings.Count > 1)
                                {
                                    newProduct = checkProduct.Where(p => p.CategoryProduct == i).ToList();
                                    currentProduct.AddRange(newProduct);
                                    z++;
                                }
                            }
                        }
                    }


                }
            }


            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По возрастанию (Цена)":
                        currentProduct = currentProduct.OrderBy(p => p.CostProduct).ToList();
                        break;
                    case "По убыванию (Цена)":
                        currentProduct = currentProduct.OrderByDescending(p => p.CostProduct).ToList();
                        break;
                }


            }

            Products = currentProduct;


        }


        public DelegateCommand deleteFavourite => new(async () =>
        {
            await _productService.deleteFavourite(SelectedProduct);
            UpdateProduct();
        });
        public DelegateCommand addInBasket => new(async () =>
        {
            var abc = await _productService.getUserHelperBasket(SelectedProduct);
            if (abc == null)
            {
                int maxHelper = _productService.GetMaxHelper() + 1;
                bool z = _productService.getUserHelper(SelectedProduct);
                if (z == true)
                    await _productService.AddHelperBasket(maxHelper, Settings.Default.idUser, SelectedProduct.IdProduct, SelectedProduct.CostProduct);
                else
                    await _productService.editHelperBasket(await _productService.getUserHelperBasket(SelectedProduct), true);
            }
            else
            {
                if (SelectedProduct.CountProduct > abc.Count)
                {
                    int maxHelper = _productService.GetMaxHelper() + 1;
                    bool z = _productService.getUserHelper(SelectedProduct);
                    if (z == true)
                        await _productService.AddHelperBasket(maxHelper, Settings.Default.idUser, SelectedProduct.IdProduct, SelectedProduct.CostProduct);
                    else
                        await _productService.editHelperBasket(await _productService.getUserHelperBasket(SelectedProduct), true);
                }
                else
                {
                    MessageBox.Show("Товар закончился");
                }
            }
        });

        public DelegateCommand Basket => new(() => _pageService.ChangePage(new BasketPage()));
        public DelegateCommand Order => new(() => _pageService.ChangePage(new OrderPage()));


        private List<string> _userLogin { get; set; } = new();
        private List<string> _userEmail { get; set; } = new();
        public DelegateCommand EditUser => new(async () => {


            _userLogin = await _userService.GetAllUser();
            _userEmail = await _userService.GetAllEmail();

            if (!_userLogin.Contains(LoginBack) && !_userEmail.Contains(Email))
            {
                Settings.Default.login = LoginBack;
                Settings.Default.email = Email;
                Settings.Default.Adress = Adress;
                Login = LoginBack;
                await _productService.editUser();
            }
            else
            {
                LoginBack = Settings.Default.login;
                Email = Settings.Default.email;
                Adress = Settings.Default.Adress;
            }
        });


        public DelegateCommand buyProduct => new(() =>
        {
            SelectProduct.product = SelectedProduct;
            _pageService.ChangePage(new ProductPage());
        });
        public DelegateCommand MenuPage => new(() =>
        {
            _pageService.ChangePage(new MenuPage());
        });
        public DelegateCommand ExitAcc => new(() =>
        {
            _pageService.ChangePage(new AuthorizationPage());
            _userService.UpdateProductNull();
        });
    }
}

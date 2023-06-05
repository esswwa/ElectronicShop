using DevExpress.Internal.WinApi;
using ElectronicShop.Data.Model;
using ElectronicShop.Models;
using ElectronicShop.Properties;
using ElectronicShop.Services;
using MaterialDesignThemes.Wpf;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicShop.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly UserService _userService;

        public string Login { get; set; }
        public string LoginBack { get; set; }
        public string Email { get; set; }
        
        public bool CheckBox { get; set; }
        public DelegateCommand<string> commandCategories { get; set; }


        public List<string> Sorts { get; set; } = new() {
            "По производителю",
            "По возрастанию (Цена)",
            "По убыванию (Цена)",
            "По возрастанию (Скидка)",
            "По убыванию (Скидка)"
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

        public List<Product> product1;

        public MenuViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;
            SelectedFilter = "Все диапазоны";
            IsCheckedFavourite = true;
            if (Settings.Default.login != "") {
                Login = Settings.Default.login;
                LoginBack = Settings.Default.login;
                Email = Settings.Default.email;
            }
            product1 = Task.Run(async () => await _productService.GetProducts()).Result;
            commandCategories = new DelegateCommand<string>(TheoryMethod);

        }
        List<string> strings = new List<string>();

    private void TheoryMethod(string parametr)
        {

            if (strings != null)
            {
                bool z = false;
                foreach (var c in strings) {
                    if (parametr == c) {

                        int b = strings.IndexOf(c);
                        z = true;
                        break;
                    }
                }
                if (z && strings.Count > 1)
                {
                    strings.Remove(parametr);
                    UpdateProduct("Фильтр");
                }
                else if (z && strings.Count == 1) {
                    strings.Remove(parametr);
                    UpdateProduct("Продукты");
                }
                else
                {
                    strings.Add(parametr);
                    UpdateProduct("Фильтр");
                }
            }
            else {
                strings.Add(parametr);
                UpdateProduct("Фильтр");
            }
        }

      
        public DelegateCommand addInFavourite => new(() =>
        {
        });
        public DelegateCommand addInBasket => new(() =>
        {
            int maxHelper = _productService.GetMaxHelper() + 1;
            bool z = _productService.getUserHelper(SelectedProduct);
            if(z == true)
                _productService.AddHelperBasket(maxHelper, Settings.Default.idUser, SelectedProduct.IdProduct ,SelectedProduct.CostProduct);
            else
                _productService.editHelperBasket(_productService.getUserHelperBasket(SelectedProduct), true); 
        });

        List<Product> currentProduct;

        private async void UpdateProduct(string check)
        {
                if (product1 != null)
                {
                     currentProduct = product1;
                }
                else
                {
                     currentProduct = await _productService.GetProducts();
                }
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
                                else {
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
                 
                    Products = currentProduct;
        }

        public DelegateCommand Basket => new(() =>
        {
            _pageService.ChangePage(new BasketPage());
        });
        public DelegateCommand Favourite => new(async () =>
        {
            var currentProduct = await _productService.GetFavouriteProducts();
            Products = currentProduct;
        });
        public DelegateCommand Order => new(() =>
        {
            _pageService.ChangePage(new OrderPage());
        });
        public DelegateCommand ExitAcc => new(() =>
        {
            _pageService.ChangePage(new AuthorizationPage());
            _userService.UpdateProductNull();
        });
    }
}

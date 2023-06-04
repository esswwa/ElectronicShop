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
            "Смартфон",
            "Телевизор",
            "Видеокарта",
            "Wi-Fi Роутер",
            "Ноутбук",
            "Периферия",
            "Фототехника",
            "Консоль",
            "Аудио",
            "Процессор",
            "Материнская плата",
            "Оперативная память",
            "Корпус",
            "Накопитель",
            "Блок питания",
            "Видеонаблюдение",
            "Духовая печь",
            "Стиральная машина",
            "Холодильник"
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

        public MenuViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;
            SelectedFilter = "Все диапазоны";
            UpdateProduct();
            IsCheckedFavourite = true;
            if (Settings.Default.login != "") {
                Login = Settings.Default.login;
                LoginBack = Settings.Default.login;
                Email = Settings.Default.email;
            }

            commandCategories = new DelegateCommand<string>(TheoryMethod);

        }
        string strings = "";
        private void TheoryMethod(string parametr)
        {
            strings = parametr;
            UpdateProduct();
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

        
        private async void UpdateProduct()
        {
            
            var currentProduct = await _productService.GetProducts();
            MessageBox.Show(currentProduct[0].CategoryProductNavigation.CategoryNameDeep);
            if (!string.IsNullOrEmpty(Search))
                currentProduct = currentProduct.Where(p => p.NameProduct.ToLower().Contains(Search.ToLower())).ToList();
            //if (!string.IsNullOrEmpty(strings))
            //    currentProduct = currentProduct.Where(p => p.CategoryProductNavigation.CategoryNameDeep == strings).ToList();
            
            Products = currentProduct;


            //if (!string.IsNullOrEmpty(SelectedSort))
            //{
            //    switch (SelectedSort)
            //    {
            //        case "По производителю":
            //            currentProduct = currentProduct.OrderBy(p => p.Manufacturer).ToList();
            //            break;
            //        case "По возрастанию (Цена)":
            //            currentProduct = currentProduct.OrderBy(p => p.Price).ToList();
            //            break;
            //        case "По убыванию (Цена)":
            //            currentProduct = currentProduct.OrderByDescending(p => p.Price).ToList();
            //            break;
            //        case "По возрастанию (Скидка)":
            //            currentProduct = currentProduct.OrderBy(p => p.Discount).ToList();
            //            break;
            //        case "По убыванию (Скидка)":
            //            currentProduct = currentProduct.OrderByDescending(p => p.Discount).ToList();
            //            break;
            //    }
            //}


        }

        public DelegateCommand Basket => new(() =>
        {
            _pageService.ChangePage(new BasketPage());
        });
        public DelegateCommand Favourite => new(() =>
        {
            _pageService.ChangePage(new FavouritePage());
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

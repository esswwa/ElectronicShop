using ElectronicShop.Data.Model;
using ElectronicShop.Models;
using ElectronicShop.Properties;
using ElectronicShop.Services;
using MaterialDesignThemes.Wpf;
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
        

        public List<string> Sorts { get; set; } = new() {
            "По производителю",
            "По возрастанию (Цена)",
            "По убыванию (Цена)",
            "По возрастанию (Скидка)",
            "По убыванию (Скидка)"
        };

        public List<string> Filters { get; set; } = new() {
            "Все диапазоны",
            "Без скидки",
            "1-5%",
            "5-9%",
            "9% и более"
        };
        public bool IsEnabledCart { get; set; }

        public List<Product> Products { get; set; }

        public Product SelectedProduct { get; set; }

        public string FullName { get; set; } = Settings.Default.login == string.Empty ? "Гость" : Settings.Default.login;

        public int? MaxRecords { get; set; } = 0;

        public int? Records { get; set; } = 0;

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
        public DelegateCommand addInFavourite => new(() =>
        {
        });
        public DelegateCommand addInBasket => new(() =>
        {
            foreach (var check in BasketCard.products) { 
            
            }

            BasketCard.products.Add(SelectedProduct);
        });

        
        private async void UpdateProduct()
        {
            var currentProduct = await _productService.GetProducts();
            Products = currentProduct;
           
            //if (!string.IsNullOrEmpty(SelectedFilter))
            //{
            //    switch (SelectedFilter)
            //    {
            //        case "Без скидки":
            //            currentProduct = currentProduct.Where(p => p.Discount == 0).ToList();
            //            break;
            //        case "1-5%":
            //            currentProduct = currentProduct.Where(p => p.Discount > 0 && p.Discount < 5).ToList();
            //            break;
            //        case "5-9%":
            //            currentProduct = currentProduct.Where(p => p.Discount >= 5 && p.Discount < 9).ToList();
            //            break;
            //        case "9% и более":
            //            currentProduct = currentProduct.Where(p => p.Discount >= 9).ToList();
            //            break;
            //    }
            //}



            //if (!string.IsNullOrEmpty(Search))
            //    currentProduct = currentProduct.Where(p => p.Title.ToLower().Contains(Search.ToLower())).ToList();



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

        //public DelegateCommand SignOutCommand => new(() =>
        //{
        //    Settings.Default.UserId = 0;
        //    Settings.Default.UserName = string.Empty;
        //    Settings.Default.UserSurname = string.Empty;
        //    Settings.Default.UserPatronymic = string.Empty;
        //    Settings.Default.RoleName = string.Empty;
        //    _pageService.ChangePage(new SingInPage());
        //});
        //public DelegateCommand AddCardToBasket => new(() =>
        //{
        //    var card = Global.CurrentCart.SingleOrDefault(c => c.ArticleName.Equals(SelectedProduct.Article));
        //    if (card == null)
        //    {
        //        Global.CurrentCart.Add(new Card
        //        {
        //            ArticleName = SelectedProduct.Article,
        //            Count = 1
        //        });
        //    }
        //    else
        //    {

        //        card.Count++;
        //        Global.CurrentCart[Global.CurrentCart.FindIndex(c => c.ArticleName.Equals(card.ArticleName))] = card;
        //    }
        //    CheckEnabled();
        //});

        //public DelegateCommand CardCommand => new(() =>
        //{
        //    _pageService.ChangePage(new BasketInfoPage());
        //});

    }
}

using ElectronicShop.Models;
using ElectronicShop.Properties;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ElectronicShop.ViewModels
{
    public class BasketViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly UserService _userService;

        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }

        public string Adress { get; set; }  

        public string CountProduct { get; set; }

        public string CostProduct { get; set; }

        public BasketViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;
            UpdateProduct();
        }

        private async void UpdateProduct()
        {
            var currentProduct = await _productService.getUserHelperBasketList();
           
            Products = currentProduct;
            int cost = 0;
            foreach (var product in Products)
            {
                cost += product.CostProduct;
            }
            CountProduct = Products.Count.ToString();
            CostProduct = cost.ToString();
            Adress = Settings.Default.Adress;
        }

        public DelegateCommand Basket => new(() =>
        {
            _pageService.ChangePage(new BasketPage());
        });
        public DelegateCommand CommandMenu => new(() =>
        {
            _pageService.ChangePage(new MenuPage());
        });
        public DelegateCommand Favourite => new(async () =>
        {
            _pageService.ChangePage(new FavouritePage());
        });
        public DelegateCommand Order => new(() =>
        {
            _pageService.ChangePage(new OrderPage());
        });

    }
}
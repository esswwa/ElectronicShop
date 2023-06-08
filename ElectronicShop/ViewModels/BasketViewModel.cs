using ElectronicShop.Models;
using ElectronicShop.Properties;
using ElectronicShop.Services;
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

        public string CountProduct1 { get; set; }

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
            var Helpers = Task.Run(async () => await _productService.getAllHelperBasketUser()).Result;
            double cost = 0;
            int Count = 0;
            foreach (var product in Products)
            {
                foreach (var index in Helpers) {

                    if (index.IdProduct == product.IdProduct)
                    {
                        cost += index.Cost;
                        Count += index.Count;
                    }
                }
            }
            CountProduct = Products.Count.ToString();
            CountProduct1 = Count.ToString();
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

        public DelegateCommand buyFastProduct => new(() =>
        {
            _pageService.ChangePage(new OrderPage());
        });

        
        public DelegateCommand addInBasket => new(async () =>
        {
            int maxHelper = 0;
            if (_productService.GetMaxHelper() > 0)
                maxHelper = _productService.GetMaxHelper() + 1;
            else
                maxHelper = 0;
            bool z = _productService.getUserHelper(SelectedProduct);
            if (z == true)
                await _productService.AddHelperBasket(maxHelper, Settings.Default.idUser, SelectedProduct.IdProduct, SelectedProduct.CostProduct);
            else
                await _productService.editHelperBasket(_productService.getUserHelperBasket(SelectedProduct), true);
            UpdateProduct();
        });
        public DelegateCommand deleteFromBasket => new(async () =>
        {

            //int maxHelper = _productService.GetMaxHelper() + 1;
            //bool z = _productService.getUserHelper(SelectedProduct);
            //if (z == true)
            //    await _productService.AddHelperBasket(maxHelper, Settings.Default.idUser, SelectedProduct.IdProduct, SelectedProduct.CostProduct);
            //else
            await _productService.editHelperBasket(_productService.getUserHelperBasket(SelectedProduct), false);
            UpdateProduct();

            //HelperBasket SelectedHelp = _productService.getUserHelperBasket(SelectedProduct);
            //await _productService.deleteBasketProduct(SelectedHelp);
            //UpdateProduct();
        });

        
    }
}
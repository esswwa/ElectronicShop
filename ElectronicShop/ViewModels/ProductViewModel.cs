using ElectronicShop.Data.Model;
using ElectronicShop.Models;
using ElectronicShop.Properties;
using ElectronicShop.Services;
using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ElectronicShop.ViewModels
{
    public class ProductViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly UserService _userService;

        public string NameProduct { get; set; }
        public string ImgProduct { get; set; }
        public string SecondNameProduct { get; set; }
        public string ReitingProduct1 { get; set; }
        public float ReitingProduct { get; set; }
        public string CostProduct { get; set; }
        public bool IsCheckedButton { get; set; }
        public string CountProduct { get; set; }
        public string Description { get; set; }
        public string CountFeedback { get; set; }

        public ProductViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;

            NameProduct = SelectProduct.product.NameProduct;
            ImgProduct = SelectProduct.product.ImgProduct;
            SecondNameProduct = SelectProduct.product.SecondNameProduct;
            ReitingProduct1 = SelectProduct.product.ReitingProduct.ToString();
            ReitingProduct = SelectProduct.product.ReitingProduct;
            CostProduct = SelectProduct.product.CostProduct.ToString();
            IsCheckedButton = false;
            CountProduct = SelectProduct.product.CountProduct.ToString();
            Description = SelectProduct.product.Description;
            CountFeedback = "0";
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

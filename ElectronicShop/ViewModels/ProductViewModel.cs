using ElectronicShop.Data.Model;
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

        public ProductViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;
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

using ElectronicShop.Services;
using MaterialDesignColors;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.ViewModels
{
    public class OrderViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly UserService _userService;

        public List<Order> OrderHelper { get; set; }
        public Order SelectedOrderHelper { get; set; }
        public List<Order> OrderHelper1 { get; set; }
        public Order SelectedOrderHelper1 { get; set; }

        public OrderViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;
            UpdateProduct();
        }
        private async void UpdateProduct()
        {
            OrderHelper = await _productService.getOrderHelper();
            OrderHelper1 = await _productService.getOrderHelper1();
        }
        public DelegateCommand Basket => new(() => _pageService.ChangePage(new BasketPage()));
        public DelegateCommand CommandMenu => new(() => _pageService.ChangePage(new MenuPage()));
        public DelegateCommand Favourite => new(() => _pageService.ChangePage(new FavouritePage()));
        public DelegateCommand cancelOrder => new(async() =>
        {
            await _productService.editOrderStatus(SelectedOrderHelper);
            UpdateProduct();
        });
        
    }
}

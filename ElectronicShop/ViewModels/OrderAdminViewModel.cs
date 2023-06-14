using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.ViewModels
{
    public class OrderAdminViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly UserService _userService;

        public List<Order> OrderHelper { get; set; }
        public Order SelectedOrderHelper { get; set; }

        public string textHeader { get; set; }
        public string IdOrder { get; set; }
        public string DateReceipt { get; set; }
        public List<string> StatusOrder { get; set; }
        public string StatusesOrder { get;set; }

        public bool boolVisibility { get; set; }
        public OrderAdminViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService; 
            boolVisibility = false;
            UpdateProduct();
        }
        private async void UpdateProduct()
        {
            OrderHelper = await _productService.getOrderHelper();
        }
        public DelegateCommand Basket => new(() => _pageService.ChangePage(new BasketPage()));
        public DelegateCommand CommandMenu => new(() => _pageService.ChangePage(new MenuPage()));
        public DelegateCommand Favourite => new(() => _pageService.ChangePage(new FavouritePage()));
        public DelegateCommand cancelOrder => new(async () =>
        {
            await _productService.editOrderStatus(SelectedOrderHelper);
            UpdateProduct();
        });

        
        public DelegateCommand editOrders => new(async () =>
        {
            boolVisibility = true;
            IdOrder = SelectedOrderHelper.Idorder.ToString();
            DateReceipt = SelectedOrderHelper.DateReceipt.ToString();
            StatusesOrder = SelectedOrderHelper.Idorder.ToString();

        });
        public DelegateCommand EditOrder => new(async () =>
        {
           
        });

        

    }
}

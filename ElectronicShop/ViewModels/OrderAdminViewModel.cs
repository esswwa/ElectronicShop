using ElectronicShop.Data.Model;
using ElectronicShop.Views;
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
        public DateTime DateReceipt { get; set; }
        public List<StatusOrder> StatusOrder { get; set; }
        public StatusOrder StatusesOrder { get;set; }

        public Visibility boolVisibility { get; set; }
        public OrderAdminViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;
            boolVisibility = Visibility.Hidden;
            UpdateProduct();
        }
        private async void UpdateProduct()
        {
            OrderHelper = await _productService.getOrderAdminHelper();
        }
        public DelegateCommand Basket => new(() => _pageService.ChangePage(new EditAndAddsProducts()));

        public DelegateCommand Exit => new(() =>
        {
            _pageService.ChangePage(new AuthorizationPage());
            _userService.UpdateProductNull();
        });

        
        public DelegateCommand cancelOrder => new(async () =>
        {
            await _productService.editOrderStatus(SelectedOrderHelper);
            UpdateProduct();
            boolVisibility = Visibility.Hidden;
        });

        
        public DelegateCommand editOrders => new(async () =>
        {
            boolVisibility = Visibility.Visible;
            IdOrder = SelectedOrderHelper.Idorder.ToString();
            DateOnly date = SelectedOrderHelper.DateReceipt;
            DateReceipt = date.ToDateTime(new TimeOnly());

            List<StatusOrder> Statuses = _productService.getAllStatuses();
            StatusOrder = Statuses;
            StatusesOrder = SelectedOrderHelper.IdStatusOrderNavigation;

        });
        public DelegateCommand EditOrder => new(async () =>
        {
            SelectedOrderHelper.IdStatusOrder = StatusesOrder.IdstatusOrder;
            SelectedOrderHelper.DateReceipt = DateOnly.FromDateTime(DateReceipt);
            await _productService.editOrder(SelectedOrderHelper);
            boolVisibility = Visibility.Hidden;
            UpdateProduct();
        });

    }
}

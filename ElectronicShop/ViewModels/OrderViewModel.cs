using ElectronicShop.Data.Model;
using ElectronicShop.Services;
using MaterialDesignColors;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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

        public Order selOrd = new();
        public DelegateCommand cancelOrder => new(async() =>
        {
            await _productService.editOrderStatus(SelectedOrderHelper);
            UpdateProduct();
            selOrd = SelectedOrderHelper;
            await Task.Delay(500);

            MailAddress from = new MailAddress(_userService.checkAdress(), "ELEISSIS");
            MailAddress to = new MailAddress(Settings.Default.email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = $"Изменение в заказе №{selOrd.Idorder}";
            m.Body = $"Здравствуйте, {Settings.Default.login}!\n\nВаш заказ был отменен. Если произошла ошибка или нужно поменять что-либо в заказе, то повторите ваш заказ :).\n\nСпасибо, что выбрали наш магазин!\n\nС уважением,\nКоманда магазина ELEISSIS.";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(_userService.checkAdress(), _userService.checkPassword());
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        });
        
    }
}

using ElectronicShop.Data.Model;
using ElectronicShop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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

            await Task.Delay(500);

            MailAddress from = new MailAddress(_userService.checkAdress(), "ELEISSIS");
            MailAddress to = new MailAddress(SelectedOrderHelper.IdUserNavigation.Email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = $"Изменение в заказе №{SelectedOrderHelper.Idorder}";
            //if (SelectedOrderHelper1.IdStatusOrder != StatusesOrder.IdstatusOrder && SelectedOrderHelper1.DateReceipt != DateOnly.FromDateTime(DateReceipt))
            //{
                m.Body = $"Здравствуйте, {SelectedOrderHelper.IdUserNavigation.Login}!\n\nВ вашем заказе произошли изменения.\n\n Заказ будет доставлен: {DateOnly.FromDateTime(DateReceipt)} c 9:00 до 18:00.\n Статус заказа: {StatusesOrder.NameStatus}.\n\n Спасибо, что выбрали наш магазин!\n\nС уважением,\nКоманда магазина ELEISSIS.\n\n";
            //}
            //else if (SelectedOrderHelper1.IdStatusOrder != StatusesOrder.IdstatusOrder)
            //{
            //    m.Body = $"Здравствуйте, {SelectedOrderHelper.IdUserNavigation.Login}!\n\nВ вашем заказе изменен статус заказа.\n Статус заказа: {StatusesOrder.NameStatus}.\n\n Заказ будет доставлен: {DateOnly.FromDateTime(DateReceipt)} c 9:00 до 18:00.\n\nСпасибо, что выбрали наш магазин!\n\nС уважением,\nКоманда магазина ELEISSIS.\n\n"
            //      +
            //      "Отправлено с помощью MailAdress и SMTP-client.";
            //}
            //else if (SelectedOrderHelper1.DateReceipt != DateOnly.FromDateTime(DateReceipt))
            //{
            //    m.Body = $"Здравствуйте, {SelectedOrderHelper.IdUserNavigation.Login}!\n\nВ вашем заказе изменена дата доставки.\nЗаказ будет доставлен: {DateOnly.FromDateTime(DateReceipt)} c 9:00 до 18:00.\n\n Статус заказа: {StatusesOrder.NameStatus}.\n\nСпасибо, что выбрали наш магазин!\n\nС уважением,\nКоманда магазина ELEISSIS.\n\n"
            //      +
            //      "Отправлено с помощью MailAdress и SMTP-client.";
            //}
            //MessageBox.Show(SelectedOrderHelper1.IdStatusOrder + " kkkkkkkkkk " + SelectedOrderHelper.IdStatusOrder);
            //MessageBox.Show(m.Body);
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(_userService.checkAdress(), _userService.checkPassword());
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);

        });

    }
}

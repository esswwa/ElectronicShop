using ElectronicShop.Data.Model;
using ElectronicShop.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ElectronicShop.Models;

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
        public string ErrorMessage { get; set; }
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

        public DelegateCommand ResetCommand => new(() => {
            UpdateProduct();
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
        public AsyncCommand EditOrder => new(async () =>
        {
            SelectedOrderHelper.IdStatusOrder = StatusesOrder.IdstatusOrder;
            SelectedOrderHelper.DateReceipt = DateOnly.FromDateTime(DateReceipt);
            await _productService.editOrder(SelectedOrderHelper);
            boolVisibility = Visibility.Hidden;
            UpdateProduct();

            await Task.Delay(500);
            if (SelectedOrderHelper.IdStatusOrder == 3)
            {
                MailAddress from = new MailAddress(_userService.checkAdress(), "ELEISSIS");
                MailAddress to = new MailAddress(SelectedOrderHelper.IdUserNavigation.Email);
                MailMessage m = new MailMessage(from, to);
                m.Subject = $"Чек по заказу №{SelectedOrderHelper.Idorder} от {DateOnly.FromDateTime(DateTime.Now).ToString("D")}";
                m.Body = $"Здравствуйте, {SelectedOrderHelper.IdUserNavigation.Login}!\n\nВаш заказ успешно оформлен. Спасибо, что выбрали наш магазин!\n\nС уважением,\nКоманда магазина ELEISSIS.\n\n";
                string fileName = $"Товарный чек №{SelectedOrderHelper.Idorder}.pdf";
                string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                m.Attachments.Add(new Attachment(path));
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(_userService.checkAdress(), _userService.checkPassword());
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(m);
            }
            else {
                MailAddress from = new MailAddress(_userService.checkAdress(), "ELEISSIS");
                MailAddress to = new MailAddress(SelectedOrderHelper.IdUserNavigation.Email);
                MailMessage m = new MailMessage(from, to);
                m.Subject = $"Изменение в заказе №{SelectedOrderHelper.Idorder}";
                m.Body = $"Здравствуйте, {SelectedOrderHelper.IdUserNavigation.Login}!\n\nВ вашем заказе произошли изменения.\n\n Заказ будет доставлен: {DateOnly.FromDateTime(DateReceipt)} c 9:00 до 18:00.\n Статус заказа: {StatusesOrder.NameStatus}.\n\n Спасибо, что выбрали наш магазин!\n\nС уважением,\nКоманда магазина ELEISSIS.\n\n";
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(_userService.checkAdress(), _userService.checkPassword());
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(m);
            }
        }, bool () =>
        {
            if (StatusesOrder == null)
            {
                ErrorMessage = "Пустые поля";
                return false;
            }
            else
            {
                ErrorMessage = string.Empty;
            }

            return true;
        });

    }
}

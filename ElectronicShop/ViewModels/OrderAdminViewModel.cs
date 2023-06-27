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
        public DelegateCommand<string> commandCategories { get; set; }


        public List<string> Sorts { get; set; } = new() {
            "По возрастанию",
            "По убыванию"
        };
       
        public List<string> FiltersStatus { get; set; } = new() {
            "В обработке",
            "Принят",
            "Отправлен",
            "Выдан покупателю",
            "Отменен",
            "1"

        };

        public string SelectedSort
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }

        public OrderAdminViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;
            boolVisibility = Visibility.Hidden;
            UpdateProduct();
            commandCategories = new DelegateCommand<string>(TheoryMethod);
        }
        List<string> strings = new List<string>();

        private void TheoryMethod(string parametr)
        {

            if (strings != null)
            {
                bool z = false;
                foreach (var c in strings)
                {
                    if (parametr == c)
                    {

                        int b = strings.IndexOf(c);
                        z = true;
                        break;
                    }
                }
                if (z && strings.Count > 1)
                {
                    strings.Remove(parametr);
                }
                else if (z && strings.Count == 1)
                {
                    strings.Remove(parametr);
                }
                else
                {
                    strings.Add(parametr);
                }
            }
            else
            {
                strings.Add(parametr);
            }
            UpdateProduct();
        }

        private async void UpdateProduct()
        {
            var currentProduct = await _productService.getOrderAdminHelper();

            int b = 0;
            List<Order> newProduct = new List<Order>();
            var checkProduct = currentProduct;
            foreach (var strings1 in strings)
            {
                if (!string.IsNullOrEmpty(strings1))
                {
                    for (int i = 0; i < FiltersStatus.Count - 1; i++)
                    {
                        if (strings1 == FiltersStatus[i])
                        {
                            if (b == 0 && strings.Count <= 1)
                            {
                                currentProduct = currentProduct.Where(p => p.IdStatusOrder == i).ToList();
                            }
                            else if (b == 0 && strings.Count > 1)
                            {
                                currentProduct = currentProduct.Where(p => p.IdStatusOrder == i).ToList();
                                b++;
                            }
                            else if (b > 0 && strings.Count > 1)
                            {
                                newProduct = checkProduct.Where(p => p.IdStatusOrder == i).ToList();
                                currentProduct.AddRange(newProduct);
                                b++;
                            }
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По возрастанию":
                        currentProduct = currentProduct.OrderBy(p => p.Idorder).ToList();
                        break;
                    case "По убыванию":
                        currentProduct = currentProduct.OrderByDescending(p => p.Idorder).ToList();
                        break;
                }


            }

            OrderHelper = currentProduct;
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

            await Task.Delay(500);

            MailAddress from = new MailAddress(_userService.checkAdress(), "ELEISSIS");
            MailAddress to = new MailAddress(SelectedOrderHelper.IdUserNavigation.Email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = $"Изменение в заказе №{SelectedOrderHelper.Idorder}";
            m.Body = $"Здравствуйте, {SelectedOrderHelper.IdUserNavigation.Login}!\n\nВаш заказ был отменен. Если произошла ошибка или нужно поменять что-либо в заказе, то повторите ваш заказ :).\n\nСпасибо, что выбрали наш магазин!\n\nС уважением,\nКоманда магазина ELEISSIS.";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(_userService.checkAdress(), _userService.checkPassword());
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);

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
                m.Subject = $"Чек по заказу №{SelectedOrderHelper.Idorder}";
                m.Body = $"Здравствуйте, {SelectedOrderHelper.IdUserNavigation.Login}!\n\nВаш заказ успешно получен. Спасибо, что выбрали наш магазин!\n\nЗаказ получен: {DateOnly.FromDateTime(DateTime.Now).ToString("D")}\n\nС уважением,\nКоманда магазина ELEISSIS.";
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
                m.Body = $"Здравствуйте, {SelectedOrderHelper.IdUserNavigation.Login}!\n\nВ вашем заказе произошли изменения.\n\nЗаказ будет доставлен: {DateOnly.FromDateTime(DateReceipt)} c 9:00 до 18:00.\nСтатус заказа: {StatusesOrder.NameStatus}.\n\nСпасибо, что выбрали наш магазин!\n\nС уважением,\nКоманда магазина ELEISSIS.";
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

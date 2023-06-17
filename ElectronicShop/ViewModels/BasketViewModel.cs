using ElectronicShop.Services;
using System.Net.Mail;
using System.Net;

namespace ElectronicShop.ViewModels
{
    public class BasketViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly UserService _userService;
        private readonly DocumentService _documentService;
        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }

        public string Adress { get; set; }  

        public string CountProduct { get; set; }

        private static readonly Random rnd = new();
        public string CostProduct { get; set; }

        public string CountProduct1 { get; set; }

        public string textHeader { get; set; }

        public BasketViewModel(PageService pageService, ProductService productService, UserService userService, DocumentService documentService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;
            _documentService = documentService;
            UpdateProduct();
            textHeader = "Корзина";
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
        public DelegateCommand Favourite => new(() =>
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
        //533
        public DelegateCommand buyProduct => new(async () =>
        {
            int code = rnd.Next(100, 999);
            int orderCode = _productService.GetMaxOrderHelper();
            await _productService.addOrder(Products); 
            await _documentService.GetCheck(code, _productService.GetMaxOrderHelper(), Products);

            MailAddress from = new MailAddress(_userService.checkAdress(), "ELEISSIS");
            MailAddress to = new MailAddress(Settings.Default.email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = $"Чек по заказу №{orderCode} от {DateOnly.FromDateTime(DateTime.Now).ToString("D")}";
            m.Body = $"Здравствуйте, {Settings.Default.login}!\n\nВаш заказ успешно оформлен. Спасибо, что выбрали наш магазин!\n\nС уважением,\nКоманда магазина ELEISSIS.\n\n"
            +
            "Отправлено с помощью MailAdress и SMTP-client.";
            string fileName = "Товарный чек.pdf";
            string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            m.Attachments.Add(new Attachment(path));
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(_userService.checkAdress(), _userService.checkPassword());
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);


            //MailMessage message = new MailMessage();
            //message.From = new MailAddress("qweq95346@gmail.com", "Отправитель");
            //message.To.Add(new MailAddress("nnice2015@yandex.ru", "Получатель"));
            //message.Subject = "Тема сообщения";
            //message.Body = "Тело сообщения";

            //Attachment attachment = new Attachment(Path.GetFullPath("Товарный чек.pdf"));
            //message.Attachments.Add(attachment);

            //SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            //client.Credentials = new System.Net.NetworkCredential("qweq95346@gmail.com", "Qwerty123*/");
            //client.EnableSsl = true;

            //await client.SendMailAsync(message);

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
            MessageBox.Show(maxHelper.ToString());
            if (z == true)
                await _productService.AddHelperBasket(maxHelper, Settings.Default.idUser, SelectedProduct.IdProduct, SelectedProduct.CostProduct);
            else
                await _productService.editHelperBasket(await _productService.getUserHelperBasket(SelectedProduct), true);
            UpdateProduct();
        });
        public DelegateCommand deleteFromBasket => new(async () =>
        {
            // 1 вариант int maxHelper = _productService.GetMaxHelper() + 1;
            //bool z = _productService.getUserHelper(SelectedProduct);
            //if (z == true)
            //    await _productService.AddHelperBasket(maxHelper, Settings.Default.idUser, SelectedProduct.IdProduct, SelectedProduct.CostProduct);
            //else
            await _productService.editHelperBasket(await _productService.getUserHelperBasket(SelectedProduct), false);
            UpdateProduct();

            //2 вариантHelperBasket SelectedHelp = _productService.getUserHelperBasket(SelectedProduct);
            //await _productService.deleteBasketProduct(SelectedHelp);
            //UpdateProduct();
        });

        
    }
}
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

        public Visibility visibilityListView { get; set; }
        public string CountProduct1 { get; set; }

        public string textHeader { get; set; }

        public BasketViewModel(PageService pageService, ProductService productService, UserService userService, DocumentService documentService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;
            _documentService = documentService;
            visibilityListView = Visibility.Hidden;
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

            if (Products.Count == 0)
                visibilityListView = Visibility.Visible;
            else
                visibilityListView = Visibility.Hidden;
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
        public DelegateCommand buyProduct => new(async () =>
        {
            int orderCode = _productService.GetMaxOrderHelper();
            await _productService.editProductCount();
            await _productService.addOrder(Products); 
            await _documentService.GetCheck(_productService.GetMaxOrderHelperWithOut(), Products);
            _pageService.ChangePage(new OrderPage());

            await Task.Delay(500);

            MailAddress from = new MailAddress(_userService.checkAdress(), "ELEISSIS");
            MailAddress to = new MailAddress(Settings.Default.email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = $"Информация по заказу №{orderCode} от {DateOnly.FromDateTime(DateTime.Now).ToString("D")}";
            m.Body = $"Здравствуйте, {Settings.Default.login}!\n\nВаш заказ успешно оформлен. Оплата товара при получении.\nСпасибо, что выбрали наш магазин!\n\nС уважением,\nКоманда магазина ELEISSIS.";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(_userService.checkAdress(), _userService.checkPassword());
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        });

        
        public DelegateCommand addInBasket => new(async () =>
        {
            var abc = await _productService.getUserHelperBasket(SelectedProduct);
            if (abc == null)
            {
                int maxHelper = _productService.GetMaxHelper() + 1;
                bool z = _productService.getUserHelper(SelectedProduct);
                if (z == true)
                    await _productService.AddHelperBasket(maxHelper, Settings.Default.idUser, SelectedProduct.IdProduct, SelectedProduct.CostProduct);
                else
                    await _productService.editHelperBasket(await _productService.getUserHelperBasket(SelectedProduct), true);
                UpdateProduct();
            }
            else
            {
                if (SelectedProduct.CountProduct > abc.Count)
                {
                    int maxHelper = _productService.GetMaxHelper() + 1;
                    bool z = _productService.getUserHelper(SelectedProduct);
                    MessageBox.Show(maxHelper.ToString());
                    if (z == true)
                        await _productService.AddHelperBasket(maxHelper, Settings.Default.idUser, SelectedProduct.IdProduct, SelectedProduct.CostProduct);
                    else
                        await _productService.editHelperBasket(await _productService.getUserHelperBasket(SelectedProduct), true);
                    UpdateProduct();
                }
                else
                {
                    MessageBox.Show("Товар закончился");
                }
            }
                
        });
        public DelegateCommand deleteFromBasket => new(async () =>
        {
            await _productService.editHelperBasket(await _productService.getUserHelperBasket(SelectedProduct), false);
            UpdateProduct();
        });

        
    }
}
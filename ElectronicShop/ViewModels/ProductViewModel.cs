using ElectronicShop.Models;
using ElectronicShop.Properties;
using ElectronicShop.Services;
using MaterialDesignColors;
using Microsoft.VisualBasic;
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

        public string NameProduct { get; set; }
        public string ImgProduct { get; set; }
        public string SecondNameProduct { get; set; }
        public string ReitingProduct1 { get; set; }
        public float ReitingProduct { get; set; }
        public string CostProduct { get; set; }
        public bool IsCheckedButton { get; set; }
        public string CountProduct { get; set; }
        public string Description { get; set; }
        public string CountFeedback { get; set; }

        public double Value5 { get; set; }
        public double Value4 { get; set; }
        public double Value3 { get; set; }
        public double Value2 { get; set; }
        public double Value1 { get; set; }
        public static List<Feedback> Feedback { get; set; }


        public string dignitiesText { get; set; }
        public string downsideText { get; set; }
        public string FeedbackText { get; set; }

        public float ReitingAddProduct { get; set; }

        public bool IsEnabledCart { get; set; }

        public ProductViewModel(PageService pageService, ProductService productService, UserService userService)
        {
            _pageService = pageService;
            _productService = productService;
            _userService = userService;

            NameProduct = SelectProduct.product.NameProduct;
            ImgProduct = SelectProduct.product.ImgProduct;
            SecondNameProduct = SelectProduct.product.SecondNameProduct;
            ReitingProduct1 = SelectProduct.product.ReitingProduct.ToString();
            ReitingProduct = SelectProduct.product.ReitingProduct;
            CostProduct = SelectProduct.product.CostProduct.ToString();
            IsCheckedButton = false;
            CountProduct = SelectProduct.product.CountProduct.ToString();
            Description = SelectProduct.product.Description;
            UpdateProduct();
            CheckEnabled();
        }
        private void CheckEnabled(){
            var z = Feedback.Where(c => c.IdUser == Settings.Default.idUser && c.IdProduct == SelectProduct.product.IdProduct);
            if (!z.Any())
                IsEnabledCart = true;
            else
                IsEnabledCart = false;
        }
        private async void UpdateProduct()
        {
            Feedback = Task.Run(async () => await _productService.getFeedbackProduct()).Result;
            CountFeedback = Feedback.Count.ToString();

            foreach (var i in Feedback)
            {

                if (i.Reiting >= 0.5 && i.Reiting < 1.5)
                    Value1++;
                if (i.Reiting > 1.5 && i.Reiting < 2.5)
                    Value2++;
                if (i.Reiting > 2.5 && i.Reiting < 3.5)
                    Value3++;
                if (i.Reiting > 3.5 && i.Reiting < 4.5)
                    Value4++;
                if (i.Reiting > 4.5 && i.Reiting <= 5)
                    Value5++;
            }

            Value1 = Value1 / Feedback.Count * 100;
            Value2 = Value2 / Feedback.Count * 100;
            Value3 = Value3 / Feedback.Count * 100;
            Value4 = Value4 / Feedback.Count * 100;
            Value5 = Value5 / Feedback.Count * 100;
        }
        

        public DelegateCommand AddFeedback => new(async () =>
        {
            double rounded = Math.Round(ReitingAddProduct, 1);
            await _productService.addFeedback(downsideText, dignitiesText, FeedbackText, (float)rounded, SelectProduct.product.IdProduct);
            _productService.UpdateProductReiting(SelectProduct.product.IdProduct);
            downsideText = "";
            dignitiesText = "";
            FeedbackText = "";
            ReitingAddProduct = 0;
            MessageBox.Show("Комментарий добавлен");
            UpdateProduct();
        });

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

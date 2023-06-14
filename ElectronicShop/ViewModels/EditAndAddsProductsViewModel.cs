using ElectronicShop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.ViewModels
{
    public class EditAndAddsProductsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;

        public EditAndAddsProductsViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
        }

    }
}

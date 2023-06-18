﻿using ElectronicShop.Data.Model;
using ElectronicShop.Services;
using ElectronicShop.Views;
using MaterialDesignColors;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.ViewModels
{
    public class EditAndAddsProductsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }

        public List<Firm> Firms { get; set; }
        public Firm SelectedFirm { get; set; }

        public List<Category> Categories { get; set; }
        public Category SelectedCategory { get; set; }


        public string TextHead { get; set; }
        public string TextHead1 { get; set; }
        public string TextHead2 { get; set; }
        public string Article { get; set; }
        public string NameProduct { get; set; }
        public string SecondNameProduct { get; set; }
        public string Description { get; set; }
        public string CostProduct { get; set; }
        public string CountProduct { get; set; }
        public string idCategory { get; set; }
        public string CategoryName { get; set; }
        public string idFirm { get; set; }
        public string Firm1 { get; set; }
        public string CategoryNameDeep { get; set; }

        public List<Category> ProductCategory { get; set; }
        public List<Firm> ProductFirm { get; set; }
        public List<Status> ProductStatus { get; set; }
        public Category ProductCategory1 { get; set; }
        public Firm ProductFirm1 { get; set; }
        public Status ProductStatus1 { get; set; }


        public EditAndAddsProductsViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
            UpdateProduct();
        }
        private async void UpdateProduct()
        {
            var currentProduct = await _productService.GetProducts();
            var currentFirm = _productService.getFirms();
            var currentCategory = _productService.getCategories();

            Categories = currentCategory;
            Firms = currentFirm;
            Products = currentProduct;
            TextHead = "Добавление";
            TextHead1 = "Добавление";
            TextHead2 = "Добавление";

            ProductCategory = currentCategory;
            ProductFirm = currentFirm;
            ProductStatus = _productService.getStatuses();
        }

        public DelegateCommand editProduct => new(() =>
        {
            TextHead = "Редактирование";
            Article = SelectedProduct.Article;
            NameProduct = SelectedProduct.NameProduct;
            SecondNameProduct = SelectedProduct.SecondNameProduct;
            Description = SelectedProduct.Description;
            CostProduct = SelectedProduct.CostProduct.ToString();
            CountProduct = SelectedProduct.CountProduct.ToString();
            ProductCategory1 = SelectedProduct.CategoryProductNavigation;
            ProductFirm1 = SelectedProduct.FirmProductNavigation;
            ProductStatus1 = SelectedProduct.StatusNavigation;
        });

        public DelegateCommand editCategory => new(() =>
        {
            TextHead1 = "Редактирование";
            idCategory = SelectedCategory.Idcategory.ToString();
            CategoryName = SelectedCategory.CategoryName;
            CategoryNameDeep = SelectedCategory.CategoryNameDeep;

        });

        public DelegateCommand editFirm => new(() =>
        {
            TextHead2 = "Редактирование";
            idFirm = SelectedFirm.Idfirms.ToString();
            Firm1 = SelectedFirm.Firm1;
        });

        public AsyncCommand EditCommand => new(async () =>
        {
            await _productService.editProduct(Article, NameProduct, SecondNameProduct,
                Article + ".jpg", ProductFirm1, int.Parse(CostProduct),
                ProductCategory1, SelectedProduct.ReitingProduct, 
                int.Parse(CountProduct), ProductStatus1, Description);
            UpdateProduct();
            Article = "";
            NameProduct = "";
            SecondNameProduct = "";
            Description = "";
            CostProduct = "";
            CountProduct = "";
            ProductCategory1 = null;
            ProductFirm1 = null;
            ProductStatus1 = null;
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(Article)
            || string.IsNullOrWhiteSpace(NameProduct)
            || string.IsNullOrWhiteSpace(SecondNameProduct)
            || string.IsNullOrWhiteSpace(Description)
            || string.IsNullOrWhiteSpace(CostProduct)
            || string.IsNullOrWhiteSpace(CountProduct)
            || TextHead == "Добавление")
                return false;
            return true;
        });

        public AsyncCommand AddCommand => new(async() =>
        {
            await _productService.AddProduct(
                Article, NameProduct, SecondNameProduct, Article + ".jpg", ProductFirm1, int.Parse(CostProduct),
                ProductCategory1, 0,
                int.Parse(CountProduct), ProductStatus1, Description);
            UpdateProduct();
            Article = "";
            NameProduct = "";
            SecondNameProduct = "";
            Description = "";
            CostProduct = "";
            CountProduct = "";
            ProductCategory1 = null;
            ProductFirm1 = null;
            ProductStatus1 = null;
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(Article)
            || string.IsNullOrWhiteSpace(NameProduct)
            || string.IsNullOrWhiteSpace(SecondNameProduct)
            || string.IsNullOrWhiteSpace(Description)
            || string.IsNullOrWhiteSpace(CostProduct)
            || string.IsNullOrWhiteSpace(CountProduct)
            || TextHead != "Добавление")
                return false;
            return true;
        });

        public AsyncCommand EditCommand1 => new(async () =>
        {
            await _productService.editCategory(SelectedCategory.Idcategory, CategoryName, CategoryNameDeep);
            UpdateProduct();
            idCategory = "";
            CategoryNameDeep = "";
            CategoryName = "";
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(CategoryName)
                || string.IsNullOrWhiteSpace(CategoryNameDeep)
                || TextHead1 != "Редактирование")
                return false;
            return true;
        });

        public AsyncCommand AddCommand1 => new(async () =>
        {
            await _productService.AddCategory(CategoryName, CategoryNameDeep);
            UpdateProduct();
            CategoryNameDeep = "";
            CategoryName = "";
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(CategoryName)
                || string.IsNullOrWhiteSpace(CategoryNameDeep)
                || TextHead1 != "Добавление")
                return false;
            return true;
        });
        public AsyncCommand EditCommand2 => new(async () =>
        {
            await _productService.editFirm(SelectedFirm.Idfirms, Firm1);
            UpdateProduct();
            idFirm = "";
            Firm1 = "";
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(Firm1)
                || TextHead2 != "Редактирование")
                return false;
            return true;
        });

        public AsyncCommand AddCommand2 => new(async () =>
        {
            await _productService.AddFirm(Firm1);
            UpdateProduct();
            idFirm = "";
            Firm1 = "";
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(Firm1)
                || TextHead2 != "Добавление")
                return false;
            return true;
        });
    }
}

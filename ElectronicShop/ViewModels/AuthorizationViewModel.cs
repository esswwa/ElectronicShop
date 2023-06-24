using ElectronicShop.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.ViewModels
{
    public class AuthorizationViewModel : BindableBase
    {
        private readonly UserService _userService;
        private readonly PageService _pageService;
        public string Username { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorMessageButton { get; set; }

        public bool IsCheckedSaveAcc { get; set; }
        public AuthorizationViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
            IsCheckedSaveAcc = true;
        }


        public AsyncCommand Authorization => new(async () =>
        {

            await Task.Run(async () =>
            {

                if (await _userService.AuthorizationAsync(Username, Password))
                {
                    if (IsCheckedSaveAcc == true)
                        _userService.UpdateProduct();
                    await Application.Current.Dispatcher.InvokeAsync(async () => 
                    {
                        if(Settings.Default.roleId == 0)
                            _pageService.ChangePage(new MenuPage());
                        else
                            _pageService.ChangePage(new EditAndAddsProducts());
                    });
                    
                }
                else
                {
                    ErrorMessageButton = "Неверный логин или пароль";
                }
            });
        }, bool () => {

            if (string.IsNullOrWhiteSpace(Username)
                  || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Пустые поля";
                ErrorMessageButton = ErrorMessageButton != string.Empty ? string.Empty : ErrorMessageButton;
            }
            else
                ErrorMessage = string.Empty;

            return ErrorMessage == string.Empty;
        });
        public DelegateCommand Registration => new(() => _pageService.ChangePage(new RegistrationPage()));
    }
}

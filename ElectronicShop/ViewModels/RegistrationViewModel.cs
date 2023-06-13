using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShop.ViewModels
{
    public class RegistrationViewModel : BindableBase
    {

        private readonly UserService _userService;
        private readonly PageService _pageService;
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
        public string adress { get; set; }
        private List<string> _userLogin { get; set; } = new();
        private List<string> _userEmail { get; set; } = new();
        public RegistrationViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
            Task.Run(async () => _userLogin = await _userService.GetAllUser());
            Task.Run(async () => _userEmail = await _userService.GetAllEmail());
        }

        public AsyncCommand Registration => new(async () =>
        {
            int maxUser = _userService.GetMaxIdUser() + 1;
            await _userService.RegistrationAsync(maxUser, Login, Email, Password, adress);
            await _userService.BasketAsync(maxUser, maxUser);
            _pageService.ChangePage(new AuthorizationPage());
        }, bool () => {
            if (string.IsNullOrWhiteSpace(Email)
                || string.IsNullOrWhiteSpace(Login)
                || string.IsNullOrWhiteSpace(Password)
                || string.IsNullOrWhiteSpace(adress))
                ErrorMessage = "Обязательно";
            else if (Login.Length <= 4)
                ErrorMessage = "Слишком короткий логин";
            else if (_userLogin.Contains(Login))
                ErrorMessage = "Логин занят";
            else if (_userEmail.Contains(Email))
                ErrorMessage = "Почта занята";
            else
                ErrorMessage = string.Empty;

            return ErrorMessage == string.Empty;
        });

        public DelegateCommand Authorization => new(() => {

            _pageService.ChangePage(new AuthorizationPage());
        });

    }
}

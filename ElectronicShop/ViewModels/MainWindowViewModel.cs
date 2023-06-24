using ElectronicShop.Properties;

namespace ElectronicShop.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ElectronickshopContext _electronickshopContext;
        public Page PageSource { get; set; }
        public MainWindowViewModel(PageService pageService, ElectronickshopContext electronickshopContext)
        {
            _pageService = pageService;
            _pageService.onPageChanged += (page) => PageSource = page;
            _electronickshopContext = electronickshopContext;
            User user = Proverka();
            if (user == null)
                _pageService.ChangePage(new AuthorizationPage());
            else
            {
                Settings.Default.idUser = user.Iduser;
                Settings.Default.login = user.Login;
                Settings.Default.email = user.Email;
                Settings.Default.password = user.Password;
                Settings.Default.roleId = user.RoleId;
                Settings.Default.exitCheck = 1;
                Settings.Default.Adress = user.Adress;
                if(user.RoleId == 0)
                    _pageService.ChangePage(new MenuPage());
                else
                    _pageService.ChangePage(new EditAndAddsProducts());
            }
        }

        public User Proverka()
        {
            return _electronickshopContext.Users.Where(u => u.ExitCheck == 1).FirstOrDefault();
        }
    }
}

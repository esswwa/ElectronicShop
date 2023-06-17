using ElectronicShop.Properties;
using System.Linq;

namespace ElectronicShop.Services
{
    public class UserService
    {

        private readonly ElectronickshopContext _electronickshopContext;
        public UserService(ElectronickshopContext electronickshopContext)
        {
            _electronickshopContext = electronickshopContext;
        }

        public ObservableCollection<User> Users { get; set; }

        public async Task<bool> AuthorizationAsync(string username, string password)
        {
            var user = await _electronickshopContext.Users.SingleOrDefaultAsync(u => u.Login == username);
            if (user == null)
                return false;
            if (user.Password.Equals(password))
            {

                Settings.Default.idUser = user.Iduser;
                Settings.Default.login = user.Login;
                Settings.Default.password = user.Password;
                Settings.Default.email = user.Email;
                Settings.Default.roleId = user.RoleId;
                Settings.Default.Adress = user.Adress;
                Settings.Default.exitCheck = 1;
                return true;
            }
            return false;
        }


        public string checkPassword()
        {
            var user = _electronickshopContext.Users.Where(u => u.Login == "admin").SingleOrDefault();
            return user.Password;
        }

        public string checkAdress()
        {
            var user = _electronickshopContext.Users.Where(u => u.Login == "admin").SingleOrDefault();
            return user.Email;
        }

        public async Task RegistrationAsync(int idUser, string login, string email, string password, string adress)
        {

            await _electronickshopContext.Users.AddAsync(new User
            {
                Iduser = idUser,
                Login = login,
                Email = email,
                Password = password,
                RoleId = 0, 
                ExitCheck = 0,
                Adress= adress
            });
            await _electronickshopContext.SaveChangesAsync();
        }


        public async Task BasketAsync(int basket, int user)
        {

            await _electronickshopContext.Baskets.AddAsync(new Basket
            {
                Idbasket = basket,
                IdUser = user
            });
            await _electronickshopContext.SaveChangesAsync();
        }


        public int GetMaxIdUser()
        {

            return _electronickshopContext.Users.Max(u => u.Iduser);
        }

        public async Task<List<string>> GetAllUser()
        {

            return await _electronickshopContext.Users.Select(u => u.Login).AsNoTracking().ToListAsync();
        }

        public async Task<List<string>> GetAllEmail()
        {

            return await _electronickshopContext.Users.Select(u => u.Email).AsNoTracking().ToListAsync();
        }

        public async void UpdateProduct()
        {
            var currentOrders = await getUsers();
            Users = new ObservableCollection<User>(currentOrders);
            var item = Users.First(i => i.Iduser == Settings.Default.idUser);
            var index = Users.IndexOf(item);
            item.ExitCheck = Settings.Default.exitCheck;
            Users.RemoveAt(index);
            Users.Insert(index, item);
            await _electronickshopContext.SaveChangesAsync();
        }

        public async Task<List<User>> getUsers()
        {
            return _electronickshopContext.Users.ToList();
        }

        public void UpdateProductNull()
        {
            Settings.Default.exitCheck = 0;
            UpdateProduct();
        }

    }
}


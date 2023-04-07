using ElectronicShop.Data;
using ElectronicShop.Properties;

namespace ElectronicShop.Services
{
    public class UserService
    {

        private readonly ElectronickshopContext _electronickshopContext;
        public UserService(ElectronickshopContext electronickshopContext)
        {
            _electronickshopContext = electronickshopContext;
        }
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
                Settings.Default.emailOrNumberPhone = user.EmailOrNumberPhone;
                Settings.Default.roleId = user.RoleId;
                return true;
            }
            return false;
        }

        public async Task RegistrationAsync(int idUser, string login, string emailOrNumber, string password)
        {

            await _electronickshopContext.Users.AddAsync(new User
            {
                Iduser = idUser,
                Login = login,
                EmailOrNumberPhone = emailOrNumber,
                Password = password,
                RoleId = 0, 
                ExitCheck = 0
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




        //public async Task AchievementsAsync(int IduserAchievements, int Taken, int IdUser, int IdAchievements)
        //{

        //    await _electronickshopContext.UserAchievements.AddAsync(new UserAchievement
        //    {
        //        IduserAchievements = IduserAchievements,
        //        Taken = Taken,
        //        IdUser = IdUser,
        //        IdAchievements = IdAchievements
        //    });
        //    await _electronickshopContext.SaveChangesAsync();
        //}

        //public async Task StatisticsAsync(int Idstatistic, int CountOfPassedLevel, int CountTry, int ResultTest, int LanguageLvl, int Score)
        //{

        //    await _electronickshopContext.Statistics.AddAsync(new Statistic
        //    {
        //        Idstatistic = Idstatistic,
        //        CountOfPassedLevel = CountOfPassedLevel,
        //        CountTry = CountTry,
        //        ResultTest = ResultTest,
        //        LanguageLvl = LanguageLvl,
        //        Score = Score
        //    });
        //    await _electronickshopContext.SaveChangesAsync();
        //}


    }
}


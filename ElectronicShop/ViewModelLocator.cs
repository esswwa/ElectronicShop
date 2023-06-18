namespace ElectronicShop
{
    public class ViewModelLocator
    {
        private static ServiceProvider? _provider;
        private static IConfiguration? _configuration;
        public static void Init()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            _configuration = builder.Build();

            var services = new ServiceCollection();

            #region ViewModel

            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<AuthorizationViewModel>();
            services.AddTransient<MenuViewModel>();
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<BasketViewModel>();
            services.AddTransient<FavouriteViewModel>();
            services.AddTransient<OrderViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<OrderAdminViewModel>();
            services.AddTransient<EditAndAddsProductsViewModel>();

            #endregion

            #region Connection


            services.AddDbContext<ElectronickshopContext>(options =>
            {
                try
                {
                    var conn = _configuration.GetConnectionString("LocalConnection");
                    options.UseMySql(conn, ServerVersion.AutoDetect(conn));
                }
                catch (MySqlConnector.MySqlException) { }
            }, ServiceLifetime.Singleton);

            #endregion

            #region Services

            services.AddSingleton<PageService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<DocumentService>();

            #endregion

            _provider = services.BuildServiceProvider();
        }
        public MainWindowViewModel? MainWindowViewModel => _provider?.GetRequiredService<MainWindowViewModel>();
        public AuthorizationViewModel? AuthorizationViewModel => _provider?.GetRequiredService<AuthorizationViewModel>();
        public BasketViewModel? BasketViewModel => _provider?.GetRequiredService<BasketViewModel>();
        public FavouriteViewModel? FavouriteViewModel => _provider?.GetRequiredService<FavouriteViewModel>();
        public OrderViewModel? OrderViewModel => _provider?.GetRequiredService<OrderViewModel>();
        public MenuViewModel? MenuViewModel => _provider?.GetRequiredService<MenuViewModel>();
        public ProductViewModel? ProductViewModel => _provider?.GetRequiredService<ProductViewModel>();
        public RegistrationViewModel? RegistrationViewModel => _provider?.GetRequiredService<RegistrationViewModel>();
        public OrderAdminViewModel? OrderAdminViewModel => _provider?.GetRequiredService<OrderAdminViewModel>();
        public EditAndAddsProductsViewModel? EditAndAddsProductsViewModel => _provider?.GetRequiredService<EditAndAddsProductsViewModel>();

    }
    
}

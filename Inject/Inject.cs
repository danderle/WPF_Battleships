using Microsoft.Extensions.DependencyInjection;
using System;

namespace Battleships
{
    public static class Inject
    {
        #region Properties

        /// <summary>
        /// Provides the services
        /// </summary>
        public static IServiceProvider Provider { get; set; }

        /// <summary>
        /// Holds the services
        /// </summary>
        public static IServiceCollection Services { get; set; } = new ServiceCollection();

        /// <summary>
        /// Shorthand to get the applicationviewmodel
        /// </summary>
        public static ApplicationViewModel Application => Service<ApplicationViewModel>();

        #endregion

        #region Public methods

        /// <summary>
        /// Setup the DI
        /// </summary>
        public static void Setup()
        {
            AddViewModels();
            Build();
        }

        /// <summary>
        /// Retrieve a desrired service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Service<T>()
        {
            return Provider.GetService<T>();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// build the service provider
        /// </summary>
        private static void Build()
        {
            Provider = Services.BuildServiceProvider();
        }

        /// <summary>
        /// Add viewmodels to the service collection
        /// </summary>
        private static void AddViewModels()
        {
            Services.AddSingleton<ApplicationViewModel>();
            Services.AddSingleton<MainMenuViewModel>();
            Services.AddTransient<ShipPlacementViewModel>();
            Services.AddTransient<BattleViewModel>();
            Services.AddSingleton<MainWindowViewModel>();
        } 

        #endregion
    }
}

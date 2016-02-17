﻿using System.IO;
using System.Windows.Threading;
using Caliburn.Micro;
using EterManager.Services;
using EterManager.Services.Abstract;
using EterManager.Services.Concrete;
using EterManager.UserInterface.Views;
using Ninject;
using System.Windows;

namespace EterManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        private IKernel _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            SetBindings();
            InitializeMainView();
        }

        private void SetBindings()
        {
            _container = new StandardKernel();
            _container.Bind<IViewManager>().To<ViewManager>().InSingletonScope();
            _container.Bind<ILogger>().To<Logger>().InSingletonScope();
            _container.Bind<ILocale>().To<Locale>().InSingletonScope();
            _container.Bind<IEventAggregator>().To<EventAggregator>().InSingletonScope();
            _container.Bind<IDrivePointManager>().To<DrivePointManager>().InSingletonScope();
        }

        private void InitializeMainView()
        {
            Current.MainWindow = new MainView();
            Current.MainWindow.Show();
        }

        public T GetInstance<T>()
        {
            var obj = _container.Get<T>();
            return obj;
        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            string exp = e.Exception.ToString();

            if (e.Exception.InnerException != null)
                exp += e.Exception.InnerException.ToString();

            File.WriteAllText("errorLog.txt", exp);
        }
    }
}

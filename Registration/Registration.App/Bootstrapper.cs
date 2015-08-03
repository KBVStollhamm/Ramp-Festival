using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Castle.MicroKernel.Registration;
using Microsoft.Practices.Prism.Regions;
using PrismContrib.WindsorExtensions;
using Registration.Infrastructure;
using Registration.ViewModels;
using Registration.Views;

namespace Registration
{
    public class Bootstrapper : WindsorBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return this.Container.Resolve<Shell>();
        }

        protected override void ConfigureContainer()
        {
            this.Container.Register(Component.For<RegistrationViewModel>());
            this.Container.Register(Component.For<RegistrationView>());

            this.Container.Register(Component.For<AutoPopulateExportedViewsBehavior>()
                .LifestyleTransient());

            this.Container.Register(Component.For<Shell>()
                .LifestyleSingleton());

           base.ConfigureContainer();
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var factory = base.ConfigureDefaultRegionBehaviors();

            factory.AddIfMissing("AutoPopulateExportedViewsBehavior", typeof(AutoPopulateExportedViewsBehavior));

            return factory;
        }
        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = this.Shell as Window;
            Application.Current.MainWindow.Show();
        }
    }
}

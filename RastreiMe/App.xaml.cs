using Microsoft.Practices.Unity;
using Prism.Unity;
using RastreiMe.Core.Services;
using RastreiMe.Views;
using Xamarin.Forms;

namespace RastreiMe
{
  public partial class App : PrismApplication
  {
    public App() : this(null) { }

    public App(IPlatformInitializer initializer = null) : base(initializer) { InitializeComponent(); }

    protected override void OnInitialized()
    {
      InitializeComponent();

      NavigationService.NavigateAsync("NavigationPage/Main");
    }

    protected override void RegisterTypes()
    {
      RegisterNavigation();
      RegisterServices();
    }

    void RegisterNavigation()
    {
      Container.RegisterTypeForNavigation<NavigationPage>();
      Container.RegisterTypeForNavigation<Main>();
    }

    void RegisterServices()
    {
      Container.RegisterInstance(typeof(RastreiMeService), "RastreiMeService", new RastreiMeService());
    }
  }
}
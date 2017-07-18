using System;
using Prism.Common;
using Prism.Mvvm;
using Prism.Navigation;
using RastreiMe.Core.Services;
using Xamarin.Forms;

namespace RastreiMe.ViewModels
{
  public class MainViewModel : BindableBase, IPageAware
  {
		public MainViewModel(INavigationService navigationService, RastreiMeService service)
		{
			_navigationService = navigationService;
			_service = service;

      this.Email = "teste@gmail.com";
		}

		private INavigationService _navigationService;
		private RastreiMeService _service;
    
    public Page Page { get; set; }

    private string _email;
    public string Email
    {
      get { return (_email); }
      set { SetProperty(ref _email, value);}
    }
	}
}
using GalaSoft.MvvmLight.Ioc;
using MSC.BingoBuzz.Interfaces;
using MSC.BingoBuzz.Xam.Controls;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSC.BingoBuzz.Xam.Services
{
    // Based on: https://mallibone.com/post/xamarin.forms-navigation-with-mvvm-light
    public class NavigationService : INavigationService
    {
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();

        public event PropertyChangedEventHandler CanGoBackChanged;

        public bool CanGoBack
        {
            get
            {
                return XamarinFormsNav.NavigationStack != null && XamarinFormsNav.NavigationStack.Count > 0;
            }
        }

        public NavigationPage NavigationPage { get; set; }

        public INavigation XamarinFormsNav { get; set; }

        public Page GetCurrentView()
        {
            if (XamarinFormsNav.NavigationStack.Any())
            {
                var currentView = XamarinFormsNav.NavigationStack[XamarinFormsNav.NavigationStack.Count - 1];
                return currentView;
            }
            return null;
        }

        public Page GetLastView()
        {
            if (XamarinFormsNav.NavigationStack.Any())
            {
                var lastView = XamarinFormsNav.NavigationStack[XamarinFormsNav.NavigationStack.Count - 2];
                return lastView;
            }
            return null;
        }

        public CustomViewModelBase GetViewModel(Type viewModelType)
        {
            CustomViewModelBase vm = ((App)Application.Current).Kernel.GetService(viewModelType) as CustomViewModelBase;
            return vm;
        }

        public async Task GoBack()
        {
            if (CanGoBack)
            {
                ContentPage pageToRemove = (ContentPage)GetCurrentView();
                if (pageToRemove is IContentPage)
                {
                    Debug.WriteLine($"Prepare For Dispose {pageToRemove.GetType().ToString()}");
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ((CustomViewModelBase)pageToRemove.BindingContext).Cleanup(); //clean up the VM - this is a singleton
                        ((IContentPage)pageToRemove).PrepareForDispose(); //clean up the page
                    });
                    Debug.WriteLine("Running CG from NavService");
                    GC.Collect();
                }

                await XamarinFormsNav.PopAsync(true);
            }

            OnCanGoBackChanged();
        }

        public async Task NavigateTo<TVM>()
            where TVM : IViewModelBase
        {
            await PushActivityIndicatorTransparentPopupAsync();
            await NavigateToView(typeof(TVM));

            if (XamarinFormsNav.NavigationStack.Last().BindingContext is IViewModelBase)
            {
                await ((IViewModelBase)(XamarinFormsNav.NavigationStack.Last().BindingContext)).Init();
            }

            PrintOutNavStack();
            await PopActivityIndicatorTransparentPopupsAsync();
        }

        public async Task NavigateTo<TVM, TParameter>(TParameter parameter)
            where TVM : IViewModelBaseWithParam<TParameter>
        {
            await PushActivityIndicatorTransparentPopupAsync();
            await NavigateToView(typeof(TVM));

            if (XamarinFormsNav.NavigationStack.Last().BindingContext is IViewModelBaseWithParam<TParameter>)
            {
                await ((IViewModelBaseWithParam<TParameter>)(XamarinFormsNav.NavigationStack.Last().BindingContext)).Init(parameter);
            }

            PrintOutNavStack();
            await PopActivityIndicatorTransparentPopupsAsync();
        }

        public async Task NavigateToNoAnimation<TVM, TParameter>(TParameter parameter)
           where TVM : IViewModelBaseWithParam<TParameter>
        {
            await NavigateToView(typeof(TVM));

            if (XamarinFormsNav.NavigationStack.Last().BindingContext is IViewModelBaseWithParam<TParameter>)
            {
                await ((IViewModelBaseWithParam<TParameter>)(XamarinFormsNav.NavigationStack.Last().BindingContext)).Init(parameter);
            }

            PrintOutNavStack();
        }

        public async Task NavigateToNoAnimation<TVM>()
                           where TVM : IViewModelBase
        {
            await NavigateToView(typeof(TVM));

            if (XamarinFormsNav.NavigationStack.Last().BindingContext is IViewModelBase)
            {
                await ((IViewModelBase)(XamarinFormsNav.NavigationStack.Last().BindingContext)).Init();
            }

            PrintOutNavStack();
        }

        public async Task NavigateToUri(Uri uri)
        {
            if (uri == null)
                throw new ArgumentException("Invalid URI");

            Device.OpenUri(uri);
        }

        public async Task PopActivityIndicatorTransparentPopupsAsync()
        {
            //we are doing this for the situations in which an error message is popped when the page init is loading
            var removeThese = PopupNavigation.PopupStack.Where(x => x.GetType() == typeof(ActivityIndicatorTransparentPopup)).ToList();
            foreach (var r in removeThese)
            {
                await PopupNavigation.RemovePageAsync(r, true);
            }
        }

        public async Task PopPopupAsync()
        {
            if (PopupNavigation.PopupStack.Count > 0)
            {
                await PopupNavigation.PopAsync();
            }
        }

        public async Task PopToRoot()
        {
            await XamarinFormsNav.PopToRootAsync();
            PrintOutNavStack();
        }

        public void PrintOutNavStack()
        {
            Debug.WriteLine("**********************************************");
            foreach (var p in XamarinFormsNav.NavigationStack)
            {
                Debug.WriteLine(p.GetType().ToString());
            }
        }

        public async Task PushActivityIndicatorTransparentPopupAsync()
        {
            await PopupNavigation.PushAsync(new ActivityIndicatorTransparentPopup());
        }

        public void RegisterViewMapping(Type viewModel, Type view)
        {
            _map.Add(viewModel, view);
        }

        public void RemoveDuplicatePageByType(Type pageType)
        {
            try
            {
                if (XamarinFormsNav.NavigationStack.Any())
                {
                    var views = XamarinFormsNav.NavigationStack.Where(x => x.GetType() == pageType).ToList();
                    if (views.Count > 1)
                    {
                        for (int i = 0; i == views.Count - 1; i++)
                        {
                            //we never want to remove the last one
                            if (XamarinFormsNav.NavigationStack.Count > 1)
                            {
                                //so, iOS will not remove the current page from the back stack.
                                XamarinFormsNav.RemovePage(views[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: something elegant here - log it somewhere
                Debug.WriteLine($"Error: NavService.RemovePageByType({pageType.ToString()}: {ex.Message})");
            }
        }

        public void RemoveLastView()
        {
            if (XamarinFormsNav.NavigationStack.Any())
            {
                var lastView = XamarinFormsNav.NavigationStack[XamarinFormsNav.NavigationStack.Count - 2];
                XamarinFormsNav.RemovePage(lastView);
            }
        }

        public void RemovePageByType(Type pageType)
        {
            try
            {
                if (XamarinFormsNav.NavigationStack.Any())
                {
                    var views = XamarinFormsNav.NavigationStack.Where(x => x.GetType() == pageType).ToList();
                    foreach (var v in views)
                    {
                        //we never want to remove the last one
                        if (XamarinFormsNav.NavigationStack.Count > 1)
                        {
                            Debug.WriteLine($"Removing {v.GetType().ToString()}");
                            //so, iOS will not remove the current page from the back stack.
                            XamarinFormsNav.RemovePage(v);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: something elegant here - log it somewhere
                Debug.WriteLine($"Error: NavService.RemovePageByType({pageType.ToString()}: {ex.Message})");
            }
        }

        public void SetNavigationPageAsApplicationMainPage(Page root)
        {
            NavigationPage = new NavigationPage(root);
            this.XamarinFormsNav = NavigationPage.Navigation;
            //NavigationPage.BarBackgroundColor = (Color)App.Current.Resources["DarkGray"];
            //NavigationPage.BarTextColor = (Color)App.Current.Resources["White"];
            //NavigationPage.Title = "AppTitle";
            NavigationPage.SetHasNavigationBar(NavigationPage, false);
            Application.Current.MainPage = NavigationPage;
        }

        public async Task StartNavStack(Type pageType)
        {
            await PushActivityIndicatorTransparentPopupAsync();
            Type viewType;

            var viewModelType = pageType;

            if (!_map.TryGetValue(viewModelType, out viewType))
                throw new ArgumentException($"No view found in View Mapping for {viewModelType.FullName}.");

            var constructor = viewType.GetTypeInfo()
                .DeclaredConstructors
                .FirstOrDefault(dc => dc.GetParameters().Count() <= 0);
            var view = constructor.Invoke(null) as Page;

            NavigationPage.SetBackButtonTitle(view, "");
            NavigationPage.SetHasBackButton(view, false);
            NavigationPage.SetHasNavigationBar(view, false);
            NavigationPage.BarBackgroundColor = (Color)Application.Current.Resources["PI.Gray"];

            var vm = ((App)Application.Current).Kernel.GetService(viewModelType);
            ((CustomViewModelBase)vm).IsBusy = true;

            view.BindingContext = vm;

            //get the navstack going again
            SetNavigationPageAsApplicationMainPage(view);

            if (XamarinFormsNav.NavigationStack.Last().BindingContext is IViewModelBase)
            {
                await ((IViewModelBase)(XamarinFormsNav.NavigationStack.Last().BindingContext)).Init();
            }

            PrintOutNavStack();
            await PopActivityIndicatorTransparentPopupsAsync();
        }

        private async Task NavigateToView(Type viewModelType)
        {
            if (!_map.TryGetValue(viewModelType, out Type viewType))
                throw new ArgumentException($"No view found in View Mapping for {viewModelType.FullName}.");

            var constructor = viewType.GetTypeInfo()
                .DeclaredConstructors
                .FirstOrDefault(dc => dc.GetParameters().Count() <= 0);
            var view = constructor.Invoke(null) as Page;

            Xamarin.Forms.NavigationPage.SetHasNavigationBar(view, false);

            var vm = ((App)Application.Current).Kernel.GetService(viewModelType);
            ((CustomViewModelBase)vm).IsBusy = true;

            view.BindingContext = vm;

            await XamarinFormsNav.PushAsync(view, true);
        }

        private void OnCanGoBackChanged()
        {
            CanGoBackChanged?.Invoke(this, new
                    PropertyChangedEventArgs("CanGoBack"));
        }
    }
}
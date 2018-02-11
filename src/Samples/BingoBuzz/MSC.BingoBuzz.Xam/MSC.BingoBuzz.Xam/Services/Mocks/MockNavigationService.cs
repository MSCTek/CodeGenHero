using MSC.BingoBuzz.Interfaces;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSC.BingoBuzz.Xam.Services.Mocks
{
    public class MockNavigationService : INavigationService
    {
        public event PropertyChangedEventHandler CanGoBackChanged;

        public bool CanGoBack => throw new NotImplementedException();

        public Page GetCurrentView()
        {
            throw new NotImplementedException();
        }

        public Page GetLastView()
        {
            throw new NotImplementedException();
        }

        public CustomViewModelBase GetViewModel(Type viewModelType)
        {
            throw new NotImplementedException();
        }

        public Task GoBack()
        {
            throw new NotImplementedException();
        }

        public Task NavigateTo<TVM>() where TVM : IViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateTo<TVM, TParameter>(TParameter parameter) where TVM : IViewModelBaseWithParam<TParameter>
        {
            throw new NotImplementedException();
        }

        public Task NavigateToNoAnimation<TVM>() where TVM : IViewModelBase
        {
            throw new NotImplementedException();
        }

        public Task NavigateToNoAnimation<TVM, TParameter>(TParameter parameter) where TVM : IViewModelBaseWithParam<TParameter>
        {
            throw new NotImplementedException();
        }

        public Task NavigateToUri(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task PopActivityIndicatorTransparentPopupsAsync()
        {
            throw new NotImplementedException();
        }

        public Task PopPopupAsync()
        {
            throw new NotImplementedException();
        }

        public Task PopToRoot()
        {
            throw new NotImplementedException();
        }

        public Task PushActivityIndicatorTransparentPopupAsync()
        {
            throw new NotImplementedException();
        }

        public void RemoveDuplicatePageByType(Type pageType)
        {
            throw new NotImplementedException();
        }

        public void RemoveLastView()
        {
            throw new NotImplementedException();
        }

        public void RemovePageByType(Type pageType)
        {
            throw new NotImplementedException();
        }

        public Task StartNavStack(Type pageType)
        {
            throw new NotImplementedException();
        }
    }
}
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSC.BingoBuzz.Constants;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Interfaces;

namespace MSC.BingoBuzz.Xam.ViewModels
{
    public abstract class CustomViewModelBase<TParameter> : CustomViewModelBase, IViewModelBaseWithParam<TParameter>
    {
        //https://developer.xamarin.com/api/type/System.IDisposable/
        //http://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface

        public CustomViewModelBase(INavigationService navService, IDataRetrievalService dataRetrievalService, IStateService stateService)
        {
            if (navService == null)
                throw new ArgumentException("Invalid navService");

            if (dataRetrievalService == null)
                throw new ArgumentException("Invalid dataRetrievalService");

            if (stateService == null)
                throw new ArgumentException("Invalid stateService");

            NavService = navService;
            DataRetrievalService = dataRetrievalService;
            StateService = stateService;
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override async Task Init()
        {
            await Init(default(TParameter));
        }

        public abstract Task Init(TParameter parameter);
    }

    public abstract class CustomViewModelBase : ViewModelBase, IViewModelBase, IDisposable
    {
        private double _currentViewPortHeight;
        private double _currentViewPortWidth;
        private bool _disposed;
        private bool _isBusy;
        private bool _isDev;

        public CustomViewModelBase(INavigationService navService, IDataRetrievalService dataRetrievalService, IStateService stateService)
        {
            if (navService == null)
                throw new ArgumentException("Invalid navService");

            if (dataRetrievalService == null)
                throw new ArgumentException("Invalid dataRetrievalService");

            if (stateService == null)
                throw new ArgumentException("Invalid stateService");

            NavService = navService;
            DataRetrievalService = dataRetrievalService;
            StateService = stateService;

            IsDev = false;
#if DEBUG
            IsDev = true;
#endif
        }

        protected CustomViewModelBase()
        {
        }

        //finalizers are expensive and should only used in debug mode... says Xamarin.
        ~CustomViewModelBase()
        {
            Debug.WriteLine("MMSViewModelBase Finalizer Running");
            Dispose(false);
        }

        public RelayCommand BackCommand { get { return new RelayCommand(async () => await NavService.GoBack()); } }

        public bool IsDev
        {
            get { return _isDev; }
            set { Set(ref _isDev, value); }
        }

        public double CurrentViewPortHeight
        {
            get { return _currentViewPortHeight; }
            set
            {
                _currentViewPortHeight = value;
                RaisePropertyChanged();
            }
        }

        /*public async Task CheckAppCenter()
        {
            if (DataService.DoIHaveInternet())
            {
                try
                {
                    Log.Debug($"Analytics are Enabled? {await Analytics.IsEnabledAsync()}", Enums.LogMessageType.Info_Diagnostics);
                    Log.Debug($"Crash Reporting is Enabled? {await Crashes.IsEnabledAsync()}", Enums.LogMessageType.Info_Diagnostics);
                    Log.Debug($"Distribution Notices are Enabled? {await Distribute.IsEnabledAsync()}", Enums.LogMessageType.Info_Diagnostics);
                    Log.Debug($"Push Notifications are Enabled? {await Push.IsEnabledAsync()}", Enums.LogMessageType.Info_Diagnostics);
                }
                catch (Exception ex)
                {
                    Log.Error("App Center enable check is Failing!", Enums.LogMessageType.Exception_Application, ex: ex);
                }
            }
        }*/

        public double CurrentViewPortWidth
        {
            get { return _currentViewPortWidth; }
            set
            {
                _currentViewPortWidth = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
                OnIsBusyChanged();
            }
        }

        protected static INavigationService NavService { get; set; }
        protected IStateService StateService { get; set; }
        protected IDataRetrievalService DataRetrievalService { get; set; }

        public override void Cleanup()
        {
            base.Cleanup();
        }

        public virtual void Dispose()
        {
            Dispose(true);
            //Debug.WriteLine("Running CG from MMSViewModelBase");
            //GC.Collect();
            //GC.SuppressFinalize(this);
        }

        public abstract Task Init();

        public void SetViewPort(double width, double height)
        {
            // Set ViewPort height and width
            if (CurrentViewPortWidth != width || CurrentViewPortHeight != height)
            {
                CurrentViewPortWidth = width;
                CurrentViewPortHeight = height;
                // Set ViewPort Background Image
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        protected virtual void OnIsBusyChanged()
        {
        }
    }
}
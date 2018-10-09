using CodeGenHero.BingoBuzz.Interfaces;
using CodeGenHero.BingoBuzz.Xam.Interfaces;
using CodeGenHero.Logging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Xam.ViewModels
{
    public abstract class CustomViewModelBase<TParameter> : CustomViewModelBase, IViewModelBaseWithParam<TParameter>
    {
        //https://developer.xamarin.com/api/type/System.IDisposable/
        //http://stackoverflow.com/questions/538060/proper-use-of-the-idisposable-interface

        public CustomViewModelBase(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IStateService stateService, ILoggingService loggingService, IMemoryReporterService memoryReporterService)
        {
            if (navService == null)
                throw new ArgumentException("Invalid navService");

            if (dataRetrievalService == null)
                throw new ArgumentException("Invalid dataRetrievalService");

            if (dataDownloadService == null)
                throw new ArgumentException("Invalid dataDownloadService");

            if (stateService == null)
                throw new ArgumentException("Invalid stateService");

            if (loggingService == null)
                throw new ArgumentException("Invalid loggingService");

            if (memoryReporterService == null)
                throw new ArgumentException("Invalid memoryReporterService");

            NavService = navService;
            DataRetrievalService = dataRetrievalService;
            DataDownloadService = dataDownloadService;
            StateService = stateService;
            LoggingService = loggingService;
            MemoryReporterService = memoryReporterService;

            IsDev = false;
#if DEBUG
            IsDev = true;
#endif
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

        public CustomViewModelBase(INavigationService navService, IDataRetrievalService dataRetrievalService, IDataDownloadService dataDownloadService, IStateService stateService, ILoggingService loggingService, IMemoryReporterService memoryReporterService)
        {
            if (navService == null)
                throw new ArgumentException("Invalid navService");

            if (dataRetrievalService == null)
                throw new ArgumentException("Invalid dataRetrievalService");

            if (dataDownloadService == null)
                throw new ArgumentException("Invalid dataDownloadService");

            if (stateService == null)
                throw new ArgumentException("Invalid stateService");

            if (loggingService == null)
                throw new ArgumentException("Invalid loggingService");

            if (memoryReporterService == null)
                throw new ArgumentException("Invalid memoryReporterService");

            NavService = navService;
            DataRetrievalService = dataRetrievalService;
            DataDownloadService = dataDownloadService;
            StateService = stateService;
            LoggingService = loggingService;
            MemoryReporterService = memoryReporterService;

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

        public void CheckMemory()
        {
            try
            {
                LoggingService.Info($"Memory In Use: {MemoryReporterService.GetMemoryInUse()} " +
                    $"- Usage Limit: {MemoryReporterService.GetUsageLimit()} " +
                    $"- Last Change: {MemoryReporterService.GetLastChange()} " +
                    $"- Is Usage Increasing: {MemoryReporterService.IsIncreasing()}", 
                    LogMessageType.Instance.Info_Diagnostics);
            }
            catch (Exception ex)
            {
                LoggingService.Error($"MemoryReporterServiceError: {ex.Message}"
                    , LogMessageType.Instance.Exception_General
                    , ex: ex);
            }
        }

        public async Task CheckAppCenter()
        {
            if (Helpers.DoIHaveInternet)
            {
                try
                {
                    LoggingService.Debug($"Analytics are Enabled? {await Analytics.IsEnabledAsync()}", LogMessageType.Instance.Info_Diagnostics);
                    LoggingService.Debug($"Crash Reporting is Enabled? {await Crashes.IsEnabledAsync()}", LogMessageType.Instance.Info_Diagnostics);
                    //LoggingService.Debug($"Distribution Notices are Enabled? {await Distribute.IsEnabledAsync()}", LogMessageType.Instance.Info_Diagnostics);
                    //LoggingService.Debug($"Push Notifications are Enabled? {await Push.IsEnabledAsync()}", LogMessageType.Instance.Info_Diagnostics);
                }
                catch (Exception ex)
                {
                    LoggingService.Error("App Center enable check is Failing!", LogMessageType.Instance.Exception_Application, ex: ex);
                }
            }
        }

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
        protected IDataDownloadService DataDownloadService { get; set; }
        protected ILoggingService LoggingService { get; set; }
        protected IMemoryReporterService MemoryReporterService { get; set; }

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
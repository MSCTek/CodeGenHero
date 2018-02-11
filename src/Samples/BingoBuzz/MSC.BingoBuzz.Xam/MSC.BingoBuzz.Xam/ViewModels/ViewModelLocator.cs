using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using MSC.BingoBuzz.Xam.Interfaces;
using MSC.BingoBuzz.Xam.Services;
using MSC.BingoBuzz.Xam.Services.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<WelcomeViewModel>();
        }

        public WelcomeViewModel Welcome
        {
            get { return SimpleIoc.Default.GetInstance<WelcomeViewModel>(); }
        }
    }
}
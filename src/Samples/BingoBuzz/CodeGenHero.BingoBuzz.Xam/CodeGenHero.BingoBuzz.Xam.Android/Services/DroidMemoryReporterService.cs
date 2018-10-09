using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CodeGenHero.BingoBuzz.Xam.Interfaces;

namespace CodeGenHero.BingoBuzz.Xam.Droid.Services
{
    public class DroidMemoryReporterService : IMemoryReporterService
    {
        private double currentValue;
        private double lastValue;

        public double GetLastChange()
        {
            return currentValue - lastValue;
        }

        public double GetMemoryInUse()
        {
            long totalMemory = Java.Lang.Runtime.GetRuntime().TotalMemory();
            long freeMemory = Java.Lang.Runtime.GetRuntime().FreeMemory();
            lastValue = currentValue;
            currentValue = (double)(totalMemory - freeMemory);
            return currentValue;
        }

        public double GetUsageLimit()
        {
            return Java.Lang.Runtime.GetRuntime().TotalMemory();
        }

        public bool IsIncreasing()
        {
            return currentValue - lastValue > 0 ? true : false;
        }
    }
}
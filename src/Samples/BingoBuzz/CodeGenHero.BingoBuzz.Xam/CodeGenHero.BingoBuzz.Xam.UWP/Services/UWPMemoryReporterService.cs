using CodeGenHero.BingoBuzz.Xam.Interfaces;
using Windows.System;

namespace CodeGenHero.BingoBuzz.Xam.UWP.Services
{
    public class UWPMemoryReporterService : IMemoryReporterService
    {
        private double currentValue;
        private double lastValue;

        public double GetLastChange()
        {
            return currentValue - lastValue;
        }

        public double GetMemoryInUse()
        {
            lastValue = currentValue;
            currentValue = (double)MemoryManager.AppMemoryUsage;
            return currentValue;
        }

        public double GetUsageLimit()
        {
            return (double)MemoryManager.AppMemoryUsageLimit;
        }

        public bool IsIncreasing()
        {
            return currentValue - lastValue > 0 ? true : false;
        }

    }
}

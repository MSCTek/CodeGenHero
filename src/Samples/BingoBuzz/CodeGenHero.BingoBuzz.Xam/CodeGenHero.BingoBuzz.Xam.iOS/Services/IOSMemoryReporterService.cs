using CodeGenHero.BingoBuzz.Xam.Interfaces;
using Foundation;

namespace CodeGenHero.BingoBuzz.Xam.iOS.Services
{
    public class IOSMemoryReporterService : IMemoryReporterService
    {
            public double GetLastChange()
            {
                return 0D;
            }

            public double GetMemoryInUse()
            {
                return 0D;
            }

            public double GetUsageLimit()
            {
                return (double)NSProcessInfo.ProcessInfo.PhysicalMemory;
            }

            public bool IsIncreasing()
            {
                return false;
            }
        }
    }
namespace CodeGenHero.BingoBuzz.Xam.Interfaces
{
    public interface IMemoryReporterService
    {
        double GetLastChange();
        double GetMemoryInUse();
        double GetUsageLimit();
        bool IsIncreasing();
    }
}

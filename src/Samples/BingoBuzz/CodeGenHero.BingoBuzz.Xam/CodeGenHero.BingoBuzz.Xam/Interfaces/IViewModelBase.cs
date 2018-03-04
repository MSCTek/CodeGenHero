using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenHero.BingoBuzz.Interfaces
{
    public interface IViewModelBase
    {
        bool IsBusy { get; set; }

        Task Init();
    }

    public interface IViewModelBaseWithParam<TParameter> : IViewModelBase
    {
        Task Init(TParameter parameter);
    }
}
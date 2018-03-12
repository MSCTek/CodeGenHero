using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using CodeGenHero.BingoBuzz.Xam.ModelObj.BB;

namespace CodeGenHero.BingoBuzz.Xam.Interfaces
{
    public interface IDataDownloadService
    {
        Task InsertAllDataCleanLocalDB();
    }
}
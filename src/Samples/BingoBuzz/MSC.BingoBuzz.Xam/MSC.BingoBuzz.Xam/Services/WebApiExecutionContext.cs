using CodeGenHero.EAMVCXamPOCO.DataService.Constants;
using CodeGenHero.EAMVCXamPOCO.DataService.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSC.BingoBuzz.Xam.Services
{
    public class WebApiExecutionContext : IWebApiExecutionContext
    {
        public string BaseFileUrl { get; set; }

        public string BaseWebApiUrl { get; set; }

        public string ConnectionIdentifier { get; set; }

        public Enums.WebApiExecutionContextType ExecutionContextType { get; set; }
    }
}
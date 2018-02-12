using CodeGenHero.EAMVCXamPOCO.DataService.Interface;
using Newtonsoft.Json;
using static CodeGenHero.EAMVCXamPOCO.DataService.Constants.Enums;

namespace CodeGenHero.EAMVCXamPOCO.DataService
{
	public class WebApiExecutionContext : IWebApiExecutionContext
    {
        private string _baseFileUrl;
        private string _baseWebApiUrl;
        private string _connectionIdentifier;

        public WebApiExecutionContext(WebApiExecutionContextType executionContextType,
            string baseWebApiUrl, string baseFileUrl, string connectionIdentifier)
        {
            ExecutionContextType = executionContextType;
            BaseWebApiUrl = baseWebApiUrl;
            BaseFileUrl = baseFileUrl;
            ConnectionIdentifier = connectionIdentifier;
        }

        protected WebApiExecutionContext()
        {
        }

        [JsonProperty]
        public string BaseFileUrl
        {
            get { return _baseFileUrl; }
            private set
            {
                _baseFileUrl = value;
                if (!string.IsNullOrEmpty(_baseFileUrl))
                {
                    if (!_baseFileUrl.EndsWith("/"))
                    {
                        _baseFileUrl = $"{_baseFileUrl}/";
                    }
                }
            }
        }

        [JsonProperty]
        public string BaseWebApiUrl
        {
            get { return _baseWebApiUrl; }
            private set
            {
                _baseWebApiUrl = value;
                if (!string.IsNullOrEmpty(_baseWebApiUrl))
                {
                    if (!_baseWebApiUrl.EndsWith("/"))
                    {
                        _baseWebApiUrl = $"{_baseWebApiUrl}/";
                    }
                }
            }
        }

        [JsonProperty]
        public string ConnectionIdentifier
        {
            get { return _connectionIdentifier; }
            private set
            {
                _connectionIdentifier = value;
            }
        }

        [JsonProperty]
        public WebApiExecutionContextType ExecutionContextType { get; set; }
    }
}
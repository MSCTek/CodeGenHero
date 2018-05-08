//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Controllers;
//using System.Web.Http.Dispatcher;
//using System.Web.Http.Routing;

//namespace CodeGenHero.WebApi
//{
//    // Originally created for Umbraco https://github.com/umbraco/Umbraco-CMS/blob/7.2.0/src/Umbraco.Web/WebApi/NamespaceHttpControllerSelector.cs
//    //  adapted from there, does not recreate HttpControllerDescriptors, instead caches them
//    // Another good reference: https://blogs.msdn.microsoft.com/webdev/2013/03/07/asp-net-web-api-using-namespaces-to-version-web-apis/
//    // https://github.com/WebApiContrib/WebAPIContrib/pull/111/files
//    // https://blogs.msdn.microsoft.com/webdev/2014/08/22/customizing-web-api-controller-selector/
//    // http://aspnet.codeplex.com/SourceControl/latest#Samples/WebApi/NamespaceControllerSelector/NamespaceHttpControllerSelector.cs
//    public class NamespaceHttpControllerSelector : DefaultHttpControllerSelector
//    {
//        private const string ControllerKey = "controller";
//        private const string NamespaceKey = "namespaceName";
//        private readonly HttpConfiguration _configuration;
//        private readonly Lazy<HashSet<NamespacedHttpControllerMetadata>> _duplicateControllerTypes;

//        public NamespaceHttpControllerSelector(HttpConfiguration configuration)
//            : base(configuration)
//        {
//            _configuration = configuration;
//            _duplicateControllerTypes = new Lazy<HashSet<NamespacedHttpControllerMetadata>>(InitializeNamespacedHttpControllerMetadata);
//        }

//        private static T GetRouteVariable<T>(IHttpRouteData routeData, string name)
//        {
//            object result = null;
//            if (routeData.Values.TryGetValue(name, out result))
//            {
//                return (T)result;
//            }
//            return default(T);
//        }

//        //[System.Diagnostics.DebuggerStepThrough]
//        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
//        {
//            var routeData = request.GetRouteData();
//            if (routeData == null || routeData.Route == null)
//                return base.SelectController(request);

//            // Look up controller in route data
//            string controllerName = GetRouteVariable<string>(routeData, ControllerKey);
//            if (string.IsNullOrEmpty(controllerName))
//                return base.SelectController(request);

//            //get the currently cached default controllers - this will not contain duplicate controllers found so if
//            // this controller is found in the underlying cache we don't need to do anything
//            var map = base.GetControllerMapping();
//            if (map.ContainsKey(controllerName))
//                return base.SelectController(request);

//            //the cache does not contain this controller because it's most likely a duplicate,
//            // so we need to sort this out ourselves and we can only do that if the namespace token
//            // is formatted correctly.
//            //var namespaces = routeData.Route.DataTokens[NamespaceKey] as IEnumerable<string>;
//            //if (namespaces == null)
//            //    return base.SelectController(request);

//            ////see if this is in our cache
//            //var found = _duplicateControllerTypes.Value.FirstOrDefault(x => string.Equals(x.ControllerName, controllerNameAsString, StringComparison.OrdinalIgnoreCase) && namespaces.Contains(x.ControllerNamespace));

//            // Replaced the above code per: stackoverflow.com/questions/32778955/asp-net-webform-web-api-version-handling-using-namespace-and-persist-old-api
//            //var @namespace = routeData.Values[NamespaceKey] as string;

//            var namespaceName = GetRouteVariable<string>(routeData, NamespaceKey);
//            if (string.IsNullOrEmpty(namespaceName))  // routeData.Route.DataTokens[NamespaceKey] == null)
//                return base.SelectController(request);

//            //see if this is in our cache - try both without the ControllerSuffix and with it.
//            var found = _duplicateControllerTypes.Value
//              .Where(x =>
//              string.Equals(x.ControllerName, controllerName, StringComparison.OrdinalIgnoreCase)
//              ||
//              string.Equals(x.ControllerName, controllerName + "List", StringComparison.OrdinalIgnoreCase)
//              ||
//              string.Equals(x.ControllerName, controllerName + ControllerSuffix, StringComparison.OrdinalIgnoreCase))
//              .FirstOrDefault(x => x.ControllerNamespace == namespaceName);

//            if (found == null)
//                return base.SelectController(request);

//            return found.Descriptor;
//        }

//        private HashSet<NamespacedHttpControllerMetadata> InitializeNamespacedHttpControllerMetadata()
//        {
//            var assembliesResolver = _configuration.Services.GetAssembliesResolver();
//            var controllersResolver = _configuration.Services.GetHttpControllerTypeResolver();
//            var controllerTypes = controllersResolver.GetControllerTypes(assembliesResolver);

//            var groupedByName = controllerTypes.GroupBy(
//                t => t.Name.Substring(0, t.Name.Length - ControllerSuffix.Length),
//                StringComparer.OrdinalIgnoreCase).Where(x => x.Count() > 1);

//            var duplicateControllers = groupedByName.ToDictionary(
//                g => g.Key,
//                g => g.ToLookup(t => t.Namespace ?? String.Empty, StringComparer.OrdinalIgnoreCase),
//                StringComparer.OrdinalIgnoreCase);

//            var result = new HashSet<NamespacedHttpControllerMetadata>();

//            foreach (var controllerTypeGroup in duplicateControllers)
//            {
//                foreach (var controllerType in controllerTypeGroup.Value.SelectMany(controllerTypesGrouping => controllerTypesGrouping))
//                {
//                    result.Add(new NamespacedHttpControllerMetadata(controllerTypeGroup.Key, controllerType.Namespace,
//                        new HttpControllerDescriptor(_configuration, controllerTypeGroup.Key, controllerType)));
//                }
//            }

//            return result;
//        }

//        private class NamespacedHttpControllerMetadata
//        {
//            private readonly string _controllerName;
//            private readonly string _controllerNamespace;
//            private readonly HttpControllerDescriptor _descriptor;

//            public NamespacedHttpControllerMetadata(string controllerName, string controllerNamespace, HttpControllerDescriptor descriptor)
//            {
//                _controllerName = controllerName;
//                _controllerNamespace = controllerNamespace;
//                _descriptor = descriptor;
//            }

//            public string ControllerName
//            {
//                get { return _controllerName; }
//            }

//            public string ControllerNamespace
//            {
//                get { return _controllerNamespace; }
//            }

//            public HttpControllerDescriptor Descriptor
//            {
//                get { return _descriptor; }
//            }
//        }
//    }
//}
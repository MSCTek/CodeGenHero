using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http.Routing;

namespace CodeGenHero.EAMVCXamPOCO.API.Helpers
{
    /// <summary>
    /// A Constraint implementation that matches an HTTP header against an expected version value.  Matches
    /// both custom request header ("api-version") and custom content type vnd.myservice.vX+json (or other dt type)
    /// </summary>
    internal class VersionConstraint : IHttpRouteConstraint
    {
        public const string VersionHeaderName = "api-version";
        //private const int DefaultVersion = 1;

        public VersionConstraint(int allowedVersion, int? defaultVersion = null)
        {
            AllowedVersion = allowedVersion;
            DefaultVersion = defaultVersion;
        }

        public int AllowedVersion
        {
            get;
            private set;
        }

        public int? DefaultVersion
        {
            get;
            private set;
        }

        public bool Match(HttpRequestMessage request, IHttpRoute route,
            string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (routeDirection == HttpRouteDirection.UriResolution)
            {
                // try custom request header "api-version"
                int? version = GetVersionHeaderOrQuery(request);

                // not found?  Try custom content type in accept header
                if (version == null)
                {
                    version = GetVersionFromCustomContentType(request);
                }

                // could simply permit default version here, but for now we want to ensure clients are making requests for a particular version #.
                //return ((version ?? DefaultVersion) == AllowedVersion);

                if (!version.HasValue && DefaultVersion.HasValue)
                {
                    version = DefaultVersion.Value;
                }

                if (version.HasValue && version.Value == AllowedVersion)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private int? GetVersionFromCustomContentType(HttpRequestMessage request)
        {
            string versionAsString = null;

            // get the accept header.

            var mediaTypes = request.Headers.Accept.Select(h => h.MediaType);
            string matchingMediaType = null;
            // find the one with the version number - match through regex
            Regex regEx = new Regex(@"application\/vnd\.mmsapi\.v([\d]+)\+json");

            foreach (var mediaType in mediaTypes)
            {
                if (regEx.IsMatch(mediaType))
                {
                    matchingMediaType = mediaType;
                    break;
                }
            }

            if (matchingMediaType == null)
                return null;

            // extract the version number
            Match m = regEx.Match(matchingMediaType);
            versionAsString = m.Groups[1].Value;

            // ... and return
            int version;
            if (versionAsString != null && Int32.TryParse(versionAsString, out version))
            {
                return version;
            }

            return null;
        }

        /// <summary>
        /// Check the request header, and the query string to determine if a version number has been provided
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private int? GetVersionHeaderOrQuery(HttpRequestMessage request)
        {
            string versionAsString;
            IEnumerable<string> headerValues;
            if (request.Headers.TryGetValues(VersionHeaderName, out headerValues) && headerValues.Count() == 1)
            {
                versionAsString = headerValues.First();
                int version;
                if (versionAsString != null && Int32.TryParse(versionAsString, out version))
                {
                    return version;
                }
            }
            else
            {
                var query = System.Web.HttpUtility.ParseQueryString(request.RequestUri.Query);
                string versionStr = query[VersionHeaderName];
                int version = 0;
                int.TryParse(versionStr, out version);

                if (version > 0)
                    return version;
            }

            return null;
        }
    }
}
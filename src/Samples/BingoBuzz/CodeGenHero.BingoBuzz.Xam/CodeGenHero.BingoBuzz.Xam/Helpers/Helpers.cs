using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenHero.BingoBuzz.Xam
{
    public static class Helpers
    {
        public static bool DoIHaveInternet
        {
            get
            {
                if (!CrossConnectivity.IsSupported)
                {
                    //this just seems weird, but it is for mocking unit tests
                    return true;
                }

                return CrossConnectivity.Current.IsConnected;
            }
        }
    }
}
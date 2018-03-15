using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static ObservableCollection<T> ToObservableCollection<T>(this IList<T> list)
        {
            var retVal = new ObservableCollection<T>();
            foreach (var item in list)
            {
                retVal.Add(item);
            }
            return retVal;
        }
    }
}
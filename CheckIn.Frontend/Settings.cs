using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckIn.Frontend
{
    public static class Settings
    {
        public static string API_URL { get; private set; }

        public static void SetURL(string url)
        {
            API_URL = url;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.BaseClasses
{
    public sealed class Settings
    {
        private static readonly Lazy<Settings> settings = new Lazy<Settings>(() => new Settings());
        private Settings()
        {

        }

        public static Settings Instanze { get { return settings.Value; } }
        private void Load()
        {

        }

        public int BroadcastPort
        {
            get
            {
                return 4711;
            }
        }


    }
}

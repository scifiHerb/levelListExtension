using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace levelListExtension.Settings
{
    public class Settings
    {
        public static Settings Instance { get; set; } = null;
        public virtual bool Enable { get; set; } = true;
        public int selectDiff = 4;
        public int count = 100;
        public bool refresh = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace levelListExtension.Settings
{
    public class Configuration
    {
        public static Configuration Instance { get; set; } = null;
        public virtual bool enable { get; set; } = true;
        public virtual bool refresh { get; set; } = true;
        public virtual bool priorityPlaylist { get; set; } = true;
        public int selectDiff =4;
        public int count =200;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Intent
    {
        //These are named this way so it can be easily added to the Intents Json
        public string intent { get; set; }
        public List<string> patterns { get; set; }

        public List<string> responses { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Siimple_Back_End.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Url { get; set; }
    }
}

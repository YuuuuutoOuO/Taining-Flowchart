using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taining.Function
{
    public class DataHolder
    {
        private static readonly DataHolder _instance = new();
        public static DataHolder Instance => _instance;
        public List<NodeData> Datas { get; set; }
        private DataHolder() { }
    }
}
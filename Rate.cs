using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace baithucuaanhhuy
{
    internal class Rate
    {
        public string IDRate { get; set; }
        public string IDPhim { get; set; }
        public string Danhgia { get; set; }
        public virtual Phim Phim { get; set; }
    }
}

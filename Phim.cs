using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace baithucuaanhhuy
{
    internal class Phim
    {
        public string IDPhim { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public string Director { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Rate> rates { get; set; }

    }
}

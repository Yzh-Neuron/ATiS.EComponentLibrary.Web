using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Other
{
    public class PageInfo<T> where T : class, new()
    {
        public int Total { get; set; }
        public List<T> Data { get; set; }
    }
}

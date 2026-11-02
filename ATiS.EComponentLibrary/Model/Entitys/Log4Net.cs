using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    /// <summary>
    /// 日志
    /// </summary>
    public class Log4Net
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(IsNullable = false)]
        public DateTime Date { get; set; }
        [SugarColumn(IsNullable = false)]
        public string Thread { get; set; }
        [SugarColumn(IsNullable = false)]
        public string Level { get; set; }
        [SugarColumn(IsNullable = false)]
        public string Logger { get; set; }
        [SugarColumn(IsNullable = false)]
        public string Message { get; set; } 
        public string Exception { get; set; }
    }
}

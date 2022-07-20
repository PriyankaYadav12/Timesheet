using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTimeSheetManagement.Models
{
    [Table("TechnologyMaster")]
    public class TechnologyMaster
    {
        [Key]
        public int TechnologyId { get; set; }
        public string TechnologyName { get; set; }
        public bool IsActive { get; set; }
    }
}

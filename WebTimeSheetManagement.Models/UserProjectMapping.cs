using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTimeSheetManagement.Models
{
    [Table("UserProjectMapping")]
    public class UserProjectMapping
    {
        [Key]
        public int UserProjectMappingID { get; set; }
        public int RegistrationID { get; set; }
        public int ProjectID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyClass.Models
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Đường dẫn không được để trống")]
        public string Link { get; set; }
        [Required(ErrorMessage = "Phân loại không được để trống")]
        public string Type { get; set; }
        public int Table { get; set; }
        public int ParentId { get; set; }
        public int Orders { get; set; }
        [Required]
        public int Status { get; set; }
    }
}

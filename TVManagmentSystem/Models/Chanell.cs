using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVManagmentSystem.Models
{
    [Table("Chanells")]
    public class Chanell
    {
        [Key]
        public int ID { get; set; }
        public string?Name { get; set; }
        public List<Info> info { get; set; }
    }
}

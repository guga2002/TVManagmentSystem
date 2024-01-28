using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVManagmentSystem.Models
{
    [Table("Infos")]
    public class Info
    {
        [Key]
        public int InfoID  { get; set; }
        public string Source { get; set; }

        public string EMR { get; set; }

        [ForeignKey("chan")]
        public int  ChanellID { get; set; }

        public Chanell chan { get; set; }
    }
}

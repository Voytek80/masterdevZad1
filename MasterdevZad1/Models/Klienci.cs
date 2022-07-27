using System.ComponentModel.DataAnnotations;

namespace MasterdevZad1.Models
{
    public class Klienci
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Surname { get; set; }
        [MaxLength(11)]
        public string PESEL { get; set; }
        public int BirthYear { get; set; }
        public int Płeć { get; set; }
    }
}

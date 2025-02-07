using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApplication7.Models
{
    public class Signature
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SignatureId { get; set; }

        [Required]
        public int PetitionId { get; set; }

        [ForeignKey("PetitionId")]
        public virtual Petition Petition { get; set; }

        // This is the ID from the IdentityUser
        [Required]
        public string UserName { get; set; }


        public DateTime SignatureDate { get; set; } = DateTime.Now;
    }
}

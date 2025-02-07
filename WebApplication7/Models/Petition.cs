
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication7.Models
{
    public class Petition
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PetitionId { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        // Assuming you store the path to the image or URL
        public string ImagePath { get; set; }

        // Navigation property for signatures
        public virtual ICollection<Signature> Signatures { get; set; } = new List<Signature>();

        [NotMapped] // Don't map to the database; it's a calculated property
        public int SignatureCount => Signatures.Count;

        // Additional properties and methods here
        public DateTime DateCreated { get; set; } = DateTime.Now;

        // Foreign key for the user who created the petition

        public string CreatedBy { get; set; }


        public Petition()
        {

        }
    }
}

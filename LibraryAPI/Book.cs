using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI
{
    public class Book
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "varchar(100)")]
        public string Title { get; set; } = string.Empty;
        [Column(TypeName = "varchar(100)")]
        public string Subtitle { get; set; } = string.Empty;
        [Column(TypeName = "varchar(500)")]
        public string Summary { get; set; } = string.Empty;
        [Column(TypeName = "int")]
        public int Pages { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Column(TypeName = "varchar(150)")]
        public string Editor { get; set; } = string.Empty;
        [Column(TypeName = "int")]
        public int Edition { get; set; }
        [ForeignKey(nameof(Autor))]
        public List<Autor> Autors { get; set; }

    }
}

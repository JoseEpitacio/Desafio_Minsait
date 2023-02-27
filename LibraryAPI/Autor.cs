using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LibraryAPI
{
    public class Autor
    {
        [Key]
        public Guid Id { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        [ForeignKey(nameof(Book))]
        [JsonIgnore]
        public List<Book> Books { get; set;}
    }
}

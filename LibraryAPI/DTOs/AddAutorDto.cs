using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.DTOs
{
    public class AddAutorDto
    {
        public Guid BookId { get; set; }
        public string Name { get; set; }
    }
}

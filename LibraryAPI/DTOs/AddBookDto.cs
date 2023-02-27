using System.Text.Json.Serialization;

namespace LibraryAPI.DTOs
{
    public class AddBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int Pages { get; set; }
        public DateTime Date { get; set; }
        public string Editor { get; set; } = string.Empty;
        public int Edition { get; set; }
        public string autorName { get; set; }
    }
}

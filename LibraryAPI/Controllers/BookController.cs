using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryAPI.DTOs;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly DataContext _context;

        public BookController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get()
        {
            return Ok(await _context.Books.Include(b => b.Autors).ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Book>>> Get(Guid id)
        {
            var book = await _context.Books.Where(b => b.Id == id).Include(b => b.Autors).FirstOrDefaultAsync();
            if (book != null)
                return Ok(book);
            return NotFound("Book not Found");
        }

        [HttpPost]
        public async Task<ActionResult<List<Book>>> AddBook(AddBookDto newBook)
        {

            var titutlo = await _context.Books.Where(b => b.Title.Equals(newBook.Title)).Where(b => b.Date == newBook.Date).FirstOrDefaultAsync();
            if (titutlo != null)
                return BadRequest("Livro já existe");

            var newAutor = new Autor();
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = newBook.Title,
                Subtitle = newBook.Subtitle,
                Summary = newBook.Summary,
                Pages = newBook.Pages,
                Date = newBook.Date,
                Editor = newBook.Editor,
                Edition = newBook.Edition
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            var autor = await _context.Autors
                .Where(autor => autor.Name
                .Equals(newBook.autorName))
                .FirstOrDefaultAsync();

            if (autor == null)
            {
                newAutor = new Autor
                {
                    Id = Guid.NewGuid(),
                    Name = newBook.autorName
                };

                _context.Autors.Add(newAutor);
                await _context.SaveChangesAsync();
            }
            else
            {
                newAutor = autor;
            }


            var autorBook = new AutorBookDto
            {
                AutorId = newAutor.Id,
                BookId = book.Id
            };

            await AddBookAutor(autorBook);

            return Ok(await _context.Books.Include(b => b.Autors).ToListAsync());
        }

        [HttpPost("Autor")]
        public async Task<ActionResult<List<Book>>> AddBookAutor(AutorBookDto request)
        {
            var book = await _context.Books
                .Where(b => b.Id == request.BookId)
                .Include(b => b.Autors)
                .FirstOrDefaultAsync();

            if (book == null) 
                return BadRequest("Book not found");

            var autor = await _context.Autors.FindAsync(request.AutorId);
            if (autor == null)
                return BadRequest("Autor not found");

            book.Autors.Add(autor);
            await _context.SaveChangesAsync();

            return Ok(await _context.Books.Include(b => b.Autors).ToListAsync());

        }

        [HttpPost("Search")]
        public async Task<ActionResult<List<Book>>> getBookByTitle(SearchDto request)
        {
            return await _context.Books.Where(b => b.Title == request.ToSearch).Include(b => b.Autors).ToListAsync();
        }

        [HttpPost("Search/Autor")]
        public async Task<ActionResult<List<Book>>> getAutorByName(SearchDto request)
        {
            var autor = await _context.Autors.Where(a => a.Name == request.ToSearch).FirstOrDefaultAsync();
            if (autor == null)
                return BadRequest("Autor não existe");

            var books = await _context.Books.Where(b => b.Autors.Contains(autor)).Include(b => b.Autors).ToListAsync();
            return books;
        }

        [HttpPut]
        public async Task<ActionResult<List<Book>>> UpdateBook(UpdateBookDto request)
        {
            var book = await _context.Books
                .Where(b => b.Id == request.Id)
                .Include(b => b.Autors)
                .FirstOrDefaultAsync();

            if (book == null)
                return BadRequest("Book not Found");

            book.Title = request.Title;
            book.Subtitle = request.Subtitle;
            book.Summary = request.Summary;
            book.Pages = request.Pages;
            book.Date = request.Date;
            book.Editor = request.Editor;
            book.Edition = request.Edition;

            await _context.SaveChangesAsync();

            return Ok(await _context.Books.Include(b => b.Autors).ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Book>>> Delete(Guid id)
        {
            var dbBook = await _context.Books.FindAsync(id);
            if (dbBook == null)
                return BadRequest("Book not Found");

            _context.Books.Remove(dbBook);
            await _context.SaveChangesAsync();

            return Ok(await _context.Books.Include(b => b.Autors).ToListAsync());
        }

        [HttpPost("DeleteAutor")]
        public async Task<ActionResult<List<Book>>> Delete(AutorBookDto request)
        {
            var book = await _context.Books
                .Where(b => b.Id == request.BookId)
                .Include(b => b.Autors)
                .FirstOrDefaultAsync();

            if (book == null)
                return BadRequest("Book not found");

            var autor = await _context.Autors.FindAsync(request.AutorId);
            if (autor == null)
                return BadRequest("Autor not found");

            book.Autors.Remove(autor);
            await _context.SaveChangesAsync();

            return Ok(await _context.Books.Include(b => b.Autors).ToListAsync());
        }
    }
}
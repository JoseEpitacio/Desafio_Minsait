using LibraryAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
        private readonly DataContext _context;

        public AutorController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> Get()
        {
            return Ok(await _context.Autors.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Book>>> Get(Guid id)
        {
            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
                return BadRequest("Autor not Found");
            return Ok(await _context.Autors.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<Book>>> AddAutor(AddAutorDto request)
        {
            var autor = new Autor();
            var searchAutor = await _context.Autors
                .Where(a => a.Name.Equals(request.Name))
                .FirstOrDefaultAsync();
            if (searchAutor == null) 
            {
                autor = new Autor
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                };
                _context.Autors.Add(autor);
                await _context.SaveChangesAsync();
            }
            else
            {
                autor = searchAutor;
            }

            var autorBook = new AutorBookDto
            {
                AutorId = autor.Id,
                BookId = request.BookId
            };

            await AddAutorBook(autorBook);

            return Ok(await _context.Books.Include(b => b.Autors).ToListAsync());
        }

        [HttpPost("Book")]
        public async Task<ActionResult<List<Book>>> AddAutorBook(AutorBookDto request)
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

        [HttpPut]
        public async Task<ActionResult<List<Book>>> UpdateAutor(UpdateAutorDto request)
        {
            var dbAutor = await _context.Autors.FindAsync(request.Id);
            if (dbAutor == null)
                return BadRequest("Autor not Found");

            dbAutor.Name = request.Name;

            await _context.SaveChangesAsync();

            return Ok(await _context.Books.Include(b => b.Autors).ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Book>>> Delete(Guid id)
        {
            var dbAutor = await _context.Autors.FindAsync(id);
            if (dbAutor == null)
                return BadRequest("Autor not Found");

            _context.Autors.Remove(dbAutor);
            await _context.SaveChangesAsync();

            return Ok(await _context.Books.Include(b => b.Autors).ToListAsync());
        }
    }
}

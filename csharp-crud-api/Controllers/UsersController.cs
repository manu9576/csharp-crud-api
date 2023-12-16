using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly UserContext _context;
		private readonly ILogger<UsersController> _logger;

		public UsersController(ILogger<UsersController> logger, UserContext context)
		{
			_logger = logger;
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			return await _context.Users.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(int id)
		{
			User? user = await _context.Users.FindAsync(id);

			if (user is null)
			{
				return NotFound("No user found");
			}

			return user;
		}

		// POST: api/users
		[HttpPost]
		public async Task<ActionResult<User>> PostUser(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> PutUser(int id, User user)
		{
			if (id != user.Id)
			{
				return BadRequest("id must be identical");
			}

			if (!UserExists(id))
			{
				return NotFound();
			}

			_context.Entry(user).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/users/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return NotFound();
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// dummy method to test the connection
		[HttpGet("hello")]
		public string Test()
		  => "Hello World!";

		private bool UserExists(int id)
			=> _context.Users.Any(u => u.Id == id);
	}
}
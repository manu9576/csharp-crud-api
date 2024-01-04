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
		public async Task<ActionResult<User>> GetUser(
			[FromRoute] int id)
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
		public async Task<ActionResult<User>> PostUser(
			[FromBody] User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
		}

		// PUT: api/users/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutUser(
			[FromRoute] int id,
			[FromBody] User user)
		{
			if (id != user.Id)
			{
				return BadRequest("id must be identical");
			}

			if (!await UserExists(id))
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

		private Task<bool> UserExists(int id)
			=> _context.Users.AnyAsync(u => u.Id == id);
	}
}
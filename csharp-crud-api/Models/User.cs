using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
	[Table("users")]
	public class User
	{
		[Column("id")]
		public required int Id { get; set; }

		[Column("name")]
		public required string Name { get; set; }

		[Column("email")]
		public required string Email { get; set; }
	}
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
	[Table("users")]
	public class User : Identity, CreatedTimeStamp
	{
		[Column("id")]
		public Guid Id { get; set; }

		[MaxLength(100)]
		[Column("name")]
		public string? Name { get; set; }

		[MaxLength(200)]
		[Column("email")]
		public string? Email { get; set; }

		[MaxLength(200)]
		[Column("password")]
		public string? Password { get; set; }

		[Column("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		public ICollection<UserOAuth> OAuths { get; set; } = new List<UserOAuth>();
	}
}
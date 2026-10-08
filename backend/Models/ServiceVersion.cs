using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
	[Table("service_version")]
	public class ServiceVersion : CreatedTimeStamp , Identity
	{
		[Column("id")]
		public Guid Id { get; set; }

		[Column("service_id")]
		public Guid ServiceId { get; set; }

		[MaxLength(10)]
		[Column("version")]
		public required string Version { get; set; }
		
		[MaxLength(10)]
		[Column("schema_version")]
		public required string SchemaVersion { get; set; }

		[Column("config" , TypeName = "jsonb")]
		public required string Config { get; set; }
		
		[Column("created_at")]
		public DateTimeOffset CreatedAt { get; set; }

		public Service? Service { get; set; } = null!;
	}
}

// service_versions
// id
// service_id
// version
// schema_version
// config
// created_at

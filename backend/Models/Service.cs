using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("services")]
public class Service : TimeStamp , Identity
{
	[Column("id")]
	public Guid Id { get; set; }

	[MaxLength(20)]
	[Column("name")]
	public required string Name { get; set; }
	
	[MaxLength(200)]
	[Column("description")]
	public string? Description { get; set; }
	
	[MaxLength(10)]
	[Column("latest_version")]
	public required string LatestVersion { get; set; }

	[Column("created_at")]
	public DateTimeOffset CreatedAt { get; set; }

	[Column("updated_at")]
	public DateTimeOffset UpdatedAt { get; set; }

	public ICollection<ServiceVersion> ServiceVersions 	= new List<ServiceVersion>();
}


// services
// id
// name
// description
// latest_version
// created_at
// updated_at

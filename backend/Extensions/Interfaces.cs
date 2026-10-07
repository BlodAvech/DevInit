public interface CreatedTimeStamp
{
	public DateTimeOffset CreatedAt { get; set; }
}
public interface TimeStamp
{
	public DateTimeOffset CreatedAt { get; set; }
	public DateTimeOffset UpdatedAt { get; set; }
}

public interface Identity
{
	public Guid Id { get; set; }
}
namespace Database;



public class UnifiedDataRecord {

	public required int Id { get; init; }

	public required string OriginatingDevice { get; init; }

	public required string TableName { get; init; }

	public required DateTime TimeCreated { get; init; }

}
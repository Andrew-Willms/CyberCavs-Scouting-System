namespace CCSSDomain.GameSpecification; 



public class ButtonSpec {

	public required string DataFieldName { get; init; }

	public required int IncrementAmount { get; init; }

	public required string ButtonText { get; init; }

	public required (double X, double Y) Location { get; init; }

	public required (double Width, double Height) Size { get; set; }

}
namespace GameMakerWpf.Domain.EditingData; 



public class ButtonEditingData {

	public required string DataFieldName { get; init; }

	public required string IncrementAmount { get; set; }

	public required string ButtonText { get; init; }

	public required string XPosition { get; init; }
	public required string YPosition { get; init; }

	public required string Width { get; init; }
	public required string Height { get; init; }

}
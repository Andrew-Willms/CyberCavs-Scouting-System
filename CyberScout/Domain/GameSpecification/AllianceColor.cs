using System.Drawing;

namespace Domain.GameSpecification;



public record struct AllianceColor {

	public required string Name { get; init; }

	public required Color Color { get; init; }

}
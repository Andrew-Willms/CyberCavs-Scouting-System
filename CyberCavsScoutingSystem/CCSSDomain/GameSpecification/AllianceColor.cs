using System.Drawing;

namespace CCSSDomain.GameSpecification;



public record struct AllianceColor {

	public required string Name { get; init; }

	public required Color Color { get; init; }

}
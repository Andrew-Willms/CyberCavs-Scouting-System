using System;

namespace CCSSDomain.GameSpecification; 



public class Event {

	public required string Name { get; init; }

	public required string EventCode { get; init; }

	public required DateTime StartDate { get; init; }

	public required DateTime EndDate { get; init; }

}
namespace WPFUtilities;



// I think these things need to be static
public record ValidationError<TSeverityEnum>(string Name, TSeverityEnum Severity, string Description = "")
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum> {

	public string Name { get; } = Name;

	// If I remove Description from the primary constructor I need this.
	//public string Description { get; init; } = Description;

	public TSeverityEnum Severity { get; } = Severity;

}
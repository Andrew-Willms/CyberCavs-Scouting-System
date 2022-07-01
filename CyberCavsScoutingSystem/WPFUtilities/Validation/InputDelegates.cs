﻿using System.Collections.ObjectModel;

namespace WPFUtilities.Validation;



public delegate (TTargetType?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	StringInputConverter<TTargetType, TSeverityEnum>
	(string inputString)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate ValidationError<TSeverityEnum>?
	StringInputValidator<in TTargetType, TSeverityEnum>
	(TTargetType targetObject)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate ValidationError<TSeverityEnum>?
	StringInputValidator<in TTargetType, in TValidationParameter, TSeverityEnum>
	(TTargetType targetObject, TValidationParameter parameter)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;



public delegate (TTargetType?, ReadOnlyList<ValidationError<TSeverityEnum>>)
	MultiInputConverter<TTargetType, TSeverityEnum>
	(in ReadOnlyDictionary<string, IStringInput<TSeverityEnum>> stringInputComponents)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate ValidationError<TSeverityEnum>?
	MultiInputValidator<in TTargetType, TSeverityEnum>
	(TTargetType targetObject)
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;

public delegate ValidationError<TSeverityEnum>?
	MultiInputValidator<in TTargetType, in TValidationParameter, TSeverityEnum>
	(TTargetType targetObject, TValidationParameter parameter) 
	where TSeverityEnum : ValidationErrorSeverityEnum<TSeverityEnum>, IValidationErrorSeverityEnum<TSeverityEnum>;


/*
When value types are used in StringInputValidator<TheValueType, TSeverityEnum> the method cannot return a nullable
version of that value type because C# handles nullable reference types and nullable value types differently
*/
﻿using GameMakerWpf.Domain;
using GameMakerWpf.Domain.Editors;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using UtilitiesLibrary.Collections;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.DisplayData.Errors.ErrorData; 



public static class RemoveFromListErrors {

	public static ErrorDisplayData RemoveAllianceError(IListRemoveResult<AllianceEditor>.ItemNotFound error) => new() {
		Caption = "Alliance Not Found",
		Message = "The Alliance you are trying to remove from the Game Project could not be found."
	};

	public static ErrorDisplayData RemoveDataFieldError(IListRemoveResult<DataFieldEditor>.ItemNotFound error) => new() {
		Caption = "Data Field Not Found",
		Message = "The Data Field you are trying to remove from the Game Project could not be found."
	};

	public static ErrorDisplayData RemoveOptionError(IListRemoveResult<SingleInput<string, string, ErrorSeverity>>.ItemNotFound error) => new() {
		Caption = "Option Not Found",
		Message = "The Option you are trying to remove from the Selection Data Field could not be found."
	};

	public static ErrorDisplayData RemoveSetupInputError(IListRemoveResult<InputEditor>.ItemNotFound error) => new() {
		Caption = "Setup Input Not Found",
		Message = "The Input you are trying to remove from the Setup Tab Inputs could not be found."
	};

	public static ErrorDisplayData RemoveAutoInputError(IListRemoveResult<InputEditor>.ItemNotFound error) => new() {
		Caption = "Auto Input Not Found",
		Message = "The Input you are trying to remove from the Auto Tab Inputs could not be found."
	};

	public static ErrorDisplayData RemoveTeleInputError(IListRemoveResult<InputEditor>.ItemNotFound error) => new() {
		Caption = "Tele Input Not Found",
		Message = "The Input you are trying to remove from the Tele Tab Inputs could not be found."
	};

	public static ErrorDisplayData RemoveEndgameInputError(IListRemoveResult<InputEditor>.ItemNotFound error) => new() {
		Caption = "Endgame Input Not Found",
		Message = "The Input you are trying to remove from the Endgame Tab Inputs could not be found."
	};

}
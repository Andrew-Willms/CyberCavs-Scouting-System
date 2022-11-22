using System;
using CCSSDomain;
using GameMakerWpf.Domain.EditingData;
using GameMakerWpf.Domain.Editors.DataFieldEditors;
using UtilitiesLibrary.Validation.Inputs;

namespace GameMakerWpf.Domain.Editors; 



public class InputEditor {

	private GameEditor GameEditor { get; }

	public DataFieldTypeEditor DataField { get; }

	public SingleInput<string, string, ErrorSeverity> Label { get; }

	public InputEditor(GameEditor gameEditor, InputEditingData initialValues) {

		GameEditor = gameEditor;

	}

	public InputEditingData ToEditingData() {
		throw new NotImplementedException();
	}

}
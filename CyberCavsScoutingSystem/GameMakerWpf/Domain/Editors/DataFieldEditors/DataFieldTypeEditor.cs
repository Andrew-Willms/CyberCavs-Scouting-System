using OneOf;

namespace GameMakerWpf.Domain.Editors.DataFieldEditors;



public abstract class DataFieldTypeEditorBase;



[GenerateOneOf]
public partial class DataFieldTypeEditor :
	OneOfBase<BooleanDataFieldEditor, TextDataFieldEditor, IntegerDataFieldEditor, SelectionDataFieldEditor> {

	public DataFieldTypeEditorBase AsBase => Match<DataFieldTypeEditorBase>(
		booleanDataFieldEditor => booleanDataFieldEditor,
		textDataFieldEditor => textDataFieldEditor,
		integerDataFieldEditor => integerDataFieldEditor,
		selectionDataFieldEditor => selectionDataFieldEditor);
}
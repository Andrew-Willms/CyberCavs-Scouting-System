using ExhaustiveMatching;
namespace GameMakerWpf.Domain.Editors.DataFieldEditors;



[Closed(typeof(BooleanDataFieldEditor), typeof(TextDataFieldEditor), typeof(IntegerDataFieldEditor), typeof(SelectionDataFieldEditor))]
public abstract class DataFieldTypeEditor;
using UnityEngine;
using System.Collections;
using UnityEditor;
using PixelCrushers.DialogueSystem;


namespace PixelCrushers.DialogueSystem {

	[CustomFieldTypeService.Name("Localization")]
	public class CustomFieldType_Localization : CustomFieldType {
		public override FieldType storeFieldAsType {
			get {
				return FieldType.Localization;
			}
		}
		public override string Draw (string currentValue, DialogueDatabase dataBase) {
			//return PixelCrushers.DialogueSystem.DialogueEditor.DialogueEditorWindow.instance.DrawAssetPopup<Location>(currentValue, (dataBase != null) ? dataBase.locations : null, null);
			return EditorGUILayout.TextField(currentValue);
		}
	}
}
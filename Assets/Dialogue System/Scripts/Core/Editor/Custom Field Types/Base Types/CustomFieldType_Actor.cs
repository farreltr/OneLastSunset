using UnityEngine;
using System.Collections;
using UnityEditor;

namespace PixelCrushers.DialogueSystem {

	[CustomFieldTypeService.Name("Actor")]
	public class CustomFieldType_Actor : CustomFieldType {
		public override FieldType storeFieldAsType {
			get {
				return FieldType.Actor;
			}
		}
		public override string Draw (string currentValue, DialogueDatabase dataBase) {
			return PixelCrushers.DialogueSystem.DialogueEditor.DialogueEditorWindow.instance.DrawAssetPopup<Actor>(currentValue, (dataBase != null) ? dataBase.actors : null, null);
		}
	}
}
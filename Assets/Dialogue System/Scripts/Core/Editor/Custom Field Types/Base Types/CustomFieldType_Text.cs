using UnityEngine;
using System.Collections;
using UnityEditor;

namespace PixelCrushers.DialogueSystem {

	[CustomFieldTypeService.Name("Text")]
	public class CustomFieldType_Text : CustomFieldType {
		public override string Draw (string currentValue, DialogueDatabase dataBase) {
			return EditorGUILayout.TextField(currentValue);
		}
	}
}
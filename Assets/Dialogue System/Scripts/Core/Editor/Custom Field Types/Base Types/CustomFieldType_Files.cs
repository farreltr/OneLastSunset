using UnityEngine;
using System.Collections;
using UnityEditor;

namespace PixelCrushers.DialogueSystem {

	[CustomFieldTypeService.Name("Files")]
	public class CustomFieldType_Files : CustomFieldType {
		public override string Draw (string currentValue, DialogueDatabase dataBase) {
			return EditorGUILayout.TextField(currentValue);
		}
	}
}
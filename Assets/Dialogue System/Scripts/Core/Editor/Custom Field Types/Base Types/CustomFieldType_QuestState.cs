using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace PixelCrushers.DialogueSystem {

	[CustomFieldTypeService.Name("Quest State")]
	public class CustomFieldType_QuestState : CustomFieldType {
		private static readonly string[] questStateStrings = { "(None)", "unassigned", "active", "success", "failure", "done", "abandoned" };
		
		public override string Draw (string currentValue, DialogueDatabase dataBase) {
			if (currentValue == string.Empty)
				currentValue = questStateStrings[0];
			
			int currentIndex = 0;
			for (int i = 0; i < questStateStrings.Length; i++) {
				var item = questStateStrings [i];
				if (item == currentValue) {
					currentIndex = i;
					break;
				}
			}
			
			int index = EditorGUILayout.Popup(currentIndex, questStateStrings);
			
			return questStateStrings[index];
		}
	}
}
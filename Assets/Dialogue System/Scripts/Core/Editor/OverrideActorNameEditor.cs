using UnityEngine;
using UnityEditor;

namespace PixelCrushers.DialogueSystem {

	[CustomEditor(typeof(OverrideActorName))]
	public class OverrideActorNameEditor : Editor {

		public override void OnInspectorGUI() {
			var overrideActorName = target as OverrideActorName;
			if (overrideActorName == null) return;

			overrideActorName.overrideName = EditorGUILayout.TextField(new GUIContent("Override Name", "Use this name instead of the GameObject name in conversations"), overrideActorName.overrideName);

			EditorGUILayout.BeginHorizontal();
			overrideActorName.internalName = EditorGUILayout.TextField(new GUIContent("Internal Name", "Use this name instead of the GameObject name when saving persistent data. Leave blank to use Override Name"), overrideActorName.internalName);
			if (GUILayout.Button(new GUIContent("Unique", "Assign a unique internal name"), GUILayout.Width(60))) {
				overrideActorName.internalName = DialogueLua.StringToTableIndex(OverrideActorName.GetActorName(overrideActorName.transform) + "_" + overrideActorName.GetInstanceID());
			}
			EditorGUILayout.EndHorizontal();
		}

	}

}

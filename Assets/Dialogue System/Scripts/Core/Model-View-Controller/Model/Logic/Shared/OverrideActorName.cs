using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem {

	/// <summary>
	/// This component allows you to override the actor name used in conversations,
	/// which is normally set to the name of the GameObject. If the override name
	/// contains a [lua] or [var] tag, it parses the value.
	/// </summary>
	[AddComponentMenu("Dialogue System/Actor/Override Actor Name")]
	public class OverrideActorName : MonoBehaviour {
		
		/// <summary>
		/// Overrides the actor name used in conversations.
		/// </summary>
		public string overrideName;

		/// <summary>
		/// The internal name to use in the dialogue database when saving persistent data.
		/// If blank, uses the override name.
		/// </summary>
		public string internalName;

		/// <summary>
		/// Gets the name of the override, including parsing if it contains a [lua]
		/// or [var] tag.
		/// </summary>
		/// <returns>The override name.</returns>
		public string GetOverrideName() {
			if (overrideName.Contains("[lua") || overrideName.Contains("[var")) {
				return FormattedText.Parse(overrideName, DialogueManager.MasterDatabase.emphasisSettings).text;
			} else {
				return overrideName;
			}
		}

		public string GetInternalName() {
			return string.IsNullOrEmpty(internalName) ? GetOverrideName() : internalName;
		}
		
		/// <summary>
		/// Gets the name of the actor, either from the GameObject or its OverrideActorComponent
		/// if present.
		/// </summary>
		/// <returns>The actor name.</returns>
		/// <param name="t">The actor's transform.</param>
		public static string GetActorName(Transform t) {
			OverrideActorName overrideActorName = t.GetComponentInChildren<OverrideActorName>();
			if (overrideActorName == null && t.parent != null) overrideActorName = t.parent.GetComponent<OverrideActorName>();
			return ((overrideActorName == null) || string.IsNullOrEmpty(overrideActorName.overrideName)) ? t.name : overrideActorName.GetOverrideName();
		}
		
		public static string GetInternalName(Transform t) {
			OverrideActorName overrideActorName = t.GetComponentInChildren<OverrideActorName>();
			if (overrideActorName == null && t.parent != null) overrideActorName = t.parent.GetComponent<OverrideActorName>();
			if (overrideActorName != null) {
				if (!string.IsNullOrEmpty(overrideActorName.internalName)) return overrideActorName.internalName;
				if (!string.IsNullOrEmpty(overrideActorName.overrideName)) return overrideActorName.overrideName;
			}
			return t.name;
		}
		
	}

}

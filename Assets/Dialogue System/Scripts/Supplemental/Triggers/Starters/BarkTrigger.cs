using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem {
	
	/// <summary>
	/// The bark trigger component can be used to make an NPC bark when it receives a dialogue
	/// trigger -- for example, when the game starts or when the level loads. You can specify an 
	/// optional target to bark at.
	/// </summary>
	[AddComponentMenu("Dialogue System/Trigger/Bark Trigger")]
	public class BarkTrigger : BarkStarter {
		
		/// <summary>
		/// The target that the bark is directed to. If assigned, the target will get an
		/// OnBarkEnd event.
		/// </summary>
		public Transform target;
		
		/// <summary>
		/// The trigger that starts the conversation.
		/// </summary>
		public DialogueTriggerEvent trigger = DialogueTriggerEvent.OnUse;
		
		public void OnBarkEnd(Transform actor) {
			if (enabled && (trigger == DialogueTriggerEvent.OnBarkEnd)) TryBark(Tools.Select(target, actor));
		}
		
		public void OnConversationEnd(Transform actor) {
			if (enabled && (trigger == DialogueTriggerEvent.OnConversationEnd)) TryBark(Tools.Select(target, actor));
		}
		
		public void OnSequenceEnd(Transform actor) {
			if ((enabled && trigger == DialogueTriggerEvent.OnSequenceEnd)) TryBark(Tools.Select(target, actor));
		}
		
		public void OnUse(Transform actor) {
			if (enabled && (trigger == DialogueTriggerEvent.OnUse)) TryBark(Tools.Select(target, actor));
		}
		
		public void OnUse(string message) {
			if (enabled && (trigger == DialogueTriggerEvent.OnUse)) TryBark(Tools.Select(target, null));
		}
		
		public void OnUse() {
			if (enabled && (trigger == DialogueTriggerEvent.OnUse)) TryBark(Tools.Select(target, null));
		}
		
		public void OnTriggerEnter(Collider other) {
			if (enabled && (trigger == DialogueTriggerEvent.OnTriggerEnter)) TryBark(Tools.Select(target, other.transform), other.transform);
		}

		public void OnTriggerEnter2D(Collider2D other) {
			if (enabled && (trigger == DialogueTriggerEvent.OnTriggerEnter)) TryBark(Tools.Select(target, other.transform), other.transform);
		}
		
		public void OnTriggerExit(Collider other) {
			if (enabled && (trigger == DialogueTriggerEvent.OnTriggerExit)) TryBark(Tools.Select(target, other.transform), other.transform);
		}
		
		public void OnTriggerExit2D(Collider2D other) {
			if (enabled && (trigger == DialogueTriggerEvent.OnTriggerExit)) TryBark(Tools.Select(target, other.transform), other.transform);
		}
		
		public void Start() {
			// Waits one frame to allow all other components to finish their Start() methods.
			if (trigger == DialogueTriggerEvent.OnStart) StartCoroutine(BarkAfterOneFrame());
		}
		
		public void OnEnable() {
			// Waits one frame to allow all other components to finish their OnEnable() methods.
			if (trigger == DialogueTriggerEvent.OnEnable) StartCoroutine(BarkAfterOneFrame());
		}
		
		public void OnDisable() {
			if (trigger == DialogueTriggerEvent.OnDisable) TryBark(target);
		}
		
		public void OnDestroy() {
			if (trigger == DialogueTriggerEvent.OnDestroy) TryBark(target);
		}
		
		private IEnumerator BarkAfterOneFrame() {
			yield return null;
			TryBark(target);
		}
		
	}

}

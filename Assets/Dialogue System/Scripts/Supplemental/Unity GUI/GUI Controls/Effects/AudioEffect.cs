using UnityEngine;
using System.Collections;

namespace PixelCrushers.DialogueSystem.UnityGUI {
	
	/// <summary>
	/// Applies an audio effect (sound) to a GUI control. To use this effect, add an audio source
	/// to the GUI control's game object. When triggered, this effect plays the audio source.
	/// </summary>
	[AddComponentMenu("Dialogue System/UI/Unity GUI/Controls/Effects/Audio Effect (Unity GUI)")]
	public class AudioEffect : GUIEffect {

		private AudioSource myAudio = null;

		public void Awake() {
			myAudio = GetComponent<AudioSource>();
		}
		
		/// <summary>
		/// Plays the audio.
		/// </summary>
		public override IEnumerator Play() {
			if (myAudio != null) myAudio.Play();
			yield return null;
		}
		
	}

}

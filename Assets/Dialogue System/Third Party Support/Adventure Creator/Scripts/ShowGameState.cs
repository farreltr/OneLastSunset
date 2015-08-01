using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AC;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.AdventureCreator {

	/// <summary>
	/// This is a tiny utility script to show AC's current game state.
	 // </summary>
	[AddComponentMenu("Dialogue System/Third Party/Adventure Creator/Show Game State")]
	public class ShowGameState : MonoBehaviour {

		private StateHandler stateHandler = null;

		void Start() {
			GameObject persistentEngine = GameObject.FindWithTag(Tags.persistentEngine);
			stateHandler = (persistentEngine != null) ? persistentEngine.GetComponent<StateHandler>() : null;
		}

		void OnGUI() {
			GUILayout.Label(string.Format("GameState: {0}", (stateHandler != null) ? stateHandler.gameState.ToString() : "NULL"));

			//---
			//--- If you're having trouble with cursors, edit AC's PlayerCursor.cs and
			//--- change the "private" to "public" in the lines "private int selectedCursor" 
			//--- and "private bool showCursor" (lines 24-25). Then uncomment the two lines
			//--- below:

			//PlayerCursor playerCursor = FindObjectOfType<PlayerCursor>();
			//GUILayout.Label(string.Format("Cursor: {0}, {1}", playerCursor.showCursor, playerCursor.selectedCursor));
		}

	}

}

using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem {
	
	/// <summary>
	/// This component combines Application.LoadLevel[Async] with the saved-game data
	/// features of PersistentDataManager. To use it, add it to your Dialogue Manager
	/// object and pass the saved-game data to LevelManager.LoadGame().
	/// </summary>
	[AddComponentMenu("Dialogue System/Save System/Level Manager")]
	public class LevelManager : MonoBehaviour {
		
		/// <summary>
		/// The default starting level to use if none is recorded in the saved-game data.
		/// </summary>
		public string defaultStartingLevel;
		
		/// <summary>
		/// Indicates whether a level is currently loading. Only useful in Unity Pro, which
		/// uses Application.LoadLevelAsync().
		/// </summary>
		/// <value><c>true</c> if loading; otherwise, <c>false</c>.</value>
		public bool IsLoading { get; private set; }
		
		void Awake() {
			IsLoading = false;
		}
		
		/// <summary>
		/// Loads the game recorded in the provided saveData.
		/// </summary>
		/// <param name="saveData">Save data.</param>
		public void LoadGame(string saveData) {
			StartCoroutine(LoadLevelFromSaveData(saveData));
		}
		
		/// <summary>
		/// Restarts the game at the default starting level and resets the
		/// Dialogue System to its initial database state.
		/// </summary>
		public void RestartGame() {
			StartCoroutine(LoadLevelFromSaveData(null));
		}
		
		private IEnumerator LoadLevelFromSaveData(string saveData) {
			string levelName = defaultStartingLevel;
			if (string.IsNullOrEmpty(saveData)) {
				// If no saveData, reset the database.
				DialogueManager.ResetDatabase(DatabaseResetOptions.RevertToDefault);
			} else {
				// Put saveData in Lua so we can get Variable["SavedLevelName"]:
				Lua.Run(saveData, true);
				levelName = DialogueLua.GetVariable("SavedLevelName").AsString;
				if (string.IsNullOrEmpty(levelName)) levelName = defaultStartingLevel;
			}
			
			// Load the level:
			PersistentDataManager.LevelWillBeUnloaded();
			if (Application.HasProLicense()) {
				AsyncOperation async = Application.LoadLevelAsync(levelName);
				IsLoading = true;
				while (!async.isDone) {
					yield return null;
				}
				IsLoading = false;
			} else {
				Application.LoadLevel(levelName);
			}
			
			// Wait two frames for objects in the level to finish their Start() methods:
			yield return null;
			yield return null;
			
			// Then apply saveData to the objects:
			if (!string.IsNullOrEmpty(saveData)) {
				PersistentDataManager.ApplySaveData(saveData);
			}
		}
		
		/// <summary>
		/// Loads a level. Use to change levels while keeping data synced. This method
		/// also calls PersistentDataManager.Record() before changing levels and
		/// PersistentDataManager.Apply() after changing levels. After loading the level,
		/// it waits two frames to allow GameObjects to finish their initialization first.
		/// </summary>
		/// <param name="levelName">Level name.</param>
		public void LoadLevel(string levelName) {
			StartCoroutine(LoadLevelCoroutine(levelName));
		}

		private IEnumerator LoadLevelCoroutine(string levelName) {
			PersistentDataManager.Record();

			// Load the level:
			PersistentDataManager.LevelWillBeUnloaded();
			if (Application.HasProLicense()) {
				AsyncOperation async = Application.LoadLevelAsync(levelName);
				IsLoading = true;
				while (!async.isDone) {
					yield return null;
				}
				IsLoading = false;
			} else {
				Application.LoadLevel(levelName);
			}
			
			// Wait two frames for objects in the level to finish their Start() methods:
			yield return null;
			yield return null;

			// Apply position data, but don't apply player's position:
			var player = GameObject.FindGameObjectWithTag("Player");
			var persistentPos = (player != null) ? player.GetComponent<PersistentPositionData>() : null;
			var originalValue = false;
			if (persistentPos != null) {
				originalValue = persistentPos.restoreCurrentLevelPosition;
				persistentPos.restoreCurrentLevelPosition = false;
			}
			
			PersistentDataManager.Apply();
			if (persistentPos != null) {
				persistentPos.restoreCurrentLevelPosition = originalValue;
			}
		}
		
		/// <summary>
		/// Records the current level in Lua.
		/// </summary>
		public void OnRecordPersistentData() {
			DialogueLua.SetVariable("SavedLevelName", Application.loadedLevelName);
		}
		
	}
	
}
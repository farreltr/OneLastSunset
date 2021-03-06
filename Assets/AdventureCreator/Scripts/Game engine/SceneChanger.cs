/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2014
 *	
 *	"SceneChanger.cs"
 * 
 *	This script handles the changing of the scene, and stores
 *	which scene was previously loaded, for use by PlayerStart.
 * 
 */

using UnityEngine;
using System.Collections;

namespace AC
{

	/**
	 * Handles the changing of the scene, and keeps track of which scene was previously loaded.
	 * It should be placed on the PersistentEngine prefab.
	 */
	public class SceneChanger : MonoBehaviour
	{

		/** The number of the previous scene */
		public int previousScene = -1;
		/** The name of the previous scene */
		public string previousSceneName = "";

		private Player playerOnTransition = null;
		private Texture2D textureOnTransition = null;


		/**
		 * <summary>Loads a new scene.</summary>
		 * <param name = "sceneName">The name of the scene to load</param>
		 * <param name = "sceneNumber">The number of the scene to load, if sceneName = ""</param>
		 * <param name = "saveRoomData">If True, then the states of the current scene's Remember scripts will be recorded in LevelStorage</param>
		 */
		public void ChangeScene (string sceneName, int sceneNumber, bool saveRoomData)
		{
			bool useLoadingScreen = false;
			if (KickStarter.settingsManager != null && KickStarter.settingsManager.useLoadingScreen)
			{
				useLoadingScreen = true;
			}

			KickStarter.mainCamera.FadeOut (0f);

			if (KickStarter.player)
			{
				KickStarter.player.Halt ();
			
				if (KickStarter.settingsManager.movementMethod == MovementMethod.UltimateFPS)
				{
					UltimateFPSIntegration.SetCameraEnabled (false, true);
				}
			}

			Sound[] sounds = FindObjectsOfType (typeof (Sound)) as Sound[];
			foreach (Sound sound in sounds)
			{
				sound.TryDestroy ();
			}
			KickStarter.stateHandler.GatherObjects ();

			KickStarter.playerMenus.ClearParents ();
			KickStarter.dialog.KillDialog (true, true);

			if (saveRoomData)
			{
				KickStarter.levelStorage.StoreCurrentLevelData ();
				previousScene = Application.loadedLevel;
				previousSceneName = Application.loadedLevelName;
			}
			
			KickStarter.stateHandler.gameState = GameState.Normal;
			playerOnTransition = KickStarter.player;
			
			LoadLevel (sceneName, sceneNumber, useLoadingScreen);
		}


		/**
		 * <summary>Gets the Player prefab that was active during the last scene transition.</summary>
		 * <returns>The Player prefab that was active during the last scene transition</returns>
		 */
		public Player GetPlayerOnTransition ()
		{
			return playerOnTransition;
		}


		/**
		 * Destroys the Player prefab that was active during the last scene transition.
		 */
		public void DestroyOldPlayer ()
		{
			if (playerOnTransition)
			{
				Debug.Log ("New player prefab found - " + playerOnTransition.name + " deleted");
				DestroyImmediate (playerOnTransition.gameObject);
			}
		}


		/*
		 * <summary>Stores a texture used as an overlay during a scene transition. This texture can be retrieved with GetAndResetTransitionTexture().</summary>
		 * <param name = "_texture">The Texture2D to store</param>
		 */
		public void SetTransitionTexture (Texture2D _texture)
		{
			textureOnTransition = _texture;
		}


		/**
		 * <summary>Gets, and removes from memory, the texture used as an overlay during a scene transition.</summary>
		 * <returns>The texture used as an overlay during a scene transition</returns>
		 */
		public Texture2D GetAndResetTransitionTexture ()
		{
			Texture2D _texture = textureOnTransition;
			textureOnTransition = null;
			return _texture;
		}


		private void LoadLevel (string sceneName, int sceneNumber)
		{
			if (KickStarter.settingsManager != null && KickStarter.settingsManager.useLoadingScreen)
			{
				LoadLevel (sceneName, sceneNumber, true);
			}
			else
			{
				LoadLevel (sceneName, sceneNumber, false);
			}
		}


		private void LoadLevel (string sceneName, int sceneNumber, bool useLoadingScreen)
		{
			if (useLoadingScreen)
			{
				if (KickStarter.player)
				{
					KickStarter.player.transform.position += new Vector3 (0f, -10000f, 0f);
				}

				GameObject go = new GameObject ("LevelManager");
				LoadingScreen loadingScreen = go.AddComponent <LoadingScreen>();
				loadingScreen.InnerLoad (sceneName, sceneNumber, AdvGame.GetSceneName (KickStarter.settingsManager.loadingSceneIs, KickStarter.settingsManager.loadingSceneName), KickStarter.settingsManager.loadingScene);
			}
			else
			{
				if (sceneName != "")
				{
					Application.LoadLevel (sceneName);
				}
				else
				{
					Application.LoadLevel (sceneNumber);
				}
			}
		}

	}

}
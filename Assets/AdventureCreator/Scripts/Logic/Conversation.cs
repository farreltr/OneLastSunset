/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013
 *	
 *	"Conversation.cs"
 * 
 *	This script is handles character conversations.
 *	It generates instances of DialogOption for each line
 *	that the player can choose to say.
 * 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AC
{

	/**
	 * This component provides the player with a list of dialogue options that their character can say.
	 * Options are display in a MenuDialogList element, and will usually run a DialogueOption ActionList when clicked - unless overrided by the "Dialogue: Start conversation" Action that triggers it.
	 */
	public class Conversation : MonoBehaviour
	{

		/** The source of the commands that are run when an option is chosen (InScene, AssetFile, CustomScript) */	
		public InteractionSource interactionSource;
		/** All available dialogue options that make up the Conversation */
		public List<ButtonDialog> options = new List<ButtonDialog>();
		/** The option selected within the Conversation's Inspector  */
		public ButtonDialog selectedOption;

		/** If True, and only one option is available, then the option will be chosen automatically */
		public bool autoPlay = false;
		/** If True, then the Conversation is timed, and the options will only be shown for a fixed period */
		public bool isTimed = false;
		/** The duration, in seconds, that the Conversation is active, if isTime = True */
		public float timer = 5f;
		/** The index number of the option to select, if isTimed = True and the timer runs out before the player has made a choice */
		public int defaultOption = 0;

		private float startTime;

		
		private void Awake ()
		{
			Upgrade ();
		}


		/**
		 * Show the Conversation's dialogue options.
		 */
		public void Interact ()
		{
			Interact (null);
		}


		/**
		 * <summary>Shows the Conversation's dialogue options.</summary>
		 * <param name = "actionConversation">The "Dialogue: Start conversation" Action that calls this function.  This is necessary when that Action overrides the Converstion's options.</param>
		 */
		public void Interact (ActionConversation actionConversation)
		{
			KickStarter.actionListManager.SetConversationPoint (actionConversation);

			CancelInvoke ("RunDefault");
			int numPresent = 0;
			foreach (ButtonDialog _option in options)
			{
				if (_option.isOn)
				{
					numPresent ++;
				}
			}
			
			if (KickStarter.playerInput)
			{
				if (numPresent == 1 && autoPlay)
				{
					foreach (ButtonDialog _option in options)
					{
						if (_option.isOn)
						{
							RunOption (_option);
							return;
						}
					}
				}
				else if (numPresent > 0)
				{
					KickStarter.playerInput.activeConversation = this;
					KickStarter.stateHandler.gameState = GameState.DialogOptions;
				}
				else
				{
					KickStarter.playerInput.activeConversation = null;
				}
			}
			
			if (isTimed)
			{
				startTime = Time.time;
				Invoke ("RunDefault", timer);
			}
		}


		private void RunOption (ButtonDialog _option)
		{
			if (options.Contains (_option))
			{
				if (KickStarter.actionListManager.OverrideConversation (options.IndexOf (_option)))
				{
					return;
				}
			}

			Conversation endConversation;
			if (_option.conversationAction == ConversationAction.ReturnToConversation)
			{
				endConversation = this;
			}
			else if (_option.conversationAction == ConversationAction.RunOtherConversation && _option.newConversation != null)
			{
				endConversation = _option.newConversation;
			}
			else
			{
				endConversation = null;
			}
			
			if (interactionSource == InteractionSource.AssetFile && _option.assetFile)
			{
				AdvGame.RunActionListAsset (_option.assetFile, endConversation);
			}
			else if (interactionSource == InteractionSource.CustomScript)
			{
				if (_option.customScriptObject != null && _option.customScriptFunction != "")
				{
					_option.customScriptObject.SendMessage (_option.customScriptFunction);
				}
			}
			else if (interactionSource == InteractionSource.InScene && _option.dialogueOption)
			{
				_option.dialogueOption.conversation = endConversation;
				_option.dialogueOption.Interact ();
			}
			else
			{
				Debug.Log ("No Interaction object found!");
				KickStarter.stateHandler.gameState = GameState.Normal;
			}
		}
		

		/**
		 * Show the Conversation's dialogue options.
		 */
		public void TurnOn ()
		{
			Interact ();
		}
		

		/**
		 * Hides the Conversation's dialogue options.
		 */
		public void TurnOff ()
		{
			if (KickStarter.playerInput)
			{
				CancelInvoke ("RunDefault");
				KickStarter.playerInput.activeConversation = null;
			}
		}
		
		
		private void RunDefault ()
		{
			if (KickStarter.playerInput && KickStarter.playerInput.activeConversation != null && options.Count > defaultOption && defaultOption > -1)
			{
				KickStarter.playerInput.activeConversation = null;
				RunOption (options[defaultOption]);
			}
		}
		
		
		private IEnumerator RunOptionCo (int i)
		{
			yield return new WaitForSeconds (0.3f);
			RunOption (options[i]);
		}
		

		/**
		 * <summary>Runs a dialogue option.</summary>
		 * <param name = "slot">The index number of the dialogue option to run</param>
		 */
		public void RunOption (int slot)
		{
			CancelInvoke ("RunDefault");
			int i = ConvertSlotToOption (slot);
			if (i == -1)
			{
				return;
			}

			if (KickStarter.playerInput)
			{
				KickStarter.playerInput.activeConversation = null;
			}
			
			StartCoroutine (RunOptionCo (i));
		}
		

		/**
		 * <summary>Gets the time remaining before a timed Conversation ends.</summary>
		 * <returns>The time remaining before a timed Conversation ends.</returns>
		 */
		public float GetTimeRemaining ()
		{
			return ((startTime + timer - Time.time) / timer);
		}
		
		
		private int ConvertSlotToOption (int slot)
		{
			int foundSlots = 0;
			for (int j=0; j<options.Count; j++)
			{
				if (options[j].isOn)
				{
					foundSlots ++;
					if (foundSlots == (slot+1))
					{
						return j;
					}
				}
			}
			return -1;
		}
		

		/**
		 * <summary>Gets the display label of a dialogue option.</summary>
		 * <param name = "slot">The index number of the dialogue option to find</param>
		 * <returns>The display label of the dialogue option</returns>
		 */
		public string GetOptionName (int slot)
		{
			int i = ConvertSlotToOption (slot);
			if (i == -1)
			{
				i = 0;
			}
			return (SpeechManager.GetTranslation (options[i].label, options[i].lineID, Options.GetLanguage ()));
		}
		

		/**
		 * <summary>Gets the display icon of a dialogue option.</summary>
		 * <param name = "slot">The index number of the dialogue option to find</param>
		 * <returns>The display icon of the dialogue option</returns>
		 */
		public Texture2D GetOptionIcon (int slot)
		{
			int i = ConvertSlotToOption (slot);
			if (i == -1)
			{
				i = 0;
			}
			return options[i].icon;
		}


		/**
		 * <summary>Turns a dialogue option on, provided that it is unlocked.</summary>
		 * <param name = "id">The ID number of the dialogue option to enable</param>
		 */
		public void TurnOptionOn (int id)
		{
			foreach (ButtonDialog option in options)
			{
				if (option.ID == id)
				{
					if (!option.isLocked)
					{
						option.isOn = true;
					}
					else
					{
						Debug.Log (gameObject.name + "'s option '" + option.label + "' cannot be turned on as it is locked.");
					}
					return;
				}
			}
		}


		/**
		 * <summary>Turns a dialogue option off, provided that it is unlocked.</summary>
		 * <param name = "id">The ID number of the dialogue option to disable</param>
		 */
		public void TurnOptionOff (int id)
		{
			foreach (ButtonDialog option in options)
			{
				if (option.ID == id)
				{
					if (!option.isLocked)
					{
						option.isOn = false;
					}
					else
					{
						Debug.LogWarning (gameObject.name + "'s option '" + option.label + "' cannot be turned off as it is locked.");
					}
					return;
				}
			}
		}


		/**
		 * <summary>Sets the enabled and locked states of a dialogue option, provided that it is unlocked.</summary>
		 * <param name = "id">The ID number of the dialogue option to change</param>
		 * <param name = "flag">The "on/off" state to set the option</param>
		 * <param name = "isLocked">The "locked/unlocked" state to set the option</param>
		 */
		public void SetOptionState (int id, bool flag, bool isLocked)
		{
			foreach (ButtonDialog option in options)
			{
				if (option.ID == id)
				{
					if (!option.isLocked)
					{
						option.isLocked = isLocked;
						option.isOn = flag;
					}
					return;
				}
			}
		}
		

		/**
		 * <summary>Gets the number of enabled dialogue options.</summary>
		 * <returns>The number of enabled dialogue options</summary>
		 */
		public int GetCount ()
		{
			int numberOn = 0;
			foreach (ButtonDialog _option in options)
			{
				if (_option.isOn)
				{
					numberOn ++;
				}
			}
			return numberOn;
		}
		

		/**
		 * <summary>Gets an array of the "on/off" states of all dialogue options.</summary>
		 * <returns>An array of the "on/off" states of all dialogue options.</returns>
		 */
		public bool[] GetOptionStates ()
		{
			List<bool> states = new List<bool>();
			foreach (ButtonDialog _option in options)
			{
				states.Add (_option.isOn);
			}
			return states.ToArray ();
		}
		

		/**
		 * <summary>Gets an array of the "locked/unlocked" states of all dialogue options.</summary>
		 * <returns>An array of the "locked/unlocked" states of all dialogue options.</returns>
		 */
		public bool[] GetOptionLocks ()
		{
			List<bool> locks = new List<bool>();
			foreach (ButtonDialog _option in options)
			{
				locks.Add (_option.isLocked);
			}
			return locks.ToArray ();
		}
		

		/**
		 * <summary>Sets the "on/off" states of all dialogue options.</summary>
		 * <param name = "locks">An array of the "on/off" states of all dialogue options.</param>
		 */
		public void SetOptionStates (bool[] states)
		{
			for (int i=0; i<options.Count; i++)
			{
				if (states.Length > i)
				{
					options[i].isOn = states[i];
				}
			}
		}
		

		/**
		 * <summary>Sets the "locked/unlocked" states of all dialogue options.</summary>
		 * <param name = "locks">An array of the "locked/unlocked" states of all dialogue options.</param>
		 */
		public void SetOptionLocks (bool[] locks)
		{
			for (int i=0; i<options.Count; i++)
			{
				if (locks.Length > i)
				{
					options[i].isLocked = locks[i];
				}
			}
		}


		/**
		 * Upgrades the Conversation from a previous version of Adventure Creator.
		 */
		public void Upgrade ()
		{
			// Set IDs as index + 1 (because default is 0 when not upgraded)
			if (options.Count > 0 && options[0].ID == 0)
			{
				for (int i=0; i<options.Count; i++)
				{
					options[i].ID = i+1;
				}
				#if UNITY_EDITOR
				if (Application.isPlaying)
				{
					Debug.Log ("Conversation '" + gameObject.name + "' has been temporarily upgraded - please view it's Inspector when the game ends and save the scene.");
				}
				else
				{
					UnityEditor.EditorUtility.SetDirty (this);
					if (!this.gameObject.activeInHierarchy)
					{
						// Asset file
						UnityEditor.AssetDatabase.SaveAssets ();
					}
					Debug.Log ("Upgraded Conversation '" + gameObject.name + "', please save the scene.");
				}
				#endif
			}
		}


		/**
		 * <summmary>Gets an array of ID numbers of existing ButtonDialog classes, so that a unique number can be generated.</summary>
		 * <returns>Gets an array of ID numbers of existing ButtonDialog classes</returns>
		 */
		public int[] GetIDArray ()
		{
			List<int> idArray = new List<int>();
			foreach (ButtonDialog option in options)
			{
				idArray.Add (option.ID);
			}
			
			idArray.Sort ();
			return idArray.ToArray ();
		}

	}

}
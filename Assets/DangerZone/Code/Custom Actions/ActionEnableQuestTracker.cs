/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2014
 *	
 *	"ActionTemplate.cs"
 * 
 *	This is a blank action template.
 * 
 */

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionEnableQuestTracker : Action
	{
		
		
		// Declare variables here
		public bool enable;
		
		
		public ActionEnableQuestTracker ()
		{
			this.isDisplayed = true;
			title = "QuestTracker: Enable";
		}
		
		
		override public float Run ()
		{
			/* 
			 * This function is called when the action is performed.
			 * 
			 * The float to return is the time that the game
			 * should wait before moving on to the next action.
			 * Return 0f to make the action instantenous.
			 * 
			 * For actions that take longer than one frame,
			 * you can return "defaultPauseTime" to make the game
			 * re-run this function a short time later. You can
			 * use the isRunning boolean to check if the action is
			 * being run for the first time, eg: 
			 */

			PixelCrushers.DialogueSystem.QuestTracker questTracker 
				= GameObject.FindObjectOfType<PixelCrushers.DialogueSystem.DialogueSystemController> ()
					.GetComponent<PixelCrushers.DialogueSystem.QuestTracker> ();
			if (questTracker != null) {
				questTracker.enabled = enable;
			} else {
				Debug.LogWarning ("No quest tracker attached to game object");
			}
			return 0f;

		}
		
		
		#if UNITY_EDITOR
		
		override public void ShowGUI ()
		{
			enable = EditorGUILayout.Toggle ("Enable Quest Tracker", enable);
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			// Return a string used to describe the specific action's job.
			return (" (QuestTracker enabled - " + enable.ToString () + ")");
		}

		#endif
		
	}

}
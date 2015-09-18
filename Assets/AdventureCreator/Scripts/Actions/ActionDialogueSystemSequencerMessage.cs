using UnityEngine;
using System.Collections;
using AC;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.AdventureCreator;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This custom Adventure Creator sends a message to the Dialogue System sequencer.
/// </summary>
[System.Serializable]
public class ActionDialogueSystemSequencerMessage : Action
{
	
	public int constantID = 0;
	public string message = string.Empty;

	public ActionDialogueSystemSequencerMessage ()
	{
		this.isDisplayed = true;
		category = ActionCategory.ThirdParty;
		title = "Dialogue System Sequencer Message";
		description = "Sends a message to the Dialogue System sequencer.";
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

		Debug.Log("<color=cyan>Sending message to Dialogue System sequencer: " + message + "</color>");
		Sequencer.Message(message);

		return 0;
	}
	
	
	#if UNITY_EDITOR
	
	override public void ShowGUI ()
	{
		// Action-specific Inspector GUI code here
		message = EditorGUILayout.TextField(new GUIContent("Message:", "The message to send to the sequencer"), message);

		AfterRunningOption ();
	}
	
	
	public override string SetLabel ()
	{
		// Return a string used to describe the specific action's job.
		
		string labelAdd = "";
		if (!string.IsNullOrEmpty(message))
		{
			labelAdd = " (" + message + ")";
		}
		return labelAdd;
	}
	
	#endif
	
}

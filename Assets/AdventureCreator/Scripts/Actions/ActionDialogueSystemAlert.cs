using UnityEngine;
using System.Collections;
using AC;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.AdventureCreator;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This custom Adventure Creator action shows a gameplay alert
/// through the Dialogue System's dialogue UI.
/// </summary>
[System.Serializable]
public class ActionDialogueSystemAlert : Action
{
	
	public int constantID = 0;
	public string message = string.Empty;
	public float duration = 5;
	public bool syncData = false;
	
	public ActionDialogueSystemAlert ()
	{
		this.isDisplayed = true;
		category = ActionCategory.ThirdParty;
		title = "Dialogue System Alert";
		description = "Shows an alert message through the Dialogue System.";
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

		var bridge = syncData ? DialogueManager.Instance.GetComponent<AdventureCreatorBridge>() : null;
		if (syncData && (bridge != null)) bridge.SyncAdventureCreatorToLua();
		DialogueManager.ShowAlert(message, duration);

		return 0;
	}
	
	
	#if UNITY_EDITOR
	
	override public void ShowGUI ()
	{
		// Action-specific Inspector GUI code here
		message = EditorGUILayout.TextField(new GUIContent("Message:", "The message to show. Can contain markup tags."), message);
		duration = EditorGUILayout.FloatField(new GUIContent("Duration:", "The duration in seconds to show the message."), duration);
		syncData = EditorGUILayout.Toggle(new GUIContent("Sync Data:", "Synchronize AC data with Lua environment"), syncData);

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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AC;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.AdventureCreator;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This custom Adventure Creator action plays a Dialogue System conversation.
/// </summary>
[System.Serializable]
public class ActionDialogueSystemConversation : Action
{

	public int constantID = 0;
	public string conversation = string.Empty;
	public Transform actor = null;
	public Transform conversant = null;
	public bool stopConversationOnSkip = true;

	public ActionDialogueSystemConversation ()
	{
		this.isDisplayed = true;
		category = ActionCategory.ThirdParty;
		title = "Dialogue System Conversation";
		description = "Starts a Dialogue System conversation. Leave Actor blank to default to the player.";
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

		if (!isRunning)
		{
			// Sync AC data to Lua:
			AdventureCreatorBridge bridge = DialogueManager.Instance.GetComponent<AdventureCreatorBridge>();
			if (bridge != null) {
				bridge.SyncAdventureCreatorToLua();
				bridge.skipSyncToLua = true;
			}

			// If the actor is null, use the player's transform:
			if (actor == null) {
				GameObject actorGameObject = GameObject.FindGameObjectWithTag("Player");
				if (actorGameObject != null) actor = actorGameObject.transform;
			}

			// Start the conversation:
			DialogueManager.StartConversation(conversation, actor, conversant);

			isRunning = true;
			return defaultPauseTime;
		}
		else
		{
			isRunning = DialogueManager.IsConversationActive;
			return isRunning ? defaultPauseTime : 0;
		}
	}

	override public void Skip ()
	{
		if (stopConversationOnSkip) DialogueManager.StopConversation();
	}



	#if UNITY_EDITOR

	public ActionConversationPicker conversationPicker = null;
	public DialogueDatabase selectedDatabase = null;
	public bool usePicker = true;

	override public void ShowGUI ()
	{
		// Action-specific Inspector GUI code here
		if (conversationPicker == null)
		{
			conversationPicker = new ActionConversationPicker(selectedDatabase, conversation, usePicker);
		}
		conversationPicker.Draw();
		conversation = conversationPicker.currentConversation;
		//---Was: conversation = EditorGUILayout.TextField(new GUIContent("Conversation Title:", "The title defined in the dialogue database"), conversation);
		actor = (Transform) EditorGUILayout.ObjectField(new GUIContent("Actor:", "The primary speaker, usually the player."), actor, typeof(Transform), true);
		conversant = (Transform) EditorGUILayout.ObjectField(new GUIContent("Conversant:", "The other speaker, usually an NPC"), conversant, typeof(Transform), true);
		stopConversationOnSkip = EditorGUILayout.Toggle(new GUIContent("Stop On Skip:", "Stop the conversation if the player skips the cutscene"), stopConversationOnSkip);

		AfterRunningOption ();
	}
	
	
	public override string SetLabel ()
	{
		// Return a string used to describe the specific action's job.
		
		string labelAdd = "";
		if (!string.IsNullOrEmpty(conversation))
		{
			labelAdd = " (" + conversation + ")";
		}
		return labelAdd;
	}
	
	#endif
	
}

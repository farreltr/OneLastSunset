using UnityEngine;
using System.Collections;
using AC;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.AdventureCreator;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This custom Adventure Creator action plays a Dialogue System bark.
/// The actor should have a bark UI component.
/// </summary>
[System.Serializable]
public class ActionDialogueSystemBark : Action
{
	
	public int constantID = 0;
	public string conversation = string.Empty;
	public Transform actor = null;
	public Transform conversant = null;
	public bool syncData = false;
	
	public ActionDialogueSystemBark ()
	{
		this.isDisplayed = true;
		category = ActionCategory.ThirdParty;
		title = "Dialogue System Bark";
		description = "Plays a Dialogue System bark. Leave Actor blank to default to the player (the target of the bark).";
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

		if (actor == null) {
			GameObject actorGameObject = GameObject.FindGameObjectWithTag("Player");
			if (actorGameObject != null) actor = actorGameObject.transform;
		}
		AdventureCreatorBridge bridge = DialogueManager.Instance.GetComponent<AdventureCreatorBridge>();
		if (syncData && (bridge != null)) bridge.SyncAdventureCreatorToLua();
		DialogueManager.Bark(conversation, actor, conversant);
		if (syncData && (bridge != null)) bridge.SyncAdventureCreatorToLua();

		return 0;
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
		actor = (Transform) EditorGUILayout.ObjectField(new GUIContent("Actor:", "The primary speaker, usually the player"), actor, typeof(Transform), true);
		conversant = (Transform) EditorGUILayout.ObjectField(new GUIContent("Conversant:", "The other speaker, usually an NPC"), conversant, typeof(Transform), true);
		syncData = EditorGUILayout.Toggle(new GUIContent("Sync Data:", "Synchronize AC data with Lua environment"), syncData);

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

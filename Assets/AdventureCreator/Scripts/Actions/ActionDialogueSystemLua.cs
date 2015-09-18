using UnityEngine;
using System.Collections;
using AC;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.AdventureCreator;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// This custom Adventure Creator action runs Lua code in the 
/// Dialogue System's Lua environment.
/// </summary>
[System.Serializable]
public class ActionDialogueSystemLua : Action
{
	
	public int constantID = 0;
	public string luaCode = string.Empty;
	public bool syncData = false;
	public bool updateQuestTracker = false;
	public bool storeResult = false;
	public int variableID = 0;

	public ActionDialogueSystemLua ()
	{
		this.isDisplayed = true;
		category = ActionCategory.ThirdParty;
		title = "Dialogue System Lua";
		description = "Runs Lua code in the Dialogue System's Lua environment.";
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

		AdventureCreatorBridge bridge = DialogueManager.Instance.GetComponent<AdventureCreatorBridge>();
		if (syncData && (bridge != null)) bridge.SyncAdventureCreatorToLua();
		Lua.Result luaResult = Lua.NoResult;
		if (!string.IsNullOrEmpty(luaCode)) 
		{
			luaResult = Lua.Run(luaCode, DialogueDebug.LogInfo);
		}
		if (syncData && (bridge != null)) bridge.SyncLuaToAdventureCreator();
		if (updateQuestTracker) DialogueManager.Instance.SendMessage("UpdateTracker", SendMessageOptions.DontRequireReceiver);
		if (storeResult)
		{
			AC.GlobalVariables.SetStringValue(variableID, luaResult.AsString);
		}
		return 0;
	}
	
	
	#if UNITY_EDITOR
	
	override public void ShowGUI ()
	{
		// Action-specific Inspector GUI code here
		luaCode = EditorGUILayout.TextField(new GUIContent("Lua Code:", "The Lua code to run"), luaCode);
		syncData = EditorGUILayout.Toggle(new GUIContent("Sync Data:", "Synchronize AC data with Lua environment"), syncData);
		updateQuestTracker = EditorGUILayout.Toggle(new GUIContent("Update Quest Tracker:", "Update quest tracker after running"), updateQuestTracker);
		storeResult = EditorGUILayout.Toggle(new GUIContent("Store Result?", "Store the result in a string variable"), storeResult);
		if (storeResult)
		{
			variableID = EditorGUILayout.IntField(new GUIContent("Variable ID", "ID of a global string variable to hold the result"), variableID);
		}

		AfterRunningOption ();
	}
	
	
	public override string SetLabel ()
	{
		// Return a string used to describe the specific action's job.
		
		string labelAdd = "";
		if (!string.IsNullOrEmpty(luaCode))
		{
			labelAdd = " (" + luaCode + ")";
		}
		return labelAdd;
	}
	
	#endif
	
}

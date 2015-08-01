using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AC;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.AdventureCreator {

	/// <summary>
	/// This component synchronizes Adventure Creator data with Dialogue System data. 
	/// Add it to your Dialogue Manager object. It synchronizes AC's variables with
	/// the Dialogue System's Variable[] Lua table, and AC's inventory with the Dialogue
	/// System's Item[] Lua table. 
	/// 
	/// It also provides methods to save and load the Dialogue System's state to
	/// an AC global variable. You can call these methods when saving and loading games
	/// in AC.
	/// </summary>
	[AddComponentMenu("Dialogue System/Third Party/Adventure Creator/Adventure Creator Bridge")]
	public class AdventureCreatorBridge : MonoBehaviour {

		/// <summary>
		/// The name of the AC global variable used to store Dialogue System state.
		/// </summary>
		public static string DialogueSystemGlobalVariableName = "DialogueSystemEnvironment";

		public enum UseDialogState { Never, IfPlayerInvolved, AfterStopIfPlayerInvolved, Always }

		/// <summary>
		/// The AC GameState to use when in conversations.
		/// </summary>
		public UseDialogState useDialogState = UseDialogState.IfPlayerInvolved;

		/// <summary>
		/// Specifies when conversations should take camera control.
		/// </summary>
		public UseDialogState takeCameraControl = UseDialogState.IfPlayerInvolved;

		/// <summary>
		/// The max time to wait for the camera stop if takeCameraControl is AfterStopIfPlayerInvolved.
		/// </summary>
		public float maxTimeToWaitForCameraStop = 10f;

		/// <summary>
		/// Set <c>true</c> to include dialogue entry status (offered and/or spoken) in save data.
		/// </summary>
		public bool includeSimStatus = false;

		/// <summary>
		/// Set <c>true</c> to save the Lua environment to the AC global variable when
		/// conversations end.
		/// </summary>
		public bool saveToGlobalVariableOnConversationEnd = false;

		/// <summary>
		/// Set this <c>true</c> to skip the next sync to Lua. The Conversation action sets
		/// this <c>true</c> because it manually syncs to Lua before the conversation starts.
		/// </summary>
		/// <value><c>true</c> to skip sync to lua; otherwise, <c>false</c>.</value>
		public bool skipSyncToLua { get; set; }

		private bool isPlayerInvolved = false;
		private GameState previousGameState = GameState.Normal;
		private CursorDisplay previousCursorDisplay = CursorDisplay.Always;

		private const float MovementThreshold = 0.1f; // Camera is "stopped" if it moves less than 0.1 units in 0.5 seconds.

		public virtual void Start() {
			PersistentDataManager.includeSimStatus = includeSimStatus;
			skipSyncToLua = false;
		}

		/// <summary>
		/// Prepares to run a conversation by freezing AC and syncing data to Lua.
		/// </summary>
		/// <param name="actor">The other actor.</param>
		public virtual void OnConversationStart(Transform actor) {
			CheckIfPlayerIsInvolved(actor);
			if (!skipSyncToLua) SyncAdventureCreatorToLua();
			skipSyncToLua = false;
			SetConversationGameState();
		}

		/// <summary>
		/// At the end of a conversation, unfreezes AC and syncs Lua back to AC.
		/// </summary>
		/// <param name="actor">Actor.</param>
		public virtual void OnConversationEnd(Transform actor) {
			UnsetConversationGameState();
			SyncLuaToAdventureCreator();
			if (saveToGlobalVariableOnConversationEnd) SaveDialogueSystemToGlobalVariable();
		}

		private void CheckIfPlayerIsInvolved(Transform actor) {
			Actor dbActor = DialogueManager.MasterDatabase.GetActor(OverrideActorName.GetActorName(actor));
			isPlayerInvolved = (dbActor != null) && dbActor.IsPlayer;
		}

		public static GameObject GetGameEngine() { 
			return GameObject.FindWithTag(Tags.gameEngine); 
		}

		public static GameObject GetPersistentEngine() {
			return GameObject.FindWithTag(Tags.persistentEngine);
		}

		public static StateHandler GetStateHandler() { 
			var persistentEngine = GetPersistentEngine();
			return (persistentEngine != null) ? persistentEngine.GetComponent<StateHandler>() : null;
		}

		public static InventoryManager GetInventoryManager() {
			return AdvGame.GetReferences().inventoryManager;
		}

		public static RuntimeInventory GetRuntimeInventory() {
			var persistentEngine = GetPersistentEngine();
			return (persistentEngine != null) ? persistentEngine.GetComponent<RuntimeInventory>() : null;
		}

		public static LocalVariables GetLocalVariables() { 
			var gameEngine = GetGameEngine();
			return (gameEngine != null) ? gameEngine.GetComponent<LocalVariables>() : null;
		}

		public static VariablesManager GetVariablesManager() {
			return AdvGame.GetReferences().variablesManager;
		}

		public static CursorManager GetCursorManager() {
			return AdvGame.GetReferences().cursorManager;
		}

		/// <summary>
		/// Sets GameState to DialogOptions if specified in the properties.
		/// </summary>
		public virtual void SetConversationGameState() {
			switch (useDialogState) {
			case UseDialogState.Never:
				break;
			case UseDialogState.IfPlayerInvolved:
			case UseDialogState.AfterStopIfPlayerInvolved:
				if (isPlayerInvolved) SetGameStateToCutscene();
				break;
			case UseDialogState.Always:
				SetGameStateToCutscene();
				break;
			}
			switch (takeCameraControl) {
			case UseDialogState.Never:
				break;
			case UseDialogState.IfPlayerInvolved:
				if (isPlayerInvolved) DisableACCameraControl();
				break;
			case UseDialogState.AfterStopIfPlayerInvolved:
				if (isPlayerInvolved) IdleACCameraControl();
				break;
			case UseDialogState.Always:
				DisableACCameraControl();
				break;
			}
		}

		/// <summary>
		/// Restores the previous GameState if necessary.
		/// </summary>
		public virtual void UnsetConversationGameState() {
			switch (useDialogState) {
			case UseDialogState.Never:
				break;
			case UseDialogState.IfPlayerInvolved:
				if (isPlayerInvolved) RestorePreviousGameState();
				break;
			case UseDialogState.Always:
				RestorePreviousGameState();
				break;
			}
			switch (useDialogState) {
			case UseDialogState.Never:
				break;
			case UseDialogState.IfPlayerInvolved:
				if (isPlayerInvolved) EnableACCameraControl();
				break;
			case UseDialogState.Always:
				EnableACCameraControl();
				break;
			}
		}

		/// <summary>
		/// Sets AC's GameState to DialogOptions.
		/// </summary>
		public void SetGameStateToCutscene() {
			var stateHandler = GetStateHandler();
			if (stateHandler == null) return;
			previousGameState = (stateHandler.gameState == GameState.DialogOptions) ? GameState.Normal : stateHandler.gameState;
			stateHandler.gameState = GameState.DialogOptions;
			if (!DialogueManager.IsConversationActive) SetConversationCursor();

		}

		public void RestorePreviousGameState() {
			var stateHandler = GetStateHandler();
			if (stateHandler == null) return;
			stateHandler.gameState = (previousGameState == GameState.DialogOptions) ? GameState.Normal : previousGameState;
			RestorePreviousCursor();
		}

		public void DisableACCameraControl() {
			var stateHandler = GetStateHandler();
			if (stateHandler == null) return;
			stateHandler.cameraIsOff = true;
		}

		public void EnableACCameraControl() {
			var stateHandler = GetStateHandler();
			if (stateHandler == null) return;
			stateHandler.cameraIsOff = false;
		}

		public void IdleACCameraControl() {
			StartCoroutine(WaitForCameraToStop());
		}

		private IEnumerator WaitForCameraToStop() {
			var cam = Camera.main;
			if (cam == null) yield break;
			var maxTime = Time.time + maxTimeToWaitForCameraStop;
			var lastPosition = cam.transform.position;
			while ((Vector3.Distance(cam.transform.position, lastPosition) < MovementThreshold) && (Time.time < maxTime)) {
				lastPosition = cam.transform.position;
				yield return new WaitForSeconds(0.5f);

			}
			DisableACCameraControl();
		}

		/// <summary>
		/// Sets the conversation cursor.
		/// </summary>
		public void SetConversationCursor() {
			var cursorManager = GetCursorManager();
			if (cursorManager == null) return;
			previousCursorDisplay = cursorManager.cursorDisplay;
			cursorManager.cursorDisplay = CursorDisplay.Always;
		}

		/// <summary>
		/// Restores the previous cursor.
		/// </summary>
		public void RestorePreviousCursor() {
			var cursorManager = GetCursorManager();
			if (cursorManager == null) return;
			cursorManager.cursorDisplay = previousCursorDisplay;
		}

		/// <summary>
		/// Syncs the AC data to Lua.
		/// </summary>
		public virtual void SyncAdventureCreatorToLua() {
			SyncVariablesToLua();
			SyncInventoryToLua();
		}

		/// <summary>
		/// Syncs Lua back to AC data.
		/// </summary>
		public virtual void SyncLuaToAdventureCreator() {
			SyncLuaToVariables();
			SyncLuaToInventory();
		}

		/// <summary>
		/// Syncs global and local variables to Lua.
		/// </summary>
		public void SyncVariablesToLua() {
			SyncVarListToLua(GlobalVariables.GetAllVars());
			var localVariables = GetLocalVariables();
			if (localVariables != null) SyncVarListToLua(localVariables.localVars);
		}

		/// <summary>
		/// Syncs Lua back to AC's global and local variables.
		/// </summary>
		public void SyncLuaToVariables() {
			SyncLuaToVarList(GlobalVariables.GetAllVars());
			var localVariables = GetLocalVariables();
			if (localVariables != null) SyncLuaToVarList(localVariables.localVars);
		}

		/// <summary>
		/// Syncs a variable list to Lua (used for global and local variables).
		/// </summary>
		/// <param name="varList">Variable list.</param>
		protected void SyncVarListToLua(List<GVar> varList) {
			foreach (var variable in varList) {
				if (!string.Equals(variable.label, DialogueSystemGlobalVariableName)) {
					string luaName = DialogueLua.StringToTableIndex(variable.label);
					switch (variable.type) {
					case VariableType.Boolean:
						bool boolValue = (variable.val != 0);
						DialogueLua.SetVariable(luaName, boolValue);
						break;
					case VariableType.Integer:
						DialogueLua.SetVariable(luaName, variable.val);
						break;
					default:
					case VariableType.String:
						DialogueLua.SetVariable(luaName, variable.textVal);
						break;
					}
				}
			}
		}

		/// <summary>
		/// Syncs Lua to a variable list (used for global and local variables).
		/// </summary>
		/// <param name="varList">Variable list.</param>
		protected void SyncLuaToVarList(List<GVar> varList) {
			foreach (var variable in varList) {
				string luaName = DialogueLua.StringToTableIndex(variable.label);
				var luaValue = DialogueLua.GetVariable(luaName);
				switch (variable.type) {
				case VariableType.Boolean:
					variable.val = (luaValue.AsBool == true) ? 1 : 0;
					break;
				case VariableType.Integer:
					variable.val = luaValue.AsInt;
					break;
				default:
				case VariableType.String:
					variable.textVal = luaValue.AsString;
					break;
				}
			}
		}

		/// <summary>
		/// Syncs AC's inventory to Lua.
		/// </summary>
		public void SyncInventoryToLua() {
			var inventoryManager = GetInventoryManager();
			var runtimeInventory = GetRuntimeInventory();
			if (inventoryManager == null || runtimeInventory == null) return;
			foreach (InvItem item in inventoryManager.items) {
				string luaName = DialogueLua.StringToTableIndex(item.label);
				InvItem runtimeItem = runtimeInventory.localItems.Find(x => x.id == item.id);
				int runtimeCount = (runtimeItem != null) ? runtimeItem.count : 0;
				Lua.Run(string.Format("Item[\"{0}\"] = {{ Name=\"{1}\", Description=\"\", Is_Item=true, AC_ID={2}, Count={3} }}", 
				                      luaName, item.label, item.id, runtimeCount), DialogueDebug.LogInfo);
			}
		}

		/// <summary>
		/// Syncs Lua to AC's inventory.
		/// </summary>
		public void SyncLuaToInventory() {
			LuaTableWrapper luaItemTable = Lua.Run("return Item").AsTable;
			if (luaItemTable == null) return;
			foreach (var luaItem in luaItemTable.Values) {
				LuaTableWrapper fields = luaItem as LuaTableWrapper;
				if (fields != null) {
					foreach (var fieldNameObject in fields.Keys) {
						string fieldName = fieldNameObject as string;
						if (string.Equals(fieldName, "AC_ID")) {
							try {
								// Get Name:
								object o = fields["Name"];
								bool valid = (o != null) && (o.GetType() == typeof(string));
								string itemName = valid ? (string) fields["Name"] : string.Empty;

								// Get AC_ID:
								o = fields["AC_ID"];
								valid = valid && (o != null) && (o.GetType() == typeof(double) || o.GetType() == typeof(float));
								double value = (o.GetType() == typeof(double)) ? (double) fields["AC_ID"] : (float) fields["AC_ID"];
								int itemID = valid ? ((int) value) : 0;

								// Get Count:
								o = fields["Count"];
								valid = valid && (o != null) && (o.GetType() == typeof(double) || o.GetType() == typeof(float));
								value = (o.GetType() == typeof(double)) ? (double) fields["Count"] : (float) fields["Count"];
								int newCount = valid ? ((int) value) : 0;

								if (valid) UpdateAdventureCreatorItem(itemName, itemID, newCount);
							} catch (System.Exception e) {
								Debug.LogError(e.Message);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Updates the count of an item in AC's inventory.
		/// </summary>
		/// <param name="itemName">Item name.</param>
		/// <param name="itemID">Item ID.</param>
		/// <param name="newCount">New count.</param>
		protected void UpdateAdventureCreatorItem(string itemName, int itemID, int newCount) {
			var runtimeInventory = GetRuntimeInventory();
			if (runtimeInventory == null) return;
			InvItem item = runtimeInventory.localItems.Find(x => x.id == itemID);
			if (item == null) {
				if (newCount > 0) {
					runtimeInventory.Add(itemID, newCount, false, KickStarter.player.ID);
					if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Added {1} {2} to inventory", DialogueDebug.Prefix, newCount, itemName));
				}
			} else if (newCount > item.count) {
				int amountToAdd = newCount - item.count;
				runtimeInventory.Add(item.id, amountToAdd, false, KickStarter.player.ID);
				if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Added {1} {2} to inventory", DialogueDebug.Prefix, amountToAdd, itemName));
			} else if (newCount < item.count ) {
				int amountToRemove = item.count - newCount;
				runtimeInventory.Remove(item.id, amountToRemove, true, KickStarter.player.ID);
				if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Removed {1} {2} from inventory", DialogueDebug.Prefix, amountToRemove, itemName));
			}
		}

		/// <summary>
		/// Saves the Dialogue System state to a dedicated AC global variable. This method
		/// will create the global variable if it doesn't already exist.
		/// </summary>
		public static void SaveDialogueSystemToGlobalVariable() {
			GlobalVariables.SetStringValue(GetDialogueSystemVarID(), PersistentDataManager.GetSaveData());
		}
			
		/// <summary>
		/// Loads the Dialogue System state from a dedicated AC global variable.
		/// </summary>
		public static void LoadDialogueSystemFromGlobalVariable() {
			PersistentDataManager.ApplySaveData(GlobalVariables.GetStringValue(GetDialogueSystemVarID()));
		}

		/// <summary>
		/// Gets the ID of the DialogueSystemEnvironment AC variable. If the variable hasn't been defined
		/// in AC yet, this method also creates the variable.
		/// </summary>
		/// <returns>The DialogueSystemEnvironment variable ID.</returns>
		private static int GetDialogueSystemVarID() {
			var variablesManager = GetVariablesManager();
			if (variablesManager == null) return 0;
			List<GVar> globalVarList = GlobalVariables.GetAllVars();
			foreach (GVar var in globalVarList) {
				if (string.Equals(var.label, DialogueSystemGlobalVariableName)) return var.id;
			}
			GVar newVar = new GVar(GetVarIDArray(variablesManager));
			newVar.label = DialogueSystemGlobalVariableName;
			newVar.type = VariableType.String;
			variablesManager.vars.Add(newVar);
			globalVarList.Add(newVar);
			return newVar.id;
		}

		/// <summary>
		/// Gets the variable ID array. To add a new variable, AC needs a reference to the 
		/// current IDs. This generates the list of current IDs.
		/// </summary>
		/// <returns>The variable ID array.</returns>
		/// <param name="variablesManager">Variables manager.</param>
		private static int[] GetVarIDArray(VariablesManager variablesManager) {
			List<int> idArray = new List<int>();
			foreach (GVar var in GlobalVariables.GetAllVars()) {
				idArray.Add(var.id);
			}
			idArray.Sort();
			return idArray.ToArray();
		}

	}

}

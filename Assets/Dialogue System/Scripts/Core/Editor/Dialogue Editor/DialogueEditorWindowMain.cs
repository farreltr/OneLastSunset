using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PixelCrushers.DialogueSystem.DialogueEditor {

	/// <summary>
	/// Dialogue database editor window. This is the main part of a class that spans 
	/// several files using partial classes. Each file handles one aspect of the 
	/// Dialogue Editor, such as the Actors tab or the Items tab.
	/// </summary>
	public partial class DialogueEditorWindow : EditorWindow {

		public static DialogueEditorWindow instance = null;

		public static object inspectorSelection {
			get {
				return _inspectorSelection;
			}
			set {
				_inspectorSelection = value;
				if ((value != null) && (instance != null)) Selection.activeObject = DialogueEditorWindow.database;
				if (DialogueDatabaseEditor.instance != null) DialogueDatabaseEditor.instance.Repaint();
			}
		}
		private static object _inspectorSelection = null;

		[SerializeField]
		private Toolbar toolbar;

		private Template template = Template.FromEditorPrefs();

		private static DialogueDatabase database;

		[SerializeField]
		private int databaseInstanceID;

		private static Vector2 scrollPosition = Vector2.zero;

		private const string ShowNodeEditorKey = "PixelCrushers.DialogueSystem.DialogueEditor.ShowNodeEditor";
		private const string ConversationIDKey = "PixelCrushers.DialogueSystem.DialogueEditor.ConversationID";
		private const string ConversationIndexKey = "PixelCrushers.DialogueSystem.DialogueEditor.ConversationIndex";

		private bool internalDebug = false;

		void OnEnable() {
			if (internalDebug) Debug.Log ("<color=lime>Enable</color>" + " savedInstanceID=" + databaseInstanceID);

			minSize = new Vector2(720, 240);
			CheckDatabase();
			if (toolbar == null) toolbar = new Toolbar();
			toolbar.UpdateTabNames(template.treatItemsAsQuests);
			if (database != null && conversationIndex >= database.conversations.Count) conversationIndex = -1;
			instance = this;
			EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
		}

		void OnDisable() {
			if (internalDebug) Debug.Log ("<color=red>Disable</color>" + " savedInstanceID=" + databaseInstanceID);

			EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
			template.SaveToEditorPrefs();
			inspectorSelection = null;
			instance = null;
		}

		void OnSelectionChange() {
			CheckDatabase();
		}

		void OnPlaymodeStateChanged() {
			if (internalDebug) Debug.Log ("<color=cyan>PlaymodeChanged, isPlaying=" + EditorApplication.isPlaying + "</color>" + " savedInstanceID=" + databaseInstanceID);

			SetDatabaseDirty();

			if (EditorPrefs.HasKey(ShowNodeEditorKey) && EditorPrefs.GetBool(ShowNodeEditorKey) && (database != null)) {
				ActivateNodeEditorMode();
			}
			if ((database != null) && EditorPrefs.HasKey(ConversationIDKey)) {
				var conversationID = EditorPrefs.GetInt(ConversationIDKey);
				var conv = database.GetConversation(conversationID);
				if (conv != null) {
					OpenConversation(conv);
					if (showNodeEditor) InitializeDialogueTree();
					if (EditorPrefs.HasKey(ConversationIndexKey)) {
						conversationIndex = EditorPrefs.GetInt(ConversationIndexKey);
					}
				}
			}

			toolbar.UpdateTabNames(template.treatItemsAsQuests);
			currentConversationState = null;
			currentRuntimeEntry = null;
			Repaint();
		}

		void Update() {
			if (Application.isPlaying && (toolbar != null)) {
				switch (toolbar.Current) {
				case Toolbar.Tab.Conversations:
					UpdateRuntimeConversationsTab();
					break;
				case Toolbar.Tab.Templates:
					UpdateRuntimeWatchesTab();
					break;
				}
			}
		}
		
		public void OnGUI() {
			DrawDatabaseName();
			DrawToolbar();
			DrawMainBody();
		}

		private void CheckDatabase() {
			if (internalDebug) Debug.Log ("CheckDatabase, selection=" + Selection.activeObject + ", savedInstanceID=" + databaseInstanceID);
			if (Selection.activeObject == null) return;
			DialogueDatabase newDatabase = Selection.activeObject as DialogueDatabase;
			if ((newDatabase == null) && (database == null)) {
				newDatabase = EditorUtility.InstanceIDToObject(databaseInstanceID) as DialogueDatabase;
			}
			if ((newDatabase != null) && (newDatabase != database)) {

				if (database != null) {
					EditorPrefs.DeleteKey(ConversationIDKey);
				}

				//---Unused: bool skipReset = false;
				database = newDatabase;
				databaseInstanceID = database.GetInstanceID();
				if (internalDebug) Debug.Log ("CheckDatabase - set savedInstanceID to " + databaseInstanceID);
				database.SyncAll();
				Reset(); //if (!skipReset) Reset();
				Repaint();
			}
		}

		private void Reset() {
			ResetActorSection();
			ResetItemSection();
			ResetLocationSection();
			ResetVariableSection();
			ResetConversationSection();
			ResetConversationNodeEditor();
			ResetAssetLists();
			ResetLanguageList();
		}

		private GUIStyle databaseNameStyle = null;
		private Color proDatabaseNameColor = new Color(1, 1, 1, 0.2f);
		private Color freeDatabaseNameColor = new Color(0, 0, 0, 0.2f);

		private void DrawDatabaseName() {
			if (database == null) return;
			if (IsSearchBarVisible || IsLuaWatchBarVisible) return;
			if (databaseNameStyle == null) {
				databaseNameStyle = new GUIStyle(EditorStyles.label);
				databaseNameStyle.fontSize = 20;
				databaseNameStyle.fontStyle = FontStyle.Bold;
				databaseNameStyle.normal.textColor = EditorGUIUtility.isProSkin
					? proDatabaseNameColor
						: freeDatabaseNameColor;
			}
			EditorGUI.LabelField(new Rect(4, position.height - 30, position.width, 30), database.name, databaseNameStyle);
		}

		private void DrawToolbar() {
			Toolbar.Tab previous = toolbar.Current;
			toolbar.Draw();
			if (toolbar.Current != previous) {
				ResetAssetLists();
				ResetDialogueTreeGUIStyles();
				ResetLanguageList();
				if (toolbar.Current == Toolbar.Tab.Items) {
					BuildLanguageListFromItems();
				} else if (!((toolbar.Current == Toolbar.Tab.Conversations) && showNodeEditor)) {
					inspectorSelection = null;
				}
			}
		}

		private void DrawMainBody() {
			if ((database != null) || (toolbar.Current == Toolbar.Tab.Templates)) {
				DrawCurrentSection();
			} else {
				DrawNoDatabaseSection();
			}
		}
		
		private void DrawCurrentSection() {
			EditorStyles.textField.wordWrap = true;
			StartUndoSnapshot();
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
			try {
				switch (toolbar.Current) {
				case Toolbar.Tab.Database:
					DrawDatabaseSection();
					break;
				case Toolbar.Tab.Actors:
					DrawActorSection();
					break;
				case Toolbar.Tab.Items:
					DrawItemSection();
					break;
				case Toolbar.Tab.Locations:
					DrawLocationSection();
					break;
				case Toolbar.Tab.Variables:
					DrawVariableSection();
					break;
				case Toolbar.Tab.Conversations:
					DrawConversationSection();
					break;
				case Toolbar.Tab.Templates:
					if (Application.isPlaying) {
						DrawWatchSection();
					} else {
						DrawTemplateSection();
					}
					break;
				}
			} finally {
				EditorGUILayout.EndScrollView();
			}
			if (IsSearchBarVisible) DrawDialogueTreeSearchBar();
			if (IsLuaWatchBarVisible) DrawLuaWatchBar();
			EndUndoSnapshot();
		}

		private void StartUndoSnapshot() {
			if (database != null) Undo.RecordObject(database, "Dialogue Database");
		}
		
		private void EndUndoSnapshot() {
			if (GUI.changed) SetDatabaseDirty();
		}

		private void SetDatabaseDirty() {
			if (database != null) EditorUtility.SetDirty(database);
		}

	}

}
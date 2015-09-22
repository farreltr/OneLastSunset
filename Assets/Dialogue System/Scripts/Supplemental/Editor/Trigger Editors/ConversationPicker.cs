using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace PixelCrushers.DialogueSystem {

	public class ConversationPicker {

		public DialogueDatabase database = null;
		public string currentConversation = string.Empty;
		public bool usePicker = false;

		private string[] titles = null;
		private int currentIndex = -1;

		private DialogueDatabase initialDatabase = null;

		public ConversationPicker(DialogueDatabase database, string currentConversation, bool usePicker) {
			initialDatabase = EditorTools.FindInitialDatabase();
			this.database = database ?? initialDatabase;
			this.currentConversation = currentConversation;
			this.usePicker = usePicker;
			UpdateTitles();
			bool currentConversationIsInDatabase = (database != null) || (currentIndex >= 0);
			if (usePicker && !string.IsNullOrEmpty(currentConversation) && !currentConversationIsInDatabase) {
				this.usePicker = false;
			}
		}

		public void UpdateTitles() {
			currentIndex = -1;
			if (database == null || database.conversations == null) {
				titles = new string[0];
			} else {
				titles = new string[database.conversations.Count];
				for (int i = 0; i < database.conversations.Count; i++) {
					titles[i] = database.conversations[i].Title;
					if (string.Equals(currentConversation, titles[i])) {
						currentIndex = i;
					}
				}
			}
		}

		public bool Draw() {
			bool changed = false;
			EditorGUILayout.BeginHorizontal();
			if (usePicker) {
				var newDatabase = EditorGUILayout.ObjectField("Reference Database", database, typeof(DialogueDatabase), false) as DialogueDatabase;
				if (newDatabase != database) {
					database = newDatabase;
					UpdateTitles();
				}
			} else {
				var newConversation = EditorGUILayout.TextField("Conversation", currentConversation);
				if (newConversation != currentConversation) {
					changed = true;
					currentConversation = newConversation;
				}
			}
			var newToggleValue = EditorGUILayout.Toggle(usePicker, EditorStyles.radioButton, GUILayout.Width(20));
			if (newToggleValue != usePicker) {
				usePicker = newToggleValue;
				if (usePicker && (database == null)) database = EditorTools.FindInitialDatabase();
				UpdateTitles();
			}
			EditorGUILayout.EndHorizontal();
			if (usePicker) {
				var newIndex = EditorGUILayout.Popup("Conversation", currentIndex, titles);
				if ((newIndex != currentIndex) && (0 <= newIndex) && (newIndex < titles.Length)) {
					changed = true;
					currentIndex = newIndex;
					currentConversation = titles[currentIndex];
				}
				if (database != initialDatabase && database != null && initialDatabase != null) {
					EditorGUILayout.HelpBox("The Dialogue Manager's Initial Database is " + initialDatabase.name + 
					                        ". Make sure to load " + this.database.name + 
					                        " before using this conversation. You can use the Extra Databases component to load additional databases.", 
					                        MessageType.Info);
				}
			}
			return changed;
		}

	}

}

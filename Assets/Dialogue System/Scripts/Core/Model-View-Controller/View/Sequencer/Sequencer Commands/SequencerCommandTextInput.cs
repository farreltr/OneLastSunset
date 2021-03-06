using UnityEngine;

namespace PixelCrushers.DialogueSystem.SequencerCommands {
	
	/// <summary>
	/// Implements sequencer command: TextInput(textFieldUI, label, luaVariableName[, maxLength[, clear]]).
	/// 
	/// - textFieldUI: the name of GameObject with an ITextFieldUI.
	/// - label: the label text, or var=varName to use a variable value as the label.
	/// - luaVariableName: where to store the input
	/// - maxLength: max length of input to accept
	/// - clear: optional; specifies to start with an empty string instead of variable value.
	/// </summary>
	public class SequencerCommandTextInput : SequencerCommand {
		
		private ITextFieldUI textFieldUI = null;
		private string variableName = string.Empty;
		private bool acceptedText = false;

		/// <summary>
		/// Start the sequence and its corresponding text field UI.
		/// </summary>
		public void Start() {
			Transform textFieldUIObject = GetSubject(0);
			if (textFieldUIObject != null) {
				bool currentlyActive = textFieldUIObject.gameObject.activeSelf;
				if (!currentlyActive) textFieldUIObject.gameObject.SetActive(true);
				textFieldUI = textFieldUIObject.GetComponent(typeof(ITextFieldUI)) as ITextFieldUI;
				if (!currentlyActive) textFieldUIObject.gameObject.SetActive(false);
			}
			string labelText = GetParameter(1);
			variableName = GetParameter(2);
			int maxLength = GetParameterAsInt(3);
			bool clearField = string.Equals(GetParameter(4), "clear");
			if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Sequencer: TextInput({1}, {2}, {3}, {4})", new System.Object[] { DialogueDebug.Prefix, Tools.GetObjectName(textFieldUIObject), labelText, variableName, maxLength }));
			if (textFieldUI != null) {
				if (labelText.StartsWith("var=")) {
					labelText = DialogueLua.GetVariable(labelText.Substring(4)).AsString;
				}
				string variableValue = clearField ? string.Empty : DialogueLua.GetVariable(variableName).AsString;
				textFieldUI.StartTextInput(labelText, variableValue, maxLength, OnAcceptedText);
			} else {
				if (DialogueDebug.LogWarnings) Debug.Log(string.Format("{0}: Sequencer: TextInput(): Text Field UI not found on a GameObject '{1}'. Did you specify the correct GameObject name?", new System.Object[] { DialogueDebug.Prefix, GetParameter(0) }));
				Stop ();
			}
		}

		/// <summary>
		/// When the text field UI calls our OnAcceptedText delegate, record the value into the Lua variable and
		/// stop this sequence.
		/// </summary>
		/// <param name="text">Text.</param>
		public void OnAcceptedText(string text) {
			if (!acceptedText) Lua.Run(string.Format("Variable[\"{0}\"] = \"{1}\"", new System.Object[] { variableName, DialogueLua.DoubleQuotesToSingle(text) }));
			acceptedText = true;
			Stop();
		}

		/// <summary>
		/// Finishes this sequence. If we haven't accepted text yet, tell the text field UI to cancel.
		/// </summary>
		public void OnDestroy() {
			if (!acceptedText && (textFieldUI != null)) textFieldUI.CancelTextInput();
		}
	}

}

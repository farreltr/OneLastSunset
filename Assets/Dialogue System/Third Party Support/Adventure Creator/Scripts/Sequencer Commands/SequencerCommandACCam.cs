using UnityEngine;
using System.Collections;
using AC;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.AdventureCreator;

namespace PixelCrushers.DialogueSystem.SequencerCommands {
	
	/// <summary>
	/// Implements the Adventure Creator sequencer command ACCam(on|off|idle), where:
	/// 
	/// - on: Enables AC camera control.
	/// - off: Disables AC camera control.
	/// - idle: Waits until the camera has stopped, then disables AC camera control.
	/// </summary>
	public class SequencerCommandACCam : SequencerCommand {

		private AdventureCreatorBridge bridge = null;

		public void Start() {
			string mode = GetParameter(0);
			bridge = DialogueManager.Instance.GetComponent<AdventureCreatorBridge>();
			if (DialogueDebug.LogInfo) Debug.Log(string.Format("{0}: Sequencer: ACCam({1})", DialogueDebug.Prefix, mode));
			if (bridge == null) {
				if (DialogueDebug.LogWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: ACCam({1}): Can't find AdventureCreatorBridge", DialogueDebug.Prefix, mode));
			} else if (string.Equals(mode, "on", System.StringComparison.OrdinalIgnoreCase)) {
				bridge.EnableACCameraControl();
			} else if (string.Equals(mode, "off", System.StringComparison.OrdinalIgnoreCase)) {
				bridge.DisableACCameraControl();
			} else if (string.Equals(mode, "idle", System.StringComparison.OrdinalIgnoreCase)) {
				bridge.IdleACCameraControl();
			} else {
				if (DialogueDebug.LogWarnings) Debug.LogWarning(string.Format("{0}: Sequencer: ACCam({1}): Invalid mode '{1}'", DialogueDebug.Prefix, mode));
			}
			Stop();
		}

	}
	
}

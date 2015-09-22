using UnityEngine;
using System;
using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.UnityGUI;

public class CustomUnityUIDialogueUI : UnityUIDialogueUI
{

	[Serializable]
	public class ActorStyle
	{
		public string actorName;
		public Color textColour;
	}

	public ActorStyle[] actorStyles;

	private Color originalColour = Color.white;

	public override void Start ()
	{
		originalColour = dialogue.npcSubtitle.line.color;
		base.Start ();
	}

	public override void ShowSubtitle (Subtitle subtitle)
	{
		dialogue.npcSubtitle.line.color = originalColour;
		dialogue.npcSubtitle.originalColor = originalColour;
		//dialogue.npcSubtitle.portraitName.color = originalColour;
		if (subtitle.formattedText != null && subtitle.formattedText.emphases.Length > 0) {
			subtitle.formattedText.emphases [0].color = originalColour;
		}

		foreach (var actorStyle in actorStyles) {
			if (string.Equals (subtitle.speakerInfo.Name, actorStyle.actorName)) {
				dialogue.npcSubtitle.line.color = actorStyle.textColour;
				dialogue.npcSubtitle.originalColor = actorStyle.textColour;
				//dialogue.npcSubtitle.portraitName.color = actorStyle.textColour;
				if (subtitle.formattedText != null && subtitle.formattedText.emphases.Length > 0) {
					subtitle.formattedText.emphases [0].color = actorStyle.textColour;
				}
			} 
		}
		base.ShowSubtitle (subtitle);
	}

}

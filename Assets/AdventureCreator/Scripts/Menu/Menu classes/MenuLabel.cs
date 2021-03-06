/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2014
 *	
 *	"MenuLabel.cs"
 * 
 *	This MenuElement provides a basic label.
 * 
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	/**
	 * A MenuElement that provides a basic label.
	 * The label can be used to display fixed text, or a number of pre-programmed string types, such as the active verb and Hotspot, subtitles, and more.
	 * Variable tokens of the form [var:ID] and [localvar:ID] can also be inserted to display the values of global and local variables respectively.
	 */
	public class MenuLabel : MenuElement
	{

		/** The Unity UI Text this is linked to (Unity UI Menus only) */
		public Text uiText;

		/** The display text, if labelType = AC_LabelType.Normal */
		public string label = "Element";
		/** The text alignement */
		public TextAnchor anchor;
		/** The special FX applied to the text (None, Outline, Shadow, OutlineAndShadow) */
		public TextEffects textEffects = TextEffects.None;
		/** What kind of text the label displays (Normal, Hotspot, DialogueLine, DialogueSpeaker, GlobalVariable, ActiveSaveProfile) */
		public AC_LabelType labelType;

		/** The ID number of the global variable to show (if labelType = AC_LabelType.GlobalVariable) */
		public int variableID;
		/** If True, and labelType = AC_LabelType.DialogueLine, then the displayed subtitle text will use the speaking character's subtitle text colour */
		public bool useCharacterColour = false;
		/** If True, and sizeType = AC_SizeType.Manual, then the label's height will adjust itself to fit the text within it */
		public bool autoAdjustHeight = true;
		/** If True, and labelType = AC_LabelType.Hotspot, .DialogueSpeaker or .DialogueLine, then the display text buffer can be empty */
		public bool updateIfEmpty = false;

		private string newLabel = "";
		private Speech speech;
		private Color speechColour;
		private bool isDuppingSpeech;

		#if UNITY_EDITOR
		private VariablesManager variablesManager;
		#endif


		/**
		 * Initialises the element when it is created within MenuManager.
		 */
		public override void Declare ()
		{
			uiText = null;

			label = "Label";
			isVisible = true;
			isClickable = false;
			numSlots = 1;
			anchor = TextAnchor.MiddleCenter;
			SetSize (new Vector2 (10f, 5f));
			labelType = AC_LabelType.Normal;
			variableID = 0;
			useCharacterColour = false;
			autoAdjustHeight = true;
			textEffects = TextEffects.None;
			newLabel = "";
			updateIfEmpty = false;

			base.Declare ();
		}


		/**
		 * <summary>Creates and returns a new MenuLabel that has the same values as itself.</summary>
		 * <returns>A new MenuLabel with the same values as itself</returns>
		 */
		public override MenuElement DuplicateSelf ()
		{
			MenuLabel newElement = CreateInstance <MenuLabel>();
			newElement.Declare ();
			newElement.CopyLabel (this);
			return newElement;
		}
		
		
		private void CopyLabel (MenuLabel _element)
		{
			uiText = _element.uiText;
			label = _element.label;
			anchor = _element.anchor;
			textEffects = _element.textEffects;
			labelType = _element.labelType;
			variableID = _element.variableID;
			useCharacterColour = _element.useCharacterColour;
			autoAdjustHeight = _element.autoAdjustHeight;
			updateIfEmpty = _element.updateIfEmpty;
			newLabel = "";

			base.Copy (_element);
		}


		/**
		 * <summary>Initialises the linked Unity UI GameObject.</summary>
		 * <param name = "_menu">The element's parent Menu</param>
		 */
		public override void LoadUnityUI (AC.Menu _menu)
		{
			uiText = LinkUIElement <Text>();
		}


		/**
		 * <summary>Gets the boundary of the element.</summary>
		 * <param name = "_slot">Ignored by this subclass</param>
		 * <returns>The boundary Rect of the element</returns>
		 */
		public override RectTransform GetRectTransform (int _slot)
		{
			if (uiText)
			{
				return uiText.rectTransform;
			}
			return null;
		}
		
		
		#if UNITY_EDITOR
		
		public override void ShowGUI (MenuSource source)
		{
			EditorGUILayout.BeginVertical ("Button");

			if (source != MenuSource.AdventureCreator)
			{
				uiText = LinkedUiGUI <Text> (uiText, "Linked Text:", source);
				EditorGUILayout.EndVertical ();
				EditorGUILayout.BeginVertical ("Button");
			}

			labelType = (AC_LabelType) EditorGUILayout.EnumPopup ("Label type:", labelType);
			if (source == MenuSource.AdventureCreator || labelType == AC_LabelType.Normal)
			{
				label = EditorGUILayout.TextField ("Label text:", label);
			}

			if (labelType == AC_LabelType.GlobalVariable)
			{
				variableID = AdvGame.GlobalVariableGUI ("Global Variable:", variableID);
			}
			else if (labelType == AC_LabelType.DialogueLine)
			{
				useCharacterColour = EditorGUILayout.Toggle ("Use Character text colour?", useCharacterColour);
				if (sizeType == AC_SizeType.Manual)
				{
					autoAdjustHeight = EditorGUILayout.Toggle ("Auto-adjust height to fit?", autoAdjustHeight);
				}
			}

			if (labelType == AC_LabelType.Hotspot || labelType == AC_LabelType.DialogueLine || labelType == AC_LabelType.DialogueSpeaker)
			{
				updateIfEmpty = EditorGUILayout.Toggle ("Update if string is empty?", updateIfEmpty);
			}

			if (source == MenuSource.AdventureCreator)
			{
				anchor = (TextAnchor) EditorGUILayout.EnumPopup ("Text alignment:", anchor);
				textEffects = (TextEffects) EditorGUILayout.EnumPopup ("Text effect:", textEffects);
			}
			EditorGUILayout.EndVertical ();

			base.ShowGUI (source);
		}

		#endif


		/**
		 * <summary>Assigns the element to a specific Speech line.</summary>
		 * <param name = "_speech">The Speech line to assign the element to</param>
		 */
		public override void SetSpeech (Speech _speech)
		{
			isDuppingSpeech = true;
			speech = _speech;
		}


		/**
		 * Clears any speech text on display.
		 */
		public override void ClearSpeech ()
		{
			if (labelType == AC_LabelType.DialogueLine || labelType == AC_LabelType.DialogueSpeaker)
			{
				newLabel = "";
			}
		}


		/**
		 * <summary>Performs all calculations necessary to display the element.</summary>
		 * <param name = "_slot">Ignored by this subclass</param>
		 * <param name = "languageNumber">The index number of the language to display text in</param>
		 * <param name = "isActive">If True, then the element will be drawn as though highlighted</param>
		 */
		public override void PreDisplay (int _slot, int languageNumber, bool isActive)
		{
			if (Application.isPlaying)
			{
				if (labelType == AC_LabelType.Hotspot)
				{
					if (KickStarter.playerMenus.GetHotspotLabel () != "" || updateIfEmpty)
					{
						newLabel = KickStarter.playerMenus.GetHotspotLabel ();
					}
				}
				else if (labelType == AC_LabelType.Normal)
				{
					newLabel = TranslateLabel (label, languageNumber);
				}
				else if (labelType == AC_LabelType.GlobalVariable)
				{
					newLabel = GlobalVariables.GetVariable (variableID).GetValue ();
				}
				else if (labelType == AC_LabelType.ActiveSaveProfile)
				{
					newLabel = KickStarter.options.GetProfileName ();
				}
				else
				{
					UpdateSpeechLink ();
				
					if (labelType == AC_LabelType.DialogueLine)
					{
						if (speech != null)
						{
							string line = speech.displayText;
							if (line != "" || updateIfEmpty)
							{
								newLabel = line;
							}

							if (useCharacterColour)
							{
								speechColour = speech.GetColour ();
								if (uiText)
								{
									uiText.color = speechColour;
								}
							}
						}
						else if (!KickStarter.speechManager.keepTextInBuffer)
						{
							newLabel = "";
						}
					}
					else if (labelType == AC_LabelType.DialogueSpeaker)
					{
						if (speech != null)
						{
							string line = speech.GetSpeaker ();

							if (line != "" || updateIfEmpty)
							{
								newLabel = line;
							}
						}
						else if (!KickStarter.speechManager.keepTextInBuffer)
						{
							newLabel = "";
						}
					}
				}
			}
			else
			{
				newLabel = label;
			}
			
			newLabel = AdvGame.ConvertTokens (newLabel);

			if (uiText != null)
			{
				uiText.text = newLabel;
				UpdateUIElement (uiText);
			}
		}


		/**
		 * <summary>Draws the element using OnGUI</summary>
		 * <param name = "_style">The GUIStyle to draw with</param>
		 * <param name = "_slot">Ignored by this subclass</param>
		 * <param name = "zoom">The zoom factor</param>
		 * <param name = "isActive">If True, then the element will be drawn as though highlighted</param>
		 */
		public override void Display (GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			base.Display (_style, _slot, zoom, isActive);
			
			_style.wordWrap = true;
			_style.alignment = anchor;
			if (zoom < 1f)
			{
				_style.fontSize = (int) ((float) _style.fontSize * zoom);
			}

			if (Application.isPlaying)
			{
				if (labelType == AC_LabelType.DialogueLine)
				{
					if (useCharacterColour)
					{
						_style.normal.textColor = speechColour;
					}

					if (newLabel != "" || updateIfEmpty)
					{
						if (sizeType == AC_SizeType.Manual && autoAdjustHeight)
						{
							GUIContent content = new GUIContent (newLabel);
							relativeRect.height = _style.CalcHeight (content, relativeRect.width);
						}
					}
				}
			}

			if (textEffects != TextEffects.None)
			{
				AdvGame.DrawTextEffect (ZoomRect (relativeRect, zoom), newLabel, _style, Color.black, _style.normal.textColor, 2, textEffects);
			}
			else
			{
				GUI.Label (ZoomRect (relativeRect, zoom), newLabel, _style);
			}
		}


		/**
		 * <summary>Gets the display text of the element.</summary>
		 * <param name = "slot">Ignored by this subclass</param>
		 * <param name = "languageNumber">The index number of the language number to get the text in</param>
		 * <returns>The display text of the element</returns>
		 */
		public override string GetLabel (int slot, int languageNumber)
		{
			if (labelType == AC_LabelType.Normal)
			{
				return TranslateLabel (label, languageNumber);
			}
			else if (labelType == AC_LabelType.DialogueSpeaker)
			{
				return KickStarter.dialog.GetSpeaker ();
			}
			else if (labelType == AC_LabelType.GlobalVariable)
			{
				return GlobalVariables.GetVariable (variableID).GetValue ();
			}
			else if (labelType == AC_LabelType.Hotspot)
			{
				return KickStarter.playerMenus.GetHotspotLabel ();
			}
			else if (labelType == AC_LabelType.ActiveSaveProfile)
			{
				if (Application.isPlaying)
				{
					return KickStarter.options.GetProfileName ();
				}
				else
				{
					return label;
				}
			}

			return "";
		}


		private void UpdateSpeechLink ()
		{
			if (!isDuppingSpeech && KickStarter.dialog.GetLatestSpeech () != null)
			{
				speech = KickStarter.dialog.GetLatestSpeech ();
			}
		}


		protected override void AutoSize ()
		{
			int languageNumber = Options.GetLanguage ();

			if (labelType == AC_LabelType.DialogueLine)
			{
				GUIContent content = new GUIContent (TranslateLabel (label, languageNumber));

				#if UNITY_EDITOR
				if (!Application.isPlaying)
				{
					AutoSize (content);
					return;
				}
				#endif

				GUIStyle normalStyle = new GUIStyle();
				normalStyle.font = font;
				normalStyle.fontSize = (int) (AdvGame.GetMainGameViewSize ().x * fontScaleFactor / 100);

				UpdateSpeechLink ();
				if (speech != null)
				{
					string line = " " + speech.log.fullText + " ";
					if (line.Length > 40)
					{
						line = line.Insert (line.Length / 2, " \n ");
					}
					content = new GUIContent (line);
					AutoSize (content);
				}
			}
			else if (labelType == AC_LabelType.ActiveSaveProfile)
			{
				GUIContent content = new GUIContent (GetLabel (0, 0));
				AutoSize (content);
			}
			else if (label == "" && backgroundTexture != null)
			{
				GUIContent content = new GUIContent (backgroundTexture);
				AutoSize (content);
			}
			else
			{
				GUIContent content = new GUIContent (TranslateLabel (label, languageNumber));
				AutoSize (content);
			}
		}
		
	}

}
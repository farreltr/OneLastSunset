using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionChangeFontColour : Action
	{
		public GUISkin guiSkin;
		private float speed = 100f;
		private int gameOver = 0;
		
		public ActionChangeFontColour ()
		{
			this.isDisplayed = true;
			title = "Font : Change colour to black";
		}
		
		
		override public float Run ()
		{
			//	PixelCrushers.DialogueSystem.UnityGUI.GUIRoot root = GameObject.FindObjectOfType<PixelCrushers.DialogueSystem.UnityGUI.GUIRoot> ();
			//root.
			guiSkin.FindStyle ("label").normal.textColor = Color.black;
			return 0f;
		}


		
		
		#if UNITY_EDITOR

		override public void ShowGUI ()
		{
			guiSkin = (GUISkin)EditorGUILayout.ObjectField ("GUISkin:", guiSkin, typeof(GUISkin), true);
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			// Return a string used to describe the specific action's job.
			return (" (Change font to black) ");
		}

		#endif
		
	}

}
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionChangeFontColour2 : Action
	{
		public GUISkin guiSkin;
		private float speed = 100f;
		private int gameOver = 0;
		
		public ActionChangeFontColour2 ()
		{
			this.isDisplayed = true;
			title = "Font : Change colour to green";
		}
		
		
		override public float Run ()
		{
			Color green = new Color (63f, 195f, 45f, 255f);
			guiSkin.FindStyle ("label").normal.textColor = green;
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
			return (" (Change font to green) ");
		}

		#endif
		
	}

}
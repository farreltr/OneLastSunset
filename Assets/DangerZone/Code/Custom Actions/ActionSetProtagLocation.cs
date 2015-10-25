using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionSetProtagLocation : Action
	{
		
		public ActionSetProtagLocation ()
		{
			this.isDisplayed = true;
			title = "Current Location : set";
		}
		
		
		override public float Run ()
		{
			FindObjectOfType<Protagonist> ().SetCurrentLocation (Application.loadedLevelName);
			return 0f;
		}
		
		#if UNITY_EDITOR

		override public void ShowGUI ()
		{
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			return (" (Update current location to loaded scene) ");
		}

		#endif
		
	}

}
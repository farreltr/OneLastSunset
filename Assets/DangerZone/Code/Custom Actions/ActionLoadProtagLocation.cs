using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionLoadProtagLocation : Action
	{
		
		public ActionLoadProtagLocation ()
		{
			this.isDisplayed = true;
			title = "Current Location : load";
		}
		
		
		override public float Run ()
		{
			string currentLocation = FindObjectOfType<Protagonist> ().GetCurrentLocation ();
			if (currentLocation != null && currentLocation != "UNKNOWN") {
				Application.LoadLevel (currentLocation);
			}
			return 0f;
		}
		
		#if UNITY_EDITOR

		override public void ShowGUI ()
		{
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			return (" (Update scene to current location) ");
		}

		#endif
		
	}

}
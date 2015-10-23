using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionUpdateMap : Action
	{

		private int[] placeIndices = {1,4,9,11,19,20,21,22,23,24,25,26,27,28,29};
		private float speed = 100;
		public static float distanceMultiplier = 180f;
		
		
		public ActionUpdateMap ()
		{
			this.isDisplayed = true;
			title = "Map : Update";
		}
		
		
		override public float Run ()
		{
			foreach (int idx in placeIndices) {
				GVar var = AC.GlobalVariables.GetVariable (idx);
				string name = var.label;
				bool visible = AC.GlobalVariables.GetBooleanValue (idx);
				foreach (GameObject place in GameObject.FindGameObjectsWithTag(name)) {
					if (place.GetComponent<Mark> () == null) {
						place.SetActive (visible);
					}

				}

			}
			GVar loc = AC.GlobalVariables.GetVariable (30);

			/*	Vector3 currentLocation;
			if (loc.GetValue () == "") {
				Vector3 jebs = GameObject.FindGameObjectWithTag ("visible.jebs").transform.position;
				Vector3 foxs = GameObject.FindGameObjectWithTag ("visible.foxs").transform.position; 
				Vector3 coyotepoint = GameObject.FindGameObjectWithTag ("visible.coyotepoint").transform.position; 
				currentLocation = new Vector3 (Random.Range (foxs.x, coyotepoint.x), Random.Range (foxs.y, jebs.y), 0);

			} else {
				GameObject currentLoc = GameObject.FindGameObjectWithTag ("visible." + loc.GetValue ());
				currentLocation = currentLoc.transform.position;
				currentLocation.y = currentLocation.y + 0.25f;

			}
			GameObject.FindGameObjectWithTag ("current.location").transform.position = currentLocation;*/

			Protagonist protag = FindObjectOfType<Protagonist> ();

			foreach (Place place in GameObject.FindObjectsOfType<Place>()) {
				if (place.gameObject.tag != "visible." + loc.GetValue () && place.isActiveAndEnabled) {
					float distance = ComputeDistance (protag.GetCoordinate (), place.GetCoordinate ());
					int gas = ComputeGas (protag.GetCoordinate (), place.GetCoordinate ());
					float time = ComputeTime (distance);
					Hotspot hotspot = place.GetComponent<Hotspot> ();

					string label = place.gameObject.name;
					label = label + "\n" + distance.ToString () + " Miles\n" 
						+ time.ToString () + " HOURS\n"
						+ gas.ToString () + "% GAS\n\n"
						+ hotspot.hotspotName;

					hotspot.hotspotName = label;
				}

			}

			return 0f;

		}

		private float ComputeDistance (Vector2 currentLocation, Vector2 otherLocation)
		{
			float distance = Vector2.Distance (currentLocation, otherLocation);
			distance = Mathf.Round (distance * 100f) / 100f;
			return distance * distanceMultiplier;

		}

		private float ComputeTime (float distance)
		{
			float time = distance / speed;
			time = Mathf.Round (time * 10f) / 10f;
			return time;
			
		}

		private int ComputeGas (Vector2 currentLocation, Vector2 otherLocation)
		{
			float modifier = ComputeModifier ();
			float distance = Vector2.Distance (currentLocation, otherLocation);
			float modulo = distance * 5 / 3;
			return Mathf.FloorToInt (modulo);
			
		}

		private float ComputeModifier ()
		{
			return 0f;
		}
		
		
		#if UNITY_EDITOR

		override public void ShowGUI ()
		{
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			return (" (Update place visibility on map) ");
		}

		#endif
		
	}

}
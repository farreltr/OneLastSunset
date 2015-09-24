using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionUpdateStats : Action
	{
		public GameObject location;
		private float speed = 100f;
		private int gameOver = 0;
		
		public ActionUpdateStats ()
		{
			this.isDisplayed = true;
			title = "Stats : Update";
		}
		
		
		override public float Run ()
		{
			gameOver = AC.GlobalVariables.GetVariable (35).val;
			GVar loc = AC.GlobalVariables.GetVariable (30);
			GameObject currentLoc = GameObject.FindObjectOfType<Player> ().gameObject;
			Vector3 currentLocation = currentLoc.transform.position;
			float distance = ComputeDistance (currentLocation, location.transform.position);
			int gas = ComputeGas (currentLocation, location.transform.position);
			float time = ComputeTime (distance);
			GVar timeVar = AC.GlobalVariables.GetVariable (16);
			GVar gasVar = AC.GlobalVariables.GetVariable (7);
			string journeyString = loc.GetValue () + "_" + location.name;
			//CheckWinForJourney (timeVar.floatVal, time, journeyString, location.name);
			timeVar.floatVal = timeVar.floatVal - time;
			gasVar.val = gasVar.val - gas;
			Vector3 updatedPosition = location.transform.position;
			updatedPosition.y = updatedPosition.y + 0.25f;
			currentLoc.transform.position = updatedPosition;
			return 0f;
		}

		void CheckWinForJourney (float current, float timeDecrease, string journeyString, string locationName)
		{
			Antagonist belt = GameObject.FindObjectOfType<Antagonist> ();
			int currInt = 70 - Mathf.RoundToInt (current);
			int tInt = Mathf.RoundToInt (timeDecrease);
			for (int i = currInt; i < currInt + tInt; i++) {
				if (belt.GetPosition (i) == journeyString || belt.GetPosition (i) == locationName) {
					GVar bullets = AC.GlobalVariables.GetVariable (5);
					AC.GlobalVariables.GetVariable (35).val = 1;
					if (bullets.val <= 0) {
						Application.LoadLevel ("bullets");
					} else {
						Application.LoadLevel ("win");
					}

				}

			}


		}
	

		private float ComputeDistance (Vector3 currentLocation, Vector3 otherLocation)
		{
			float distance = Vector3.Distance (currentLocation, otherLocation);
			distance = Mathf.Round (distance * 100f) / 100f;
			return distance * ActionUpdateMap.distanceMultiplier;

		}

		private float ComputeTime (float distance)
		{
			float time = distance / speed;
			time = Mathf.Round (time * 10f) / 10f;
			return time;
			
		}

		private int ComputeGas (Vector3 currentLocation, Vector3 otherLocation)
		{
			float modifier = ComputeModifier ();
			float distance = Vector3.Distance (currentLocation, otherLocation);
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
			location = (GameObject)EditorGUILayout.ObjectField ("Location:", location, typeof(GameObject), true);
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			// Return a string used to describe the specific action's job.
			return (" (Update stats on move to location) ");
		}

		#endif
		
	}

}
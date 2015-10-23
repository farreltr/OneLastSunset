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
		public string locationName;
		private float speed = 100f;
		private int gameOver = 0;
		private Vector2 currentCoordinate;
		
		public ActionUpdateStats ()
		{
			this.isDisplayed = true;
			title = "Stats : Update";
		}
		
		
		override public float Run ()
		{
			Protagonist protag = GameObject.FindObjectOfType<Protagonist> ();
			Place location = GameObject.Find (locationName).GetComponent<Place> ();
			Vector2 protagCoord = protag.GetCoordinate ();

			gameOver = AC.GlobalVariables.GetVariable (35).val;
			GVar loc = AC.GlobalVariables.GetVariable (30);
			float distance = ComputeDistance (protagCoord, location.GetCoordinate ());
			int gas = ComputeGas (protagCoord, location.GetCoordinate ());
			float time = ComputeTime (distance);
			GVar timeVar = AC.GlobalVariables.GetVariable (16);
			GVar gasVar = AC.GlobalVariables.GetVariable (7);
			timeVar.floatVal = timeVar.floatVal - time;
			gasVar.val = gasVar.val - gas;
			currentCoordinate = location.GetCoordinate ();
			protag.SetCoordinate (currentCoordinate);
			protag.transform.parent = location.transform;
			protag.transform.localPosition = Vector3.zero;
			return 0f;
		}

		void CheckWinForJourney (float current, float timeDecrease, string journeyString, string locationName)
		{
			Antagonist belt = GameObject.FindObjectOfType<Antagonist> ();
			int currInt = 70 - Mathf.RoundToInt (current);
			int tInt = Mathf.RoundToInt (timeDecrease);
			for (int i = currInt; i < currInt + tInt; i++) {
				if (belt.GetCoordinate () == currentCoordinate) {
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
	

		private float ComputeDistance (Vector2 currentLocation, Vector2 otherLocation)
		{
			float distance = Vector2.Distance (currentLocation, otherLocation);
			distance = Mathf.Round (distance * 100f) / 100f;
			return distance * ActionUpdateMap.distanceMultiplier;
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
			locationName = EditorGUILayout.TextField ("Location Name: ", locationName);
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			return (" (Update stats on move to location) ");
		}

		#endif
		
	}

}
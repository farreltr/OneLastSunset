using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionUpdateGridUL : Action
	{

		private string direction;
		
		public ActionUpdateGridUL ()
		{
			this.isDisplayed = true;
			title = "Grid : Update";

		}
		
		
		override public float Run ()
		{
			/*Random.seed = System.DateTime.Now.Millisecond + 134878;
			Controller controller = GameObject.FindObjectOfType<Controller> ();
			string placeName = AC.GlobalVariables.GetStringValue (30);
			Vector2 placeCoord;
			if (Application.loadedLevelName != "Map") {
				placeName = Application.loadedLevelName;
			}
			Place place = controller.GetPlace (placeName);
			if (place == null) {
				placeCoord = FindObjectOfType<Antagonist> ().GetCoordinate ();
			} else {
				placeCoord = place.GetCoordinate ();
			}

			Vector2 beltCoordinate = controller.GetBeltsCoordinate ();
			float x = beltCoordinate.x - placeCoord.x; // pos east, neg west
			float y = beltCoordinate.y - placeCoord.y; // pos north, neg south


			if (Mathf.Abs (x) > Mathf.Abs (y)) { // More across than up
				direction = x > 0 ? "east" : "west";

			} else if (Mathf.Abs (x) < Mathf.Abs (y)) {// More up than across
				direction = y > 0 ? "north" : "south";
			} else {
				direction = Random.Range (0, 1) > 0 ? x > 0 ? "east" : "west" : y > 0 ? "north" : "south";
			}
			AC.GlobalVariables.SetStringValue (39, direction);
			
			Vector4 placeCoordinate;
			if (place != null) {
				placeCoordinate = place.GetCoordinate ();
			} else {
				placeCoordinate = FindObjectOfType<PlaceCoordinate> ().GetCoordinate ();
			}
			controller.TurnOnCoords (direction, placeCoordinate);	
			controller.UpdateMap ();*/
			return 0f;
		}

	
		
		#if UNITY_EDITOR

		override public void ShowGUI ()
		{
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			return (" (Update grid on map) ");
		}

		#endif
		
	}

}
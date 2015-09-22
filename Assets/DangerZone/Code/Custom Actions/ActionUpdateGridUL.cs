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
		
		public ActionUpdateGridUL ()
		{
			this.isDisplayed = true;
			title = "Grid : Update";
		}
		
		
		override public float Run ()
		{
			Random.seed = System.DateTime.Now.Millisecond;
			CoordList coords = GameObject.FindObjectOfType<CoordList> ();
			string placeName = AC.GlobalVariables.GetStringValue (30);
			ShowPosition grid = GameObject.FindObjectOfType<ShowPosition> ();
			Vector2 placeCoord = coords.GetCoordinateForPlace (placeName);
			float x = coords.beltCoord.x - placeCoord.x; // pos east, neg west
			float y = coords.beltCoord.y - placeCoord.y; // pos north, neg south
			string direction;

			if (Mathf.Abs (x) > Mathf.Abs (y)) { // More across than up
				direction = x > 0 ? "east" : "west";

			} else if (Mathf.Abs (x) < Mathf.Abs (y)) {// More up than across
				direction = y > 0 ? "north" : "south";
			} else {
				direction = Random.Range (0, 1) > 0 ? x > 0 ? "east" : "west" : y > 0 ? "north" : "south";
			}
			AC.GlobalVariables.SetStringValue (39, direction);
			TurnOnCoords (direction, placeCoord);

			return 0f;

		}

		private void TurnOnCoords (string direction, Vector2 placeCoord)
		{

			CoordList coords = GameObject.FindObjectOfType<CoordList> ();
			if (direction == "north") {
				int x = Mathf.RoundToInt (placeCoord.x);
				for (int i = 0; i<18; i++) {
					for (int j=0; j<x; j++) {
						Vector2 vec = new Vector2 (i, j);
						if (!coords.coordinates.Contains (vec)) {
							coords.coordinates.Add (vec);
						}

					}
				}

			}
			if (direction == "south") {
				int x = Mathf.RoundToInt (placeCoord.x);
				for (int i = 0; i<18; i++) {
					for (int j=8; j>x; j--) {
						Vector2 vec = new Vector2 (i, j);
						if (!coords.coordinates.Contains (vec)) {
							coords.coordinates.Add (vec);
						}

					}
				}
			}
			if (direction == "east") {
				int y = Mathf.RoundToInt (placeCoord.y);
				for (int i = 0; i<y; i++) {
					for (int j=0; j<9; j++) {
						Vector2 vec = new Vector2 (i, j);
						if (!coords.coordinates.Contains (vec)) {
							coords.coordinates.Add (vec);
						}

					}
				}
				
			}
			if (direction == "west") {

				int y = Mathf.RoundToInt (placeCoord.y);
				for (int i = 16; i>y; i--) {
					for (int j=0; j<9; j++) {
						Vector2 vec = new Vector2 (i, j);
						if (!coords.coordinates.Contains (vec)) {
							coords.coordinates.Add (vec);
						}

					}
				}
				
			}

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
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

	private IDictionary<Vector2, Coordinated> boxes = new Dictionary<Vector2, Coordinated> ();
	private Antagonist antagonist;
	private Protagonist protagonist;
	private string direction;

	void Start ()
	{
		DontDestroyOnLoad (this.gameObject);
	
	}

	void Update ()
	{
		UpdateDirection ();
		if (AC.GlobalVariables.GetBooleanValue (2)) {
			TurnOnCoords ();
			AC.GlobalVariables.SetBooleanValue (2, false);
		}
		if (EnableBelt ()) {
			antagonist.SetActive (true);
		}

	}

	private bool EnableBelt ()
	{
		int count = 0;
		foreach (Coordinated c in FindObjectsOfType<Coordinated>()) {
			if (!c.isOn) {
				count++;
			}
			if (count > 1) {
				return false;
			}
		}
		if (count == 1) {
			return true;
		}
		return false;
	}

	public IDictionary<Vector2,Coordinated> GetBlocks ()
	{
		if (boxes.Count == 0) {
			Coordinated[] coords = FindObjectsOfType<Coordinated> ();
			foreach (Coordinated coord in coords) {
				if (!boxes.ContainsKey (coord.GetCoordinate ())) {
					boxes.Add (coord.GetCoordinate (), coord);
				}

			}
		}
		return boxes;
	}

	private void UpdateDirection ()
	{
		Vector2 antagCoordinate = FindObjectOfType<Antagonist> ().GetCoordinate ();
		Vector2 protagCoordinate = FindObjectOfType<Protagonist> ().GetCoordinate ();
		float x = antagCoordinate.x - protagCoordinate.x; // pos east, neg west
		float y = antagCoordinate.y - protagCoordinate.y; // pos north, neg south
				
		if (Mathf.Abs (x) > Mathf.Abs (y)) {
			direction = x > 0 ? "east" : "west";
		} else if (Mathf.Abs (x) < Mathf.Abs (y)) {
			direction = y > 0 ? "south" : "north";
		} else {
			direction = Random.Range (0, 1) > 0 ? x > 0 ? "east" : "west" : y > 0 ? "south" : "north";
		}
		AC.GlobalVariables.SetStringValue (39, direction);
	}


	public void TurnOnCoords ()
	{
		Vector2 coord = FindObjectOfType<Protagonist> ().GetCoordinate ();

		if (direction == "south") {
			int y = Mathf.RoundToInt (coord.y);
			for (int i = 0; i<18; i++) {
				for (int j=y+1; j>0; j--) {
					TurnOn (i, j);
				}
			}
		}
		if (direction == "north") {
			int y = Mathf.RoundToInt (coord.y);
			for (int i = 0; i<18; i++) {
				for (int j=0; j<y; j++) {
					TurnOn (i, j);
				}
			}
		}
		if (direction == "east") {
			int x = Mathf.RoundToInt (coord.x);
			for (int i = 0; i<x; i++) {
				for (int j=0; j<9; j++) {
					TurnOn (i, j);
				}
			}
		}
		if (direction == "west") {
			int x = Mathf.RoundToInt (coord.x);
			for (int i = x+1; i<18; i++) {
				for (int j=0; j<9; j++) {
					TurnOn (i, j);
				}
			}
		}
	}

	private void TurnOn (int i, int j)
	{
		Vector2 vec = new Vector2 (i, j);
		Coordinated block;
		if (GetBlocks ().TryGetValue (vec, out block)) {
			block.TurnOn ();
		}
	}



}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Protagonist : MonoBehaviour
{

	private static Protagonist instance;
	private SpriteRenderer renderer;
	private Vector2 coordinate = -Vector2.one;
	private IDictionary<Vector2, Coordinated> boxes = new Dictionary<Vector2, Coordinated> ();
	private bool isDriving = false;
	private string currentLocation;
	
	private Protagonist ()
	{
	}
	
	public static Protagonist Instance {
		get {
			if (instance == null) {
				instance = new Protagonist ();
			}
			return instance;
		}
	}

	void Start ()
	{
		DontDestroyOnLoad (this.gameObject);
		this.renderer = GetComponent<SpriteRenderer> ();
	}

	void Update ()
	{
		this.renderer.enabled = Application.loadedLevelName == "Map";

	}

	public Vector2 GetCoordinate ()
	{

		if (coordinate == -Vector2.one) {
			SetCoordinate (new Vector2 (Random.Range (7, 14), Random.Range (2, 6)));
		}
		return coordinate;
	
	}

	public void SetCoordinate (Vector2 coord)
	{
		this.coordinate = coord;
		Coordinated value = GetCoordinated (coord);
		if (value != null) {
			transform.parent = value.transform;
			transform.localPosition = Vector3.one;
			Place place = value.GetComponentInChildren<Place> ();
			if (place != null) {
				AC.GlobalVariables.SetStringValue (30, place.GetName ());
				SetCurrentLocation (place.GetName ());
			}
		}
		
	}

	Coordinated GetCoordinated (Vector2 coord)
	{
		foreach (Coordinated c in FindObjectsOfType<Coordinated> ()) {
			if (c.GetCoordinate () == coord) {
				return c;
			}
		}
		return null;

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

	public bool IsDriving ()
	{
		return isDriving;
	}

	public void SetDriving (bool isDriving)
	{
		this.isDriving = isDriving;
	}

	public string GetCurrentLocation ()
	{
		if (currentLocation == null || currentLocation == "") {
			currentLocation = "UNKNOWN";
		}
		return currentLocation;
	}

	public void SetCurrentLocation (string currentLocation)
	{
		this.currentLocation = currentLocation;
	}



}

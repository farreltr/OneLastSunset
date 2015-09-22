using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoordList : MonoBehaviour
{
	public List<Vector2> coordinates;
	public Vector2 beltCoord;
	public Dictionary<string, Vector2> places = new Dictionary<string,Vector2 > ();

	// Use this for initialization
	void Start ()
	{
		Random.seed = System.DateTime.Now.Millisecond;
		beltCoord = new Vector2 (Random.Range (1, 17), Random.Range (1, 8));
		SetPlaceCoordinates ();
	
	}

	private void SetPlaceCoordinates ()
	{

		places.Add ("foxs", new Vector2 (6, 4));
		places.Add ("jebs", new Vector2 (10, 6));
		places.Add ("canyon", new Vector2 (12, 1));
		places.Add ("coyotepoint", new Vector2 (16, 5));
		places.Add ("reevesville", new Vector2 (16, 1));
		places.Add ("radlers", new Vector2 (13, 2));
		places.Add ("getsburg", new Vector2 (14, 5));
		places.Add ("connors", new Vector2 (10, 3));
		places.Add ("rats", new Vector2 (7, 1));
		places.Add ("oasis", new Vector2 (1, 1));
		places.Add ("mine", new Vector2 (2, 3));
		places.Add ("rust", new Vector2 (2, 6));
		places.Add ("cemetery", new Vector2 (5, 6));
		places.Add ("ranch", new Vector2 (8, 8));
		places.Add ("ghost", new Vector2 (15, 8));
	}

	public Vector2 GetCoordinateForPlace (string placeName)
	{
		Vector2 coord = Vector2.one;
		places.TryGetValue (placeName, out coord);
		return coord;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

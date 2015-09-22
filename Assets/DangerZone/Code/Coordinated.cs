using UnityEngine;
using System.Collections;

public class Coordinated : MonoBehaviour
{
	
	public Vector2 coordinate;
	private ShowPosition grid;
	public bool isOn;

	void OnStart ()
	{
		grid = GameObject.FindObjectOfType<ShowPosition> ();
		SetCoordinate (grid.GetCoordinateFromPosition (this.transform.position));
	}

	public void SetCoordinate (Vector2 coord)
	{
		this.coordinate = coord;
	}

	public Vector2 GetCoordinate ()
	{
		return this.coordinate;
	}

	public bool IsPositionAtCoordinate (Vector3 pos)
	{
		return coordinate == grid.GetCoordinateFromPosition (pos);
	}
}

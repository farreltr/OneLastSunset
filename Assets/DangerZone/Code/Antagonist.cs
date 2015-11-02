using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Antagonist : MonoBehaviour
{
	private static Antagonist instance;
	private Vector2 coordinate = -Vector2.one;
	private AC.Hotspot hotspot;
	private SpriteRenderer renderer;
	private BoxCollider2D collider2d;
	private IDictionary<Vector2, Coordinated> boxes = new Dictionary<Vector2, Coordinated> ();
	
	private Antagonist ()
	{
	}
	
	public static Antagonist Instance {
		get {
			if (instance == null) {
				instance = new Antagonist ();
			}
			return instance;
		}
	}

	void Start ()
	{
		Random.seed = System.DateTime.Now.Millisecond;
		DontDestroyOnLoad (this.gameObject);
		this.renderer = GetComponent<SpriteRenderer> ();
		this.hotspot = GetComponent<AC.Hotspot> ();
		this.collider2d = GetComponent<BoxCollider2D> ();
		SetActive (false);
	}

	
	public Vector2 GetCoordinate ()
	{
		if (coordinate == -Vector2.one) {
			SetCoordinate (new Vector2 (Random.Range (1, 17), Random.Range (1, 8)));
		}
		return coordinate;
		
	}

	public void SetCoordinate (Vector2 coord)
	{
		this.coordinate = coord;
		Coordinated value = GetCoordinated (coord);
		if (value != null) {
			transform.parent = value.transform;
			transform.localPosition = Vector3.zero;
		}
	}

	public void SetActive (bool active)
	{
		this.renderer.enabled = active && Application.loadedLevelName == "Map";
		this.hotspot.enabled = active && Application.loadedLevelName == "Map";
		this.collider2d.enabled = active && Application.loadedLevelName == "Map";
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

	Coordinated GetCoordinated (Vector2 coord)
	{
		foreach (Coordinated c in FindObjectsOfType<Coordinated> ()) {
			if (c.GetCoordinate () == coord) {
				return c;
			}
		}
		return null;
		
	}

}

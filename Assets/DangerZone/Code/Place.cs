using UnityEngine;
using System.Collections;

public class Place : MonoBehaviour
{

	private SpriteRenderer renderer;
	private BoxCollider2D collider2d;
	public string description;
	
	void Start ()
	{
		this.renderer = GetComponent<SpriteRenderer> ();
		this.collider2d = GetComponent<BoxCollider2D> ();
	}
	
	void Update ()
	{
		this.renderer.enabled = Application.loadedLevelName == "Map";
		this.collider2d.enabled = Application.loadedLevelName == "Map";
	}

	public Vector2 GetCoordinate ()
	{
		return GetComponentInParent<Coordinated> ().coordinate;
	}

	public string GetName ()
	{
		return this.gameObject.name.ToUpper ();
	}

	public string GetDescription ()
	{
		return description;
	}

	public void SetDescription (string description)
	{
		this.description = description;
	}


}

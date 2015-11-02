using UnityEngine;
using System.Collections;
using AC;

public class Place : MonoBehaviour
{

	private SpriteRenderer renderer;
	private BoxCollider2D collider2d;
	private Hotspot hotspot;
	public string description;
	private bool isActive;
	
	void Start ()
	{
		this.renderer = GetComponent<SpriteRenderer> ();
		this.collider2d = GetComponent<BoxCollider2D> ();
		this.hotspot = GetComponent<Hotspot> ();
	}
	
	void Update ()
	{
		this.renderer.enabled = isActive && Application.loadedLevelName == "Map";
		this.collider2d.enabled = isActive && Application.loadedLevelName == "Map";
		if (isActive && Application.loadedLevelName == "Map") {
			this.hotspot.TurnOn ();
		} else {
			this.hotspot.TurnOff ();
		}
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

	public void TurnOn ()
	{
		this.isActive = true;
	}

	public void TurnOff ()
	{
		this.isActive = false;
	}

}

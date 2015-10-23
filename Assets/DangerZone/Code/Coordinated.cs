using UnityEngine;
using System.Collections;

public class Coordinated : MonoBehaviour
{
	
	public Vector2 coordinate;
	public bool isOn;
	public Sprite onSprite;
	public Sprite offSprite;
	private SpriteRenderer renderer;
	private BoxCollider2D collider2d;

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
	

	public void SetCoordinate (Vector2 coord)
	{
		this.coordinate = coord;
	}

	public Vector2 GetCoordinate ()
	{
		return this.coordinate;
	}

	public void TurnOn ()
	{
		this.GetComponent<SpriteRenderer> ().sprite = onSprite;
		isOn = true;
	}

	public void TurnOff ()
	{
		this.GetComponent<SpriteRenderer> ().sprite = offSprite;
		isOn = false;

	}
	
}

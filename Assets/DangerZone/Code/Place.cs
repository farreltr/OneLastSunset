using UnityEngine;
using System.Collections;

public class Place : MonoBehaviour
{

	private SpriteRenderer renderer;
	
	void Start ()
	{
		this.renderer = GetComponent<SpriteRenderer> ();
	}
	
	void Update ()
	{
		this.renderer.enabled = Application.loadedLevelName == "Map";
	}

	public Vector2 GetCoordinate ()
	{
		return GetComponentInParent<Coordinated> ().coordinate;
	}

	public string GetName ()
	{
		return this.gameObject.name.ToUpper ();
	}
}

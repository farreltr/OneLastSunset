using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour
{
	public int damage = 0;

	// Use this for initialization
	void Start ()
	{
		this.gameObject.layer = LayerMask.NameToLayer ("Body Part");
	}
	
	public int GetHitpoints ()
	{
		return damage;

	}	

	void Update ()
	{
	
	}
}

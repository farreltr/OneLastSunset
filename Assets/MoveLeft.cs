using UnityEngine;
using System.Collections;

public class MoveLeft : MonoBehaviour
{

	public float speed = 20f;

	void Update ()
	{
		if (transform.position.x > -100f) {
			transform.position = Vector3.left * Time.deltaTime * speed;
		}
	
	}
}

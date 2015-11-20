using UnityEngine;
using System.Collections;

public class TargetMovement : MonoBehaviour
{    
	
	private float angle = 0;
	private float speed = (2 * Mathf.PI) / 2 ;//2*PI in degress is 360, so you get 5 seconds to complete a circle
	private float radius = 0.4f;

	void Update ()
	{
		angle += speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
		Vector3 center = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector3 position = new Vector3 ();
		float rand = Random.Range (0.0f, 0.2f);
		float randRad = radius + rand;
		position.x = Mathf.Cos (angle) * radius + center.x;
		position.y = Mathf.Sin (angle) * radius + center.y;
		this.transform.position = position;
	}
}
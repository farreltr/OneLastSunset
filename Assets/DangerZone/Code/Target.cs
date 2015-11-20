using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{

	public Vector3 target;
	public float movementSpeed = 5f;
	public float rotationSpeed = 90f;
	public float xoffset = 0f;
	public float yoffset = 0.5f;
	
	private float currentAngle;
	private float targetAngle;
	private Vector3 offsetTargetPos;

	void Start ()
	{
		target = GameObject.FindObjectOfType<Shootable> ().gameObject.transform.position;
		target = transform.parent.position;
		//target = Input.mousePosition;
		offsetTargetPos = new Vector3 (target.x + xoffset, target.y + yoffset, 0);
		Vector3 pos = transform.position;
		pos.z = 0f;
		transform.position = pos;
		currentAngle = GetAngleToTarget ();
	}

	void Update ()
	{
		target = transform.parent.position;
		offsetTargetPos = new Vector3 (target.x + xoffset, target.y + yoffset, 0);
		targetAngle = GetAngleToTarget ();
		currentAngle = Mathf.MoveTowardsAngle (currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
		transform.position += Quaternion.Euler (0, 0, currentAngle) * Vector3.right * movementSpeed * Time.deltaTime;	
		transform.parent.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}
	
	float GetAngleToTarget ()
	{
		Vector3 v3 = offsetTargetPos - transform.position;        
		return Mathf.Atan2 (v3.y, v3.x) * Mathf.Rad2Deg;
	}
}

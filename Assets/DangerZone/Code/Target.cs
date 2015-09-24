using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour
{

	public GameObject target;
	public float movementSpeed = 5f;
	public float rotationSpeed = 90f;
	public float xoffset = 0f;
	public float yoffset = 0.5f;
	
	private float currentAngle;
	private float targetAngle;
	private Vector3 offsetTargetPos;

	void Start ()
	{
		target = GameObject.FindObjectOfType<Shootable> ().gameObject;
		offsetTargetPos = new Vector3 (target.transform.position.x + xoffset, target.transform.position.y + yoffset, 0);
		Vector3 pos = transform.position;
		pos.z = 0f;
		transform.position = pos;
		currentAngle = GetAngleToTarget ();
	}

	void Update ()
	{
		targetAngle = GetAngleToTarget ();
		currentAngle = Mathf.MoveTowardsAngle (currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
		transform.position += Quaternion.Euler (0, 0, currentAngle) * Vector3.right * movementSpeed * Time.deltaTime;	
	}
	
	float GetAngleToTarget ()
	{
		Vector3 v3 = offsetTargetPos - transform.position;        
		return Mathf.Atan2 (v3.y, v3.x) * Mathf.Rad2Deg;
	}
}

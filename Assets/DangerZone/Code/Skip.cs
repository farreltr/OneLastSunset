using UnityEngine;
using System.Collections;

public class Skip : MonoBehaviour
{

	void Start ()
	{
		foreach (Coordinated coord in FindObjectsOfType<Coordinated>()) {
			coord.TurnOff ();
		}
		Grid grid = FindObjectOfType<Grid> ();
		if (grid != null) {
			DestroyObject (grid);
		}
	}

	void Update ()
	{
		if (Input.GetKey (KeyCode.Space)) {
			Application.LoadLevel ("Drive");
		}
	
	}
}

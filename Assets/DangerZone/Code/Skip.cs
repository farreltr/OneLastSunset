using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
		this.GetComponent<Text> ().CrossFadeAlpha (255, 1000, false);
	}

	void Update ()
	{
		if (Input.GetKey (KeyCode.Space)) {
			Application.LoadLevel ("Drive");
		}
	
	}
}

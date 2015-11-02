using UnityEngine;
using System.Collections;

public class ClickForward : MonoBehaviour
{

	public string level = "Opening";
	
	
	void Update ()
	{
		if (Input.GetMouseButton (0)) {
			Application.LoadLevel (level);
		}
	
	}
}

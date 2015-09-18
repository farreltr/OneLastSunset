using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour
{
	private static DontDestroy instance;
	
	private DontDestroy ()
	{
	}
	
	public static DontDestroy Instance {
		get {
			if (instance == null) {
				instance = new DontDestroy ();
			}
			return instance;
		}
	}

	void Start ()
	{
		DontDestroyOnLoad (this.gameObject);
	}

	void Update ()
	{
		if (AC.GlobalVariables.GetVariable (35).val == 1) {
			AC.GlobalVariables.GetVariable (35).val = 0;
			Application.LoadLevel ("win");
		}
	}
}

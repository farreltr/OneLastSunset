using UnityEngine;
using System.Collections;

public class UpdateInfo : MonoBehaviour
{

	private int[] placeIndices = {1,4,9,11,19,20,21,22,23,24,25,26,27,28,29};
	
	void Update ()
	{
/*		AC.GVar updateInfo = AC.GlobalVariables.GetVariable (2);
		if (updateInfo.val == 1) {
			Antagonist belt = GameObject.FindObjectOfType<Antagonist> ();
			string position = belt.GetPosition (70f - AC.GlobalVariables.GetVariable (16).floatVal);
			Mark[] markers = GameObject.FindObjectsOfType<Mark> ();
			foreach (Mark marker in markers) {
				if (marker.GetName () == position) {
					marker.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
					marker.EnablePlaces ();
					foreach (int idx in placeIndices) {
						AC.GVar var = AC.GlobalVariables.GetVariable (idx);
						string name = var.label;
						if (name == marker.gameObject.tag) {
							var.val = 1;
						}
						
					}
					updateInfo.val = 0;
				} else {
					marker.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
				}
			}

		}
		AC.GVar updateReevesville = AC.GlobalVariables.GetVariable (34);
		if (updateReevesville.val == 1) {
			Mark[] markersR = GameObject.FindObjectsOfType<Mark> ();
			foreach (Mark marker in markersR) {
				if (marker.GetName () == "reevesville") {
					marker.gameObject.GetComponent<SpriteRenderer> ().enabled = true;
					marker.EnablePlaces ();
					updateReevesville.val = 0;
				} else {
					marker.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
				}

			}
		}*/
	
	}
}

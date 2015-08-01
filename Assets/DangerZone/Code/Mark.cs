using UnityEngine;
using System.Collections;

public class Mark : MonoBehaviour
{

	public Place associatedPlace;
	public AC.Hotspot associatedHotspot;

	public string GetName ()
	{
		return this.gameObject.name;
	}

	public void EnablePlaces ()
	{
		if (associatedPlace != null) {
			associatedPlace.gameObject.SetActive (true);
		}
		if (associatedHotspot != null) {
			associatedHotspot.gameObject.SetActive (true);
		}

	}
}

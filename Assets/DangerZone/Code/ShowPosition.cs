using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ShowPosition : MonoBehaviour
{
	public Coordinated onBlock;
	public Coordinated offBlock;
	public float iStart;
	public float iEnd;
	public float jStart;
	public float jEnd;
	public float offset;
	public float scale;
	private CoordList list;
	private List<Coordinated> offBlocks;
	private List<Coordinated> onBlocks;
	public AC.Hotspot belt = null;
	
	void Start ()
	{
		offBlocks = new List<Coordinated> ();
		onBlocks = new List<Coordinated> ();
		list = GameObject.FindObjectOfType<CoordList> ();
		//list = GenerateTestData (); // Uncomment for testing
		offBlock.transform.localScale = Vector3.one * scale;
		onBlock.transform.localScale = Vector3.one * scale;
		for (float i=iStart; i<iEnd; i+=offset) {
			for (float j=jStart; j<jEnd; j+=offset) {
				Vector3 pos = new Vector3 (i, j, 0);
				Vector2 coord = GetCoordinateFromPosition (pos);
				Coordinated b = GetBlock (coord);
				Coordinated block = Instantiate (b, pos, b.transform.rotation) as Coordinated;
				block.SetCoordinate (coord);
				if (block.isOn) {
					onBlocks.Add (block);
				} else {
					offBlocks.Add (block);
				}
			}
		}
		if (offBlocks.Count == 1) {
			Coordinated[] bs = offBlocks.ToArray ();
			//AC.Hotspot hs = Instantiate (belt, bs [0].transform.position, belt.transform.rotation) as AC.Hotspot;
			GameObject go = GameObject.FindGameObjectWithTag ("BeltHS");
			AC.Hotspot hs = go.GetComponent<AC.Hotspot> ();
			hs.transform.SetParent (bs [0].transform);
			hs.transform.localPosition = Vector3.zero;
		}
	}

	private Coordinated GetBlock (Vector2 coord)
	{
		if (list.coordinates.Contains (coord)) {
			return onBlock;
		}
		return offBlock;
	}

	private CoordList GenerateTestData ()
	{
		CoordList list = GameObject.FindObjectOfType<CoordList> ();

		for (int i = 0; i<19; i++) {
			for (int j = 0; j<9; j++) {
				list.coordinates.Add (new Vector2 (i, j));
			}
		}
		list.coordinates.RemoveAt (Random.Range (0, list.coordinates.Count));
		return list;
	}


	void Update ()
	{
		// coord.transform.position = Input.mousePosition;
		// coord.text = Input.mousePosition.ToString ();
	}

	public Vector2 GetCoordinateFromPosition (Vector3 position)
	{
		float x = (position.x - iStart) / offset;
		float y = (position.y - jStart) / offset;
		return new Vector2 (x, y);
	}
}

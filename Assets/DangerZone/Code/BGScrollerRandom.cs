using UnityEngine;
using System.Collections;

public class BGScrollerRandom : MonoBehaviour
{
	public float scrollSpeed;
	public float tileSizeX;
	public Sprite[] tiles;
	public float[] tileHeights;
	
	private Vector3 startPosition;
	private Vector3 offScreen;

	private SpriteRenderer renderer;
	
	void Start ()
	{
		startPosition = transform.position;
		offScreen = startPosition;
		offScreen.x = tileSizeX;
		renderer = this.GetComponent<SpriteRenderer> ();
	}
	
	void Update ()
	{
		transform.Translate (Vector3.left * Time.deltaTime * scrollSpeed);
		Sprite currentSprite = renderer.sprite;
		int i = 0;
		foreach (Sprite sprite in tiles) {
			if (sprite == currentSprite) {
				transform.localPosition = new Vector3 (transform.position.x, tileHeights [i], 0);
			}
			i++;
		}


		if (transform.position.x < tileSizeX * -1f) {
			int rand = Random.Range (0, tiles.Length);
			this.GetComponent<SpriteRenderer> ().sprite = tiles [rand];
			offScreen = new Vector3 (offScreen.x, tileHeights [rand], 0);
			transform.localPosition = offScreen;
		}
	}
}
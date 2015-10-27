using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class NPCController : MonoBehaviour
{

	private bool isStanding;
	private SpriteRenderer renderer;
	public Sprite crouch;
	public Sprite stand;
	public Sprite shoot;
	private PlayerController player;
	public float aimMin = 0.5f;
	public float aimMax = 1.5f;
	private static bool isToggling = false;
	private BarkTrigger barkTrigger;
	
	void Start ()
	{
		Random.seed = System.DateTime.Now.Millisecond * 10373289;
		renderer = this.GetComponent<SpriteRenderer> ();
		player = GameObject.FindObjectOfType<PlayerController> ();
		barkTrigger = this.GetComponent<BarkTrigger> ();
		InvokeRepeating ("Toggle", 3f, 0.5f);
	}


	void Toggle ()
	{
		if (!isToggling && AC.GlobalVariables.GetBooleanValue (42)) {
			isToggling = true;
			isStanding = !isStanding;
			if (isStanding) {
				this.renderer.sprite = stand;
				StartCoroutine (Aim ());
			} else {
				this.renderer.sprite = crouch;
				StartCoroutine (Wait ());
			}

		}

	}

	public void ForceToggle ()
	{
		isToggling = false;
		Toggle ();

	}

	IEnumerator Aim ()
	{
		float aimTime = Random.Range (aimMin * 10, aimMax * 10) / 10;
		yield return new WaitForSeconds (aimTime);
		this.renderer.sprite = shoot;
		if (player.IsStanding ()) {
			AC.GlobalVariables.SetIntegerValue (6, AC.GlobalVariables.GetIntegerValue (6) - 1);
			player.GetBarkTrigger ().TryBark (this.transform);
			//GameObject.Find ("PlayerHit").GetComponent <AC.Cutscene> ().Interact ();
		}
		StartCoroutine (Fire ());

	}

	IEnumerator Fire ()
	{
		yield return new WaitForSeconds (0.5f);
		isToggling = false;
	}

	
	IEnumerator Wait ()
	{
		int rand = Random.Range (3, 6);
		yield return new WaitForSeconds (rand);
		isToggling = false;
	}



	public bool IsStanding ()
	{
		return this.renderer.sprite == stand;
	}

	public BarkTrigger GetBarkTrigger ()
	{
		return barkTrigger;
	}
}

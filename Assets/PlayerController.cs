using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	private SpriteRenderer renderer;
	private NPCController enemy;
	public Sprite crouch;
	public Sprite stand;
	public Sprite shoot;
	private bool isStanding;
	private Target target;
	private SpriteRenderer targetRenderer;
	private bool waiting = false;
	public float shotDelayTime = 0.5f;
	
	void Start ()
	{
		renderer = this.GetComponent<SpriteRenderer> ();
		enemy = GameObject.FindObjectOfType<NPCController> ();
		target = GameObject.FindObjectOfType<Target> ();
		targetRenderer = target.GetComponent<SpriteRenderer> ();
	}

	void Update ()
	{
		if (AC.GlobalVariables.GetBooleanValue (42)) {
			if (Input.GetMouseButtonDown (0)) {
				if (isStanding && !waiting) {
					
					Shoot ();
					waiting = true;
					StartCoroutine (Wait ());
				}
			}
			
			if (Input.GetMouseButtonDown (1)) {
				isStanding = !isStanding;
				if (isStanding) {
					//target.GetComponent<Animator> ().SetInteger ("param", Random.Range (0, 3));
					//target.GetComponent<Animator> ().SetInteger ("param", 1);
					this.renderer.sprite = stand;
					//target.transform.position = enemy.transform.position;
					targetRenderer.enabled = true;
				} else {
					this.renderer.sprite = crouch;
					targetRenderer.enabled = false;
				}
			}

		}

	}

	IEnumerator Wait ()
	{
		yield return new WaitForSeconds (shotDelayTime);
		waiting = false;
	}

	private void Shoot ()
	{
		int numberOfBullets = AC.GlobalVariables.GetIntegerValue (5);
		if (numberOfBullets > 0) {
			this.renderer.sprite = shoot;
			AC.GlobalVariables.SetIntegerValue (5, numberOfBullets - 1);
			Target t = GameObject.FindObjectOfType<Target> ();
			Collider2D hitCollider = Physics2D.OverlapCircle (t.transform.position, t.GetComponent<CircleCollider2D> ().radius, 1 << LayerMask.NameToLayer ("Body Part"));

			if (hitCollider == null || !enemy.IsStanding ()) {
				GameObject.Find ("Missed").GetComponent <AC.Cutscene> ().Interact ();
				//MonoBehaviour.print ("miss");
			} else {
				BodyPart part = hitCollider.gameObject.GetComponent<BodyPart> ();
				GameObject.Find ("BeltHit").GetComponent <AC.Cutscene> ().Interact ();
				//Debug.Log (part.gameObject.name + " shot. You did " + part.damage + " damage");
				Shootable npc = hitCollider.gameObject.GetComponentInParent<Shootable> ();
				npc.DecreaseHP (part.damage);
				npc.GetComponent<NPCController> ().ForceToggle ();
			}
			
		} else {
			Application.LoadLevel ("losetobelt");
		}

	}

	public bool IsStanding ()
	{
		return isStanding;
	}
}

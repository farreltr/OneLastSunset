using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;
using AC;

public class PlayerController : MonoBehaviour
{

	private SpriteRenderer renderer;
	private NPCController enemy;
	public Sprite crouch;
	public Sprite stand;
	public Sprite shoot;
	private bool isStanding;
	private TargetMovement target;
	private SpriteRenderer targetRenderer;
	private bool waiting = false;
	public float shotDelayTime = 0.5f;
	private BarkTrigger barkTrigger;
	private float speed;
	private Animator animator;
	private Player player;
	
	void Start ()
	{
		renderer = this.GetComponent<SpriteRenderer> ();
		if (renderer == null) {
			this.GetComponentInChildren<SpriteRenderer> ();
		}
		enemy = GameObject.FindObjectOfType<NPCController> ();
		barkTrigger = this.GetComponent<BarkTrigger> ();
		isStanding = true;
		player = GetComponent<Player> ();
		if (player == null) {
			player = GetComponentInParent<Player> ();
		}
		if (player == null) {
			player = GetComponentInChildren<Player> ();
		}
		if (player != null) {
			speed = player.walkSpeedScale;
			animator = player.GetComponent<Animator> ();
		} else {
			speed = 2f;
		}
	}

	void Update ()
	{
		/*if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			transform.Translate ((Vector3.left * speed * Time.deltaTime));
			player.charState = CharState.Move;
		} else if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			transform.Translate ((Vector3.right * speed * Time.deltaTime));
			player.charState = CharState.Move;
		} else {
			player.charState = CharState.Idle;
		}*/
		if (AC.GlobalVariables.GetBooleanValue (42) || AC.GlobalVariables.GetBooleanValue (52)) {
			AC.KickStarter.playerMenus.DisableHotspotMenus ();
			if (Input.GetMouseButtonDown (0)) {
				if (isStanding && !waiting) {
					target = GameObject.FindObjectOfType<TargetMovement> ();
					targetRenderer = target.GetComponent<SpriteRenderer> ();
					Shoot ();
					waiting = true;
					StartCoroutine (Wait ());
				}
			}
			
			if (Input.GetMouseButtonDown (1)) {
				isStanding = !isStanding;
				target = GameObject.FindObjectOfType<TargetMovement> ();
				targetRenderer = target.GetComponent<SpriteRenderer> ();
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

		} else {
			TargetMovement t = FindObjectOfType<TargetMovement> ();
			if (t != null) {
				Destroy (t);
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
		if (numberOfBullets > 0 && AC.GlobalVariables.GetBooleanValue (52)) {
			this.renderer.sprite = shoot;
			AC.GlobalVariables.SetIntegerValue (5, numberOfBullets - 1);
			TargetMovement t = GameObject.FindObjectOfType<TargetMovement> ();
			Collider2D hitCollider = Physics2D.OverlapCircle (t.transform.position, t.GetComponent<CircleCollider2D> ().radius, 1 << LayerMask.NameToLayer ("Body Part"));

			//if (hitCollider == null || !enemy.IsStanding ()) {
			if (hitCollider == null) {
				//GameObject.Find ("Missed").GetComponent <AC.Cutscene> ().Interact ();
				MonoBehaviour.print ("miss");
			} else {
				BodyPart part = hitCollider.gameObject.GetComponent<BodyPart> ();
				enemy = part.GetComponentInParent<NPCController> ();
				enemy.GetBarkTrigger ().TryBark (this.transform);
				//GameObject.Find ("BeltHit").GetComponent <AC.Cutscene> ().Interact ();
				//Debug.Log (part.gameObject.name + " shot. You did " + part.damage + " damage");
				Shootable npc = hitCollider.gameObject.GetComponentInParent<Shootable> ();
				npc.DecreaseHP (part.damage);
				//npc.GetComponent<NPCController> ().ForceToggle ();
			}
			
		} else {
			Destroy (FindObjectOfType<TargetMovement> ().gameObject);
			AC.GlobalVariables.SetBooleanValue (52, false);
		}

	}

	public bool IsStanding ()
	{
		return isStanding;
	}

	public BarkTrigger GetBarkTrigger ()
	{
		return barkTrigger;
	}
}

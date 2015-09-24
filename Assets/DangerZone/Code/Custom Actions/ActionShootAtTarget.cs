using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	[System.Serializable]
	public class ActionShootAtTarget : Action
	{
		
		public ActionShootAtTarget ()
		{
			this.isDisplayed = true;
			title = "Target : Shoot";
		}
		
		
		override public float Run ()
		{
			int numberOfBullets = AC.GlobalVariables.GetIntegerValue (5);
			if (numberOfBullets > 0) {
				AC.GlobalVariables.SetIntegerValue (5, numberOfBullets - 1);
				Target t = GameObject.FindObjectOfType<Target> ();
				Collider2D hitCollider = Physics2D.OverlapCircle (t.transform.position, t.GetComponent<CircleCollider2D> ().radius, 1 << LayerMask.NameToLayer ("Body Part"));
				
				if (hitCollider == null) {
					MonoBehaviour.print ("miss");
				} else {
					BodyPart part = hitCollider.gameObject.GetComponent<BodyPart> ();
					Debug.Log (part.gameObject.name + " shot. You did " + part.damage + " damage");
					Shootable npc = hitCollider.gameObject.GetComponentInParent<Shootable> ();
					npc.DecreaseHP (part.damage);
					if (npc.hp > 0) {
						Debug.Log (npc.gameObject.name + "has " + npc.hp + " health left");
					}
					Debug.Log ("You have " + numberOfBullets + " bullets left.");
				}

			} else {
				Debug.Log ("You're out of bullets");
			}


			return 0f;
		}

		
		#if UNITY_EDITOR

		override public void ShowGUI ()
		{
			AfterRunningOption ();
		}
		
		
		public override string SetLabel ()
		{
			return (" (Shoot at target) ");
		}

		#endif
		
	}

}
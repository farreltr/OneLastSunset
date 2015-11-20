using UnityEngine;
using System.Collections;

public class Shootable : MonoBehaviour
{

	public int hp;
	
	void Start ()
	{
	
	}
	
	public void DecreaseHP (int i)
	{
		hp = hp - i;
		if (hp <= 0) {
			//
			this.gameObject.SetActive (false);
			//Application.LoadLevel ("win");
		}
	}
}

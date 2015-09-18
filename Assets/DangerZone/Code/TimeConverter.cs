using UnityEngine;
using System.Collections;

public class TimeConverter : MonoBehaviour
{

	private int gameOver = 0;

	void Start ()
	{
		gameOver = AC.GlobalVariables.GetVariable (35).val;
	}

	void Update ()
	{
		if (gameOver == 0) {
			UpdateTime ();
			UpdateHealth ();
			UpdateHunger ();
			UpdateGas ();
		}

	}

	void UpdateTime ()
	{
		string postfix = "AM";
		string prefix = "WEDNESDAY ";
		
		AC.GVar time = AC.GlobalVariables.GetVariable (16);
		if (time.floatVal <= 0) {
			gameOver = 1;
			Application.LoadLevel ("time");
		}
		AC.GVar formattedTime = AC.GlobalVariables.GetVariable (32);
		
		int hour = Mathf.FloorToInt (70 - time.floatVal + 14);
		int day = hour / 24;
		if (day == 1) {
			prefix = "THURSDAY ";
		}
		
		if (day == 2) {
			prefix = "FRIDAY ";
		}
		
		if (day == 3) {
			prefix = "SATURDAY ";
		}
		
		hour = hour % 24;
		if (hour > 12) {
			postfix = "PM";
			hour -= 12;
		}
		
		float hr = (float)Mathf.FloorToInt (time.floatVal);
		
		int minutes = Mathf.FloorToInt ((time.floatVal - hr) * 60f);
		
		string minStr = minutes.ToString ();
		if (minutes < 10) {
			minStr = "0" + minStr;
		}
		string timeString = prefix + hour + ":" + minStr + postfix;
		
		formattedTime.SetStringValue (timeString);
	}

	void UpdateHealth ()
	{
		AC.GVar health = AC.GlobalVariables.GetVariable (6);
		if (health.val <= 0) {
			gameOver = 1;
			Application.LoadLevel ("injury");
		}
		if (health.val > 3) {
			health.SetValue (3);
		}

		AC.MenuElement injured = AC.PlayerMenus.GetElementWithName ("Stats", "Injured");
		AC.MenuElement healthy = AC.PlayerMenus.GetElementWithName ("Stats", "Healthy");
		AC.MenuElement wounded = AC.PlayerMenus.GetElementWithName ("Stats", "Wounded");
		injured.isVisible = health.val == 1;
		wounded.isVisible = health.val == 2;
		healthy.isVisible = health.val == 3;
	}

	void UpdateHunger ()
	{
		AC.GVar hunger = AC.GlobalVariables.GetVariable (8);
		if (hunger.val <= 0) {
			gameOver = 1;
			Application.LoadLevel ("starve");
		}

		if (hunger.val > 3) {
			hunger.SetValue (3);
		}
		
		AC.MenuElement full = AC.PlayerMenus.GetElementWithName ("Stats", "Full");
		AC.MenuElement hungry = AC.PlayerMenus.GetElementWithName ("Stats", "Hungry");
		AC.MenuElement starving = AC.PlayerMenus.GetElementWithName ("Stats", "Starving");
		full.isVisible = hunger.val == 3;
		hungry.isVisible = hunger.val == 2;
		starving.isVisible = hunger.val == 1;
	}

	void UpdateGas ()
	{
		AC.GVar gas = AC.GlobalVariables.GetVariable (7);
		if (gas.val <= 0) {
			gameOver = 1;
			Application.LoadLevel ("fuel");
		}
	}


}

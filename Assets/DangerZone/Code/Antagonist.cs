using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Antagonist : MonoBehaviour
{
	private string[] locations = new string[70];
	private string randomLocation;

	void Start ()
	{
		Random.seed = System.DateTime.Now.Millisecond;
		DontDestroyOnLoad (this);
		locations [0] = "canyon";
		locations [1] = "canyon";
		locations [2] = "canyon";
		locations [3] = "canyon_reevesville";
		locations [4] = "canyon_reevesville";
		locations [5] = "canyon_reevesville";
		locations [6] = "canyon_reevesville";
		locations [7] = "canyon_reevesville";
		locations [8] = "reevesville";
		locations [9] = "reevesville";

		locations [10] = "reevesville_radlers";
		locations [11] = "reevesville_radlers";
		locations [12] = "reevesville_radlers";
		locations [13] = "radlers";
		locations [14] = "radlers_getsburg";
		locations [15] = "radlers_getsburg";
		locations [16] = "radlers_getsburg";
		locations [17] = "radlers_getsburg";
		locations [18] = "getsburg";
		locations [19] = "getsburg_connors";

		locations [20] = "getsburg_connors";
		locations [21] = "getsburg_connors";
		locations [22] = "getsburg_connors";
		locations [23] = "getsburg_connors";
		locations [24] = "getsburg_connors";
		locations [25] = "connors";
		locations [26] = "connors_rats";
		locations [27] = "connors_rats";
		locations [28] = "connors_rats";
		locations [29] = "connors_rats";

		locations [30] = "rats";
		locations [31] = "rats-oasis";
		locations [32] = "rats-oasis";
		locations [33] = "rats-oasis";
		locations [34] = "rats-oasis";
		locations [35] = "oasis";
		locations [36] = "oasis";
		locations [37] = "oasis";
		locations [38] = "oasis";
		locations [39] = "oasis";

		locations [40] = "oasis";
		locations [41] = "oasis_mine";
		locations [42] = "oasis_mine";
		locations [43] = "oasis_mine";
		locations [44] = "oasis_mine";
		locations [45] = "oasis_mine";
		locations [46] = "oasis_mine";
		locations [47] = "oasis_mine";
		locations [48] = "mine";
		locations [49] = "mine_rust";

		locations [50] = "mine_rust";
		locations [51] = "mine_rust";
		locations [52] = "mine_rust";
		locations [53] = "mine_rust";
		locations [54] = "mine_rust";
		locations [55] = "rust";
		locations [56] = "rust";
		locations [57] = "rust_cemetery";
		locations [58] = "rust_cemetery";
		locations [59] = "rust_cemetery";

		locations [60] = "rust_cemetery";
		locations [61] = "cemetery";
		locations [62] = "cemetery_ranch";
		locations [63] = "cemetery_ranch";
		locations [64] = "cemetery_ranch";
		locations [65] = "ranch";
		locations [66] = "ranch_ghost";
		locations [67] = "ranch_ghost";
		locations [68] = "ranch_ghost";
		locations [69] = "ghost";

		char[] splitChar = {'_'};
		string[] randomLocationArray = locations [Random.Range (0, locations.Length)].Split (splitChar);
		randomLocation = randomLocationArray [Random.Range (0, randomLocationArray.Length)];

	
	}

	/*public string GetPosition (float time)
	{
		int idx = Mathf.RoundToInt (time);
		if (idx < 70 && idx > -1) {
			return locations [idx];
		}
		return "";
	} */

	public string GetPosition (float time)
	{
		return randomLocation;
	}
}

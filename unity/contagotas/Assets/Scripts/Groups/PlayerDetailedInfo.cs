using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetailedInfo : MonoBehaviour {

	[SerializeField]
	Text playerName;

	[SerializeField]
	Text score;

	private int groupID;

	public void SetupPlayerInfo(string playerName, int score)
	{
		this.playerName.text = playerName;
		this.score.text = score.ToString();
	}

}

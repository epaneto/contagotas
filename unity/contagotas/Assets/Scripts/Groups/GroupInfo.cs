using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupInfo : MonoBehaviour {

	[SerializeField]
	Text groupName;
	[SerializeField]
	Text score;

	public void SetupGroupInfo(string groupName, int score)
	{
		this.groupName.text = groupName;
		this.score.text = score.ToString();
	}
}

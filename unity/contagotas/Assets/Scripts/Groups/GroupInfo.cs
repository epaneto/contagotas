﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupInfo : MonoBehaviour {

	[SerializeField]
	Text groupName;

	[SerializeField]
	Text score;

	[SerializeField]
	Button joinButton;

	private int groupID;

	public System.Action<int> joinGroupClicked = new System.Action<int>(delegate(int id) {});

	public void SetupGroupInfo(string groupName, int score, int groupID, bool hideJoinButton = true)
	{
		this.groupName.text = groupName;
		this.score.text = score.ToString();
		this.groupID = groupID;

		if (hideJoinButton) {
			joinButton.gameObject.SetActive(false);
		} else {
			joinButton.onClick.AddListener(HandleClick);
		}
	}

	private void HandleClick()
	{
		joinGroupClicked (groupID);
	}
}
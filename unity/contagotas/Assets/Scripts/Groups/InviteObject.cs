﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InviteObject : MonoBehaviour {

	[SerializeField]
	Text inviteText;

	[SerializeField]
	Button acceptButton;

	[SerializeField]
	Button declineButton;

	[SerializeField]
	Button closeButton;

	private int inviteID;

	public System.Action<int> acceptButtonClicked = new System.Action<int>(delegate(int id) {});

	public System.Action<int> declineButtonClicked = new System.Action<int>(delegate(int id) {});

	public System.Action<InviteObject> closeButtonClicked = new System.Action<InviteObject>(delegate(InviteObject inviteObject) {});

	public void SetupInviteInfo(string senderName, string groupName, int inviteId)
	{
		this.inviteText.text = senderName + this.inviteText.text + groupName;
		this.inviteID = inviteId;

		acceptButton.onClick.AddListener(HandleAcceptClick);
		declineButton.onClick.AddListener(HandleDeclineClick);
		closeButton.onClick.AddListener (HandleCloseClick);
	}

	private void HandleAcceptClick()
	{
		acceptButtonClicked (inviteID);
	}

	private void HandleDeclineClick()
	{
		declineButtonClicked (inviteID);
	}

	private void HandleCloseClick()
	{
		closeButtonClicked (this);
	}


}

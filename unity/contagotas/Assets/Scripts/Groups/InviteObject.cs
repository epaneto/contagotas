using System.Collections;
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

	private int inviteID;

	public System.Action<int> acceptButtonClicked = new System.Action<int>(delegate(int id) {});

	public System.Action<int> declineButtonClicked = new System.Action<int>(delegate(int id) {});

	public void SetupInviteInfo(string senderName, string groupName, int inviteId)
	{
		this.inviteText.text = senderName + this.inviteText.text + groupName;
		this.inviteID = inviteId;

		acceptButton.onClick.AddListener(HandleAcceptClick);
		declineButton.onClick.AddListener(HandleDeclineClick);
	}

	private void HandleAcceptClick()
	{
		acceptButtonClicked (inviteID);
	}

	private void HandleDeclineClick()
	{
		declineButtonClicked (inviteID);
	}

}

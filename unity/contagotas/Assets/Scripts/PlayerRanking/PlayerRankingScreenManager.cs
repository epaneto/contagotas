using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Facebook.Unity;
using Facebook.MiniJSON;
using System.Linq;
using System.Text;

public enum PlayerRankingScreenType 
{
	LOADING,
	EXISTING_GROUP,
	NEW_GROUP,
	CREATE_GROUP,
	JOIN_GROUP,
	CONFIRM_JOIN_GROUP,
	ERROR,
	RECEIVE_INVITES,
	SEND_INVITES
}

public class PlayerRankingScreenManager : MonoBehaviour {


	[Header("Loading Screen References")]
	[SerializeField]
	PlayerRankingBaseAssetsGroupManager loadingScreenManager;

	[Header("Error Screen References")]
	[SerializeField]
	PlayerRankingErrorScreenManager errorScreenManager;

	[Header("Existing Screen References")]
	[SerializeField]
	PlayerRankingScoreScreenManager existingGroupScreenManager;

	// Use this for initialization
	void Start () {
		InitiliazeManagers ();
	}

	private void InitiliazeManagers()
	{
		loadingScreenManager.Initialize(this);
		errorScreenManager.Initialize(this);
		existingGroupScreenManager.Initialize(this);
	}


	public void ShowScreen(PlayerRankingScreenType groupToShow)
	{
		loadingScreenManager.SetScreen (groupToShow == PlayerRankingScreenType.LOADING);
		errorScreenManager.SetScreen(groupToShow == PlayerRankingScreenType.ERROR);
		existingGroupScreenManager.SetScreen (groupToShow == PlayerRankingScreenType.EXISTING_GROUP);
	}

	public void ShowLoadingScreen()
	{
		ShowScreen (PlayerRankingScreenType.LOADING);
	}

	public void ShowErrorScreen(string error_message)
	{
		errorScreenManager.SetErrorMessage (error_message);
		ShowScreen (PlayerRankingScreenType.ERROR);
	}
}

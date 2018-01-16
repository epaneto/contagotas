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
	PLAYER_RANKING_SCREEN,
	ERROR,
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
	PlayerRankingScoreScreenManager playerRankingScreenManager;

	// Use this for initialization
	void Start () {
		InitiliazeManagers ();
		playerRankingScreenManager.StartLoadRanking ();
		ShowLoadingScreen ();
	}

	private void InitiliazeManagers()
	{
		loadingScreenManager.Initialize(this);
		errorScreenManager.Initialize(this);
		playerRankingScreenManager.Initialize(this);
	}


	public void ShowScreen(PlayerRankingScreenType groupToShow)
	{
		loadingScreenManager.SetScreen (groupToShow == PlayerRankingScreenType.LOADING);
		errorScreenManager.SetScreen(groupToShow == PlayerRankingScreenType.ERROR);
		playerRankingScreenManager.SetScreen (groupToShow == PlayerRankingScreenType.PLAYER_RANKING_SCREEN);
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

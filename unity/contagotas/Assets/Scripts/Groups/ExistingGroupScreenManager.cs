using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class ExistingGroupScreenManager : BaseAssetsGroupManager {

	[SerializeField]
	Text groupName;

	[SerializeField]
	Text groupScore;

	[SerializeField]
	GameObject PlayerDataObject;

	[SerializeField]
	private Transform playerDataParentTransform;

	private GroupData groupData;

	[SerializeField]
	GameObject MyGroupContent;

	[SerializeField]
	GameObject RankingContent;

	[SerializeField]
	GameObject LoadingContent;

	public void StartLeaveGroup()
	{
		StartCoroutine (LeaveGroup (PlayerPrefs.GetInt ("user_id")));
	}

	IEnumerator LeaveGroup(int userID)
	{
		screenManager.ShowLoadingScreen ();
		WWW result;
		yield return result = WWWUtils.DoWebRequest ("leave/" + userID + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper().Contains("SUCCESS")) 
		{
			screenManager.ShowGroup (ScreenType.NEW_GROUP);	
		}
		else {
			screenManager.ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		}
	}

	IEnumerator GetGroupInfo()
	{
		SetButtonOn (groupButton);
		SetButtonOff (rankingButton);
		LoadingContent.SetActive (true);
		MyGroupContent.SetActive (false);
		RankingContent.SetActive (false);

		WWW result;
		yield return result = WWWUtils.DoWebRequest ("score/get_detailed/" + groupData.Id + "/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper ().Contains ("ERROR") || result.text.ToUpper ().Contains ("TIMEOUT")) {
			screenManager.ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		} 

		string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

		List<PlayerInfo> foundPlayers = JsonConvert.DeserializeObject<List<PlayerInfo>>(json);

		foreach (var item in screenManager.temporaryObjsList) {
			Destroy (item);
		}
		screenManager.temporaryObjsList.Clear ();

		foreach (var player in foundPlayers) {
			GameObject item = Instantiate (PlayerDataObject, playerDataParentTransform);
			screenManager.temporaryObjsList.Add (item);
			PlayerDetailedInfo playerInfo = item.GetComponent<PlayerDetailedInfo>(); 
			playerInfo.SetupPlayerInfo (player.Name, player.Score);
		}
		LoadingContent.SetActive (false);
		MyGroupContent.SetActive (true);
		RankingContent.SetActive (false);
		screenManager.ShowGroup (ScreenType.EXISTING_GROUP);
	}

	public void ShowExistingGroup(GroupData info)
	{
		groupData = info;
		groupName.text = info.Name;
		groupScore.text = info.Score.ToString();
		StartCoroutine (GetGroupInfo ());
	}

	public void ShowMyGroupInfo()
	{
		LoadingContent.SetActive (false);
		MyGroupContent.SetActive (true);
		RankingContent.SetActive (false);
		SetButtonOn (groupButton);
		SetButtonOff (rankingButton);
	}

	public void ShowRankingScreen()
	{
		StartCoroutine (LoadRanking ());
	}

	public void ShowSendInviteScreen()
	{
		screenManager.ShowSendInviteScreen ();
	}

	public void GoToMapScreen()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Map");
	}
	#region RANKING

	[SerializeField]
	Transform RankingGroupParent;

	[SerializeField]
	Text[] GroupsName;
	[SerializeField]
	Text[] GroupsScore;

	public void StartLoadRanking()
	{
		StartCoroutine (LoadRanking ());
	}

	IEnumerator LoadRanking()
	{
		LoadingContent.SetActive (true);
		MyGroupContent.SetActive (false);
		RankingContent.SetActive (false);
		SetButtonOn (rankingButton);
		SetButtonOff (groupButton);

		WWW result;
		yield return result = WWWUtils.DoWebRequest("score/top/");
		Debug.Log ("url result = " + result.text);

		if (result.text.ToUpper ().Contains ("ERROR")) {
			screenManager.ShowErrorScreen ("error leaving group:" + result.text);
			yield break;
		} else {

			//clearing all texts
			for (int i = 0; i < 8; i++) {
				GroupsName [i].text = "";
				GroupsScore [i].text = "";
			}

			string json = StringUtils.DecodeBytesForUTF8 (result.bytes);

			List<GroupData> rankedGroups = JsonConvert.DeserializeObject<List<GroupData>>(json);

			int index = 0;
			foreach (var group in rankedGroups) {
				GroupsName [index].text = group.Name;
				GroupsScore [index].text = group.Score.ToString();
				index++;
			}

			LoadingContent.SetActive (false);
			MyGroupContent.SetActive (false);
			RankingContent.SetActive (true);
		}
	}

	#endregion




	#region ButtonsBehaviour

	[SerializeField]
	Sprite buttonOnSprite;
	[SerializeField]
	Sprite buttonOffSprite;

	[SerializeField]
	Button groupButton;
	[SerializeField]
	Button rankingButton;

	[SerializeField]
	Color onColor;
	[SerializeField]
	Color offColor;

	void SetButtonOn(Button button)
	{
		button.image.sprite = buttonOnSprite;
		Text textComponent = button.GetComponentInChildren<Text> ();
		textComponent.color = onColor;
		textComponent.text = textComponent.text;
	}

	void SetButtonOff(Button button)
	{
		button.image.sprite = buttonOffSprite;
		Text textComponent = button.GetComponentInChildren<Text> ();
		textComponent.color = offColor;
		textComponent.text = textComponent.text;
	}

	#endregion



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using Newtonsoft.Json;
using Facebook.MiniJSON;
using Facebook;
using System.Linq;

public class FacebookInviteManager : MonoBehaviour {

	[SerializeField]
	GameObject inviteButton;

	List<string> perms = new List<string>(){"public_profile", "email", "user_friends"};

	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		if (!FB.IsInitialized) {
			InitiliazeFacebook ();
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}

	public void InitiliazeFacebook()
	{
		inviteButton.SetActive(false);
		// Initialize the Facebook SDK
		FB.Init(InitCallback, OnHideUnity);
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			Debug.Log("Facebook Initialized");

			if (!FB.IsLoggedIn) {
				DoLogin ();
			}
		} else {
			Debug.Log("Failed to Initialize the Facebook SDK");
		}
	}

	public void DoLogin()
	{
		FB.LogInWithReadPermissions(perms, AuthCallback);
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	private void AuthCallback (ILoginResult result) {
		if (FB.IsLoggedIn) {
			// AccessToken class will have session details
			var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
			// Print current access token's User ID
			Debug.Log(aToken.UserId);
			inviteButton.SetActive (true);
		}
		else {
			Debug.Log("User cancelled login");
		}
	}

	public void GetFriends(FacebookDelegate<IGraphResult> callBackDelegate)
	{
		FB.API("me/friends?fields=installed,name,picture", HttpMethod.GET, callBackDelegate);    
	}

	public List<object> FriendListReceived(IResult result)
	{
		var dict = Json.Deserialize(result.RawResult)
			as Dictionary<string,object>;

		var friendList = new List<object>();
		return friendList = (List<object>)(dict["data"]);

	}


}

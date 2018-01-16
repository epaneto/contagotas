using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using Newtonsoft.Json;
using Facebook.MiniJSON;
using Facebook;
using System.Linq;

public class FacebookManager : MonoBehaviour {

	public Text feedbackStatus; 
	public Image profilePicture;
	[SerializeField]
	GameObject loginButton;
	[SerializeField]
	GameObject logoutButton;
	[SerializeField]
	GameObject inviteButton;
	[SerializeField]
	GameObject retryButton;

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
		profilePicture.enabled = false;
		loginButton.SetActive(false);
		logoutButton.SetActive (false);
		inviteButton.SetActive(false);
		retryButton.SetActive (false);
		// Initialize the Facebook SDK
		FB.Init(InitCallback, OnHideUnity);
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			feedbackStatus.text = "Facebook Initialized";
			Debug.Log("Facebook Initialized");

			if (FB.IsLoggedIn) {
				LoadProfile ();
			} else {
				profilePicture.enabled = false;
				loginButton.SetActive (true);
				logoutButton.SetActive (false);
				inviteButton.SetActive (false);
				retryButton.SetActive (false);
			}
			// Continue with Facebook SDK
			// ...
		} else {
			feedbackStatus.text = "Failed to Initialize the Facebook SDK";
			Debug.Log("Failed to Initialize the Facebook SDK");
			retryButton.SetActive (true);
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
			profilePicture.enabled = false;
			loginButton.SetActive (false);
			logoutButton.SetActive (true);
			inviteButton.SetActive (true);
			retryButton.SetActive (false);
			// Print current access token's granted permissions
			/*foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}
			*/

			if (FB.IsLoggedIn) {
				LoadProfile ();
			}
		} else {
			feedbackStatus.text = "User cancelled login";
			Debug.Log("User cancelled login");
		}
	}

	private void LoadProfile()
	{
		FB.API ("/me?fields=name", HttpMethod.GET, DispName); 
		FB.API ("/me/picture?type=square&height=128&width=128", HttpMethod.GET, GetPicture);
	}


	private void DispName(IResult result)
	{
		feedbackStatus.enabled = true;
		if(result.Error != null)
		{
			feedbackStatus.text = result.Error.ToString();
		}
		else
		{
			feedbackStatus.text = "My Name is " + result.ResultDictionary["name"];
		}
	}

	private void GetPicture(IGraphResult result)
	{
		
		if (result.Error == null && result.Texture != null) {
			profilePicture.enabled = true;
			profilePicture.sprite = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 ());
		}
	}

	public void InviteFriends()
	{
		FB.AppRequest (
			message:"This game is awesome, join now",
			title:"Chame seus amigos:"
		);
	}

	public void Logout()
	{
		profilePicture.enabled = false;
		loginButton.SetActive (true);
		logoutButton.SetActive (false);
		inviteButton.SetActive (false);
		retryButton.SetActive (false);
		FB.LogOut ();
	}

	public void GetFriends()
	{
		FB.API("me/friends?fields=installed,name,picture", HttpMethod.GET, FriendListReceived);    
	}

	public void FriendListReceived(IResult result)
	{
		var dict = Json.Deserialize(result.RawResult)
			as Dictionary<string,object>;

		var friendList = new List<object>();
		friendList = (List<object>)(dict["data"]);

		//Dictionary<string,object> friends = friendList as Dictionary<string,object>;

		foreach (var friend in friendList) {

			var info = ((IEnumerable)friend).Cast<object> ()
				.Select (x => x.ToString ())
				.ToArray ();

			string name = info [1].Replace ('[', ' ').Replace (']', ' ').Trim ().Split (',') [1];
			string facebook_id = info [3].Replace ('[', ' ').Replace (']', ' ').Trim ().Split (',') [1];

			//Debug.Log (info [0] + "" + info [1] + "" + info [2]);
			//Debug.Log (info [4] );
			//Debug.Log(name + "facebook = " + facebook_id);
		}
		//var friends = JsonConvert.DeserializeObject<List<data>>(result.ResultDictionary["data"].ToString());

	}
		

}

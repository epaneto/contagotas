using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using Newtonsoft.Json;
using Facebook.MiniJSON;
using Facebook;
using System.Linq;

public class FacebookLoginManager : MonoBehaviour {

	public Text feedbackStatus; 
	public Image profilePicture;

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
		// Initialize the Facebook SDK
		FB.Init(InitCallback, OnHideUnity);
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			Debug.Log("Facebook Initialized");

			if (FB.IsLoggedIn) {
				Debug.Log ("Ja logado como facebook");
			} 			// Continue with Facebook SDK
			// ...
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
			// Print current access token's granted permissions
			/*foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}
			*/

			//if (PlayerPrefs.HasKey ("signed") && PlayerPrefs.GetInt("signed") == 1)
			//	Debug.Log ("Should Go to Next Scene");
			//else {
				LoadUserInfo ();
//			}

		} else {
			Debug.Log("User cancelled login");
		}
	}

	private void LoadUserInfo()
	{
		//FB.API ("/location?fields=city,state", HttpMethod.GET, ReceivedUserInfoLocation);
		FB.API ("/me?fields=name,email,id", HttpMethod.GET, ReceivedUserInfo); 
	}

	/*private void ReceivedUserInfoLocation(IResult result)
	{
		if (result.Error != null) {
			Debug.Log (result.Error.ToString ());
		} else {
			string city = result.ResultDictionary ["city"].ToString ();
			string state = result.ResultDictionary ["state"].ToString ();

			PlayerPrefs.SetString ("city", city);
			PlayerPrefs.SetString ("state", state);

			//FB.API ("/me?fields=name,email,id,location", HttpMethod.GET, ReceivedUserInfo); 

		}
	}*/

	private void ReceivedUserInfo(IResult result)
	{
		if(result.Error != null)
		{
			Debug.Log(result.Error.ToString());
		}
		else
		{
			string playerName = "";
			if(result.ResultDictionary.ContainsKey("name"))
				playerName = result.ResultDictionary ["name"].ToString();

			string email = "";
			if(result.ResultDictionary.ContainsKey("email"))
				email = result.ResultDictionary ["email"].ToString();
			
			string facebookid = "";
			if(result.ResultDictionary.ContainsKey("id"))
				facebookid = result.ResultDictionary ["id"].ToString();

			PlayerPrefs.SetInt ("signed", 1);
			PlayerPrefs.SetString ("user_name", playerName);
			PlayerPrefs.SetString ("user_email", email);
			PlayerPrefs.SetString ("user_facebookid", facebookid);

			UnityEngine.SceneManagement.SceneManager.LoadScene ("SignUp");
			//StartCoroutine (DoRegister(playerName,email, city,facebookid));
		}
	}

	public void Logout()
	{
		FB.LogOut ();
	}

}

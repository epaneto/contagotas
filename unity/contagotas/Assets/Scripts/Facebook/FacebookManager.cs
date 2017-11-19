using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FacebookManager : MonoBehaviour {

	public Text feedbackStatus; 
	public Image profilePicture;

	List<string> perms = new List<string>(){"public_profile", "email", "user_friends"};

	// Awake function from Unity's MonoBehavior
	void Awake ()
	{
		if (!FB.IsInitialized) {
			// Initialize the Facebook SDK
			FB.Init(InitCallback, OnHideUnity);
		} else {
			// Already initialized, signal an app activation App Event
			FB.ActivateApp();
		}
	}

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp();
			feedbackStatus.text = "Facebook Initialized";
			Debug.Log("Facebook Initialized");
			// Continue with Facebook SDK
			// ...
		} else {
			feedbackStatus.text = "Failed to Initialize the Facebook SDK";
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
			foreach (string perm in aToken.Permissions) {
				Debug.Log(perm);
			}

			if (FB.IsLoggedIn) {
				FB.API ("/me?fields=name", HttpMethod.GET, DispName); 
				FB.API ("/me/picture?type=square&height=128&width=128", HttpMethod.GET, GetPicture);
			}
		} else {
			feedbackStatus.text = "User cancelled login";
			Debug.Log("User cancelled login");
		}
	}

	private void DispName(IResult result)
	{
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
			profilePicture.sprite = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 ());
		}
	}



}

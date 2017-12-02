﻿using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

public class NativeShare : MonoBehaviour {

	public string subject,ShareMessage,url;
	private bool isProcessing = false;

	public void Share()
	{
		#if UNITY_ANDROID
		if(!isProcessing)
			StartCoroutine( ShareScreenshot() );
		#elif UNITY_IOS
		if(!isProcessing)
		StartCoroutine( CallSocialShareRoutine() );
		#else
		Debug.Log("No sharing set up for this platform.");
		#endif
		}
		#if UNITY_ANDROID

		public IEnumerator ShareScreenshot()
		{
			isProcessing = true;
			// wait for graphics to render
			yield return new WaitForEndOfFrame();


			if(!Application.isEditor)
			{
				AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
				AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
				intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
				intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), ShareMessage);
				AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Convide seus amigos!");
				currentActivity.Call("startActivity", jChooser);
			}
			isProcessing = false;
		}

		#endif
		#if UNITY_IOS
		public struct ConfigStruct
		{
			public string title;
			public string message;
		}

		[DllImport ("__Internal")] private static extern void showAlertMessage(ref ConfigStruct conf);
		public struct SocialSharingStruct
		{
			public string text;
			public string url;
			public string subject;
		}

		[DllImport ("__Internal")] private static extern void showSocialSharing(ref SocialSharingStruct conf);
		public void CallSocialShare(string title, string message)
		{
			ConfigStruct conf = new ConfigStruct();
			conf.title = title;
			conf.message = message;
			showAlertMessage(ref conf);
			isProcessing = false;
		}

		public static void CallSocialShareAdvanced(string defaultTxt, string subject, string url)
		{
			SocialSharingStruct conf = new SocialSharingStruct();
			conf.text = defaultTxt; 
			conf.url = url;
			conf.subject = subject;
			showSocialSharing(ref conf);
		}

		IEnumerator CallSocialShareRoutine()
		{
			isProcessing = true;
			yield return new WaitForSeconds(0.1f);
			CallSocialShareAdvanced(ShareMessage, subject, url);
		}
		#endif
	}
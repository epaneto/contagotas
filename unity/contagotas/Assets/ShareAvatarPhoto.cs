using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ShareAvatarPhoto : MonoBehaviour {

    public GameObject menuObject;
    public GameObject optionsObject;
    public GameObject confirmButton;
    public GameObject shareButton;
    public GameObject titleObject;
	public GameObject screenshotTakenText;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public string ScreenshotName = "screenshot.png";

    public void ShareScreenshotWithText(string text)
    {
        string screenShotPath = Application.persistentDataPath + "/" + ScreenshotName;
        if (File.Exists(screenShotPath)) File.Delete(screenShotPath);

        menuObject.SetActive(false);
        optionsObject.SetActive(false);
        confirmButton.SetActive(false);
        shareButton.SetActive(false);
        titleObject.SetActive(false);
		screenshotTakenText.SetActive (false);

        #if UNITY_ANDROID
        ScreenshotManager.SaveScreenshot("avatar", "ContaGotas");
		StartCoroutine(ShowText());
		StartCoroutine(HideText());
		#elif UNITY_IOS
			ScreenCapture.CaptureScreenshot(ScreenshotName);
			StartCoroutine(delayedShare(screenShotPath, text));
		#endif
    }

    IEnumerator ScreenShotEncode()
    {
        yield return new WaitForEndOfFrame();
        string datacion = System.DateTime.Now.ToFileTime().ToString();
        ScreenCapture.CaptureScreenshot(datacion + ".png");
    }


    //CaptureScreenshot runs asynchronously, so you'll need to either capture the screenshot early and wait a fixed time
    //for it to save, or set a unique image name and check if the file has been created yet before sharing.
    IEnumerator delayedShare(string screenShotPath, string text)
    {
        while (!File.Exists(screenShotPath))
        {
            yield return new WaitForSeconds(.05f);
        }

        menuObject.SetActive(true);
        optionsObject.SetActive(true);
        confirmButton.SetActive(true);
        shareButton.SetActive(true);
        titleObject.SetActive(true);

		NativeSharePhoto.Share("", screenShotPath, "", "", "image/png", true, "");

    }

	IEnumerator ShowText()
	{
		yield return new WaitForSeconds(0.3f);
		screenshotTakenText.SetActive(true);
	}

	IEnumerator HideText()
	{
		yield return new WaitForSeconds(3f);
		menuObject.SetActive(true);
		optionsObject.SetActive(true);
		confirmButton.SetActive(true);
		shareButton.SetActive(true);
		titleObject.SetActive(true);
		screenshotTakenText.SetActive(false);
	}

}

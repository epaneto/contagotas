using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public CanvasGroup transitionCanvas;
	public float fadeDuration = 1f;
	public string startingSceneName = "Home";

	public event Action BeforeSceneUnload;
	public event Action AfterSceneLoad;

	private bool isFading;

	private Animator anim;

	public static SceneController sceneController;

	void Awake() {
		if (sceneController == null) {
			DontDestroyOnLoad (gameObject);
			sceneController = this;
		} else if (sceneController != this){
			Destroy (gameObject);
		}
	}

	private IEnumerator Start () {
		anim = GameObject.Find("transition").GetComponent<Animator> ();
		anim.Play ("transition_empty");

		transitionCanvas.alpha = 1f;
		yield return StartCoroutine (LoadSceneAndSetActive (startingSceneName));
		StartCoroutine (Fade (0f));
	}

	public void FadeAndLoadScene (string sceneName, bool hasTransition = false)
	{
		if (!isFading)
		{
			StartCoroutine (FadeAndSwitchScenes (sceneName, hasTransition));
		}
	}

	private IEnumerator FadeAndSwitchScenes (string sceneName, bool hasTransition)
	{
		if(hasTransition)
			anim.Play ("transition_in");
		yield return StartCoroutine (Fade (1f));
		if (BeforeSceneUnload != null)
			BeforeSceneUnload ();
		
		yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
		yield return StartCoroutine (LoadSceneAndSetActive (sceneName));

		if (AfterSceneLoad != null)
			AfterSceneLoad ();

		if(hasTransition)
			anim.Play ("transition_out");
		yield return StartCoroutine (Fade (0f));
	}

	private IEnumerator LoadSceneAndSetActive (string sceneName)
	{
		yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);
		Scene newlyLoadedScene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);
		SceneManager.SetActiveScene (newlyLoadedScene);
	}

	private IEnumerator Fade(float finalAlpha) {
		isFading = true;
		transitionCanvas.blocksRaycasts = true;

		float fadeSpeed = Mathf.Abs (transitionCanvas.alpha - finalAlpha) / fadeDuration;

		while(!Mathf.Approximately(transitionCanvas.alpha,finalAlpha))
		{
			transitionCanvas.alpha = Mathf.MoveTowards (transitionCanvas.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
			yield return null;
		}

		isFading = false;
		transitionCanvas.blocksRaycasts = false;
	}

}

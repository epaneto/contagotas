using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using DG.Tweening;

public class MinigamesController : MonoBehaviour {

	private string missionJSONFile = "missoes.json";
	List<string> minigames;
	public int minigameIndex;

    public GameObject timeBar;
    public GameObject timeBarFill;

	JArray Missions;
	GameObject endGame;
	GameObject loseGame;
	GameObject loseGameSadChar;
	GameObject scoreTxt;
	GameObject continueButton;
	GameObject continueLoseButton;
	GameObject titleTxt;
    GameObject hintTxt;
    GameObject challengeTxt;
    Text endGametxt;

    int playerScore;
    float scoreAnimationDuration = 0.6f;

	// Use this for initialization
	void Start () {
		endGame = GameObject.Find ("EndGame");
		scoreTxt = GameObject.Find ("score_txt");
		continueButton = GameObject.Find ("bt_continuar_score");
		continueLoseButton = GameObject.Find ("bt_continuar_lose");
		loseGame = GameObject.Find ("GameLose");
		loseGameSadChar = GameObject.Find ("character_sad");
        endGametxt = scoreTxt.GetComponent<Text>();

        titleTxt = GameObject.Find("titulo_txt");
        hintTxt = GameObject.Find("hint_txt");
        challengeTxt = GameObject.Find("desafio_txt");

        timeBar.SetActive(false);

		///load mission json
		TextAsset t = (TextAsset) Resources.Load("missoes", typeof(TextAsset));
		string DataAsJSON = t.text;

		JObject o = JObject.Parse(DataAsJSON);
		Missions = (JArray) o["missoes"];


		endGame.SetActive (false);
		loseGame.SetActive (false);

		minigameIndex = 0;

		JToken sceneName = Missions[minigameIndex]["sceneid"];
        SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);
	}

	public void ShowLose()
	{
		///REMOVE ACTIVE MINIGAME FROM SCREEN
		JToken sceneName = Missions[minigameIndex]["sceneid"];
		SceneManager.UnloadSceneAsync (sceneName.ToString());

		loseGame.SetActive (true);

		SkeletonGraphic graphic = loseGameSadChar.GetComponent<SkeletonGraphic> ();
		graphic.AnimationState.SetAnimation(0,"sad",false);

		continueLoseButton.GetComponent<Button> ().onClick.AddListener (PlayNextMiniGame);
	}

	public void ShowResults(int score)
	{
		///REMOVE ACTIVE MINIGAME FROM SCREEN
		JToken sceneName = Missions[minigameIndex]["sceneid"];
        SceneManager.UnloadSceneAsync(sceneName.ToString());

		JToken titleString = Missions[minigameIndex]["title"];
        Text titleField = titleTxt.GetComponent<Text> ();
        titleField.text = titleString.ToString ();

        JToken hintString = Missions[minigameIndex]["hint"];
        Text hintField = hintTxt.GetComponent<Text>();
        hintField.text = hintString.ToString();

        Text challengeField = challengeTxt.GetComponent<Text>();
        challengeField.text = "Desafio Real #" + (minigameIndex + 1).ToString();

		endGame.SetActive(true);

        playerScore = 0;
        StartCoroutine("CountTo", score);

		continueButton.GetComponent<Button> ().onClick.AddListener (PlayNextMiniGame);
	}

    IEnumerator CountTo(int target)
    {
        int start = playerScore;
        float timeBarScale = timeBarFill.transform.localScale.x;

        for (float timer = 0; timer < scoreAnimationDuration; timer += Time.deltaTime)
        {
            float progress = timer / scoreAnimationDuration;
            float barProgress = timeBarScale - (timeBarScale * progress);
            timeBarFill.transform.localScale = new Vector3(barProgress, 1, 1);

            playerScore = (int)Mathf.Lerp(start, target, progress);
            yield return null;
        }
        playerScore = target;
    }

    void OnGUI()
    {
        ///SHOW END GAME
        endGametxt.text = playerScore.ToString();
    }


    public void PlayNextMiniGame()
    {
        ///if user came from win window, we need to hide
        hideTime();

        continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        continueLoseButton.GetComponent<Button>().onClick.RemoveAllListeners();

        endGame.SetActive(false);
        loseGame.SetActive(false);

        if (minigameIndex + 1 < Missions.Count)
        {
            //Debug.Log ("play next minigame " + minigameIndex);

            minigameIndex++;


            JToken sceneName = Missions[minigameIndex]["sceneid"];
            SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);

        }
        else
        {
            //Debug.Log ("that was the last minigame, show map");
            SceneController.sceneController.FadeAndLoadScene("Map", true);
        }
    }

    public void blinkTimeBar()
    {
        Image timeBarFillMaterial = timeBarFill.GetComponent<Image>();

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(timeBarFillMaterial.DOColor(Color.red, 0.2f));
        mySequence.AppendInterval(0.2f);
        mySequence.Append(timeBarFillMaterial.DOColor(Color.white, 0.2f));
    }

    public void showTime()
    {
        timeBar.SetActive(true);
    }

    public void hideTime()
    {
        timeBar.SetActive(false);
    }

    public void updateTime(float barScale)
    {
        timeBarFill.transform.localScale = new Vector3(barScale, 1, 1);
    }

}
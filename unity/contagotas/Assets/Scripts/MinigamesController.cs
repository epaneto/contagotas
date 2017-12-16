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
    GameObject successTitle;
    GameObject scoreGroup;
    GameObject scoreTxt;
    GameObject continueButton;
    GameObject titleTxt;
    GameObject hintTxt;
    GameObject challengeTxt;
    GameObject endBackground;

    GameObject toogleButton;
    Toggle toogleComponent;


    GameObject loseGame;
	GameObject loseGameSadChar;
    GameObject continueLoseButton;

    Text endGametxt;

    int playerScore;
    float scoreAnimationDuration = 0.6f;

	// Use this for initialization
	void Start () {
		endGame = GameObject.Find ("EndGame");

        successTitle = GameObject.Find("success_title");
        scoreGroup = GameObject.Find("score_group");
        titleTxt = GameObject.Find("titulo_txt");
        challengeTxt = GameObject.Find("desafio_txt");
        hintTxt = GameObject.Find("hint_txt");
        continueButton = GameObject.Find("bt_continuar_score");
        endBackground = GameObject.Find("endgamebackground");
        toogleButton = GameObject.Find("routine_toogle");
        toogleComponent = toogleButton.GetComponent<Toggle>();

		scoreTxt = GameObject.Find ("score_txt");
        endGametxt = scoreTxt.GetComponent<Text>();
		
        loseGame = GameObject.Find("GameLose");
        continueLoseButton = GameObject.Find("bt_continuar_lose");
        loseGameSadChar = GameObject.Find("character_sad");

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
        successTitle.transform.DOScale(new Vector3(0,0,1),0.7f).SetEase(Ease.OutBack).From();
        scoreGroup.transform.DOScale(new Vector3(0, 0, 1), 0.8f).SetEase(Ease.OutBack).From();
        titleTxt.transform.DOScale(new Vector3(0, 0, 1), 0.9f).SetEase(Ease.OutBack).From();
        challengeTxt.transform.DOScale(new Vector3(0, 0, 1), 1.0f).SetEase(Ease.OutBack).From();
        hintTxt.transform.DOScale(new Vector3(0, 0, 1), 1.1f).SetEase(Ease.OutBack).From();
        continueButton.transform.DOScale(new Vector3(0, 0, 1.2f), 1.0f).SetEase(Ease.OutBack).From();
        toogleButton.transform.DOScale(new Vector3(0, 0, 1.2f), 1.1f).SetEase(Ease.OutBack).From();
        endBackground.transform.DOMoveY(-1400,0.6f).SetEase(Ease.OutQuad).From();

        ///CHECK ACTIVITY STATUS
        //Debug.Log("prefs is for mission" + minigameIndex + " : " + PlayerPrefs.HasKey("mission" + minigameIndex));
        toogleComponent.isOn = false;
        if (PlayerPrefs.HasKey("mission" + minigameIndex))
        {
            toogleComponent.isOn = true;
        }
        
            
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


        ////SEND POINTS TO SERVER
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
        }
        else
        {
            if (!PlayerPrefs.HasKey("user_id"))
            {
                Debug.Log("ERROR: Player não contem user id");
                return;
            }

			var hasGroup = PlayerPrefs.HasKey ("group_id");
			if (!hasGroup)
            {
                //player não tem grupo, nao precisa registrar score de grupo
                return;
            }

            StartCoroutine(AddScore(PlayerPrefs.GetInt("group_id"), score));
        }
	}

    IEnumerator AddScore(int groupid, int score)
    {
        WWW result;
        yield return result = WWWUtils.DoWebRequest("score/" + groupid.ToString() + "/" + score.ToString() + "/" + PlayerPrefs.GetInt("user_id") + "/");
        Debug.Log("url result = " + result.text);

        if (result.text == "success")
            Debug.Log("Inserido score");
        else
            Debug.Log("ERROR:" + result.text);
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
        GameSound.gameSound.PlaySFX("button");

        ///if user came from win window, we need to hide
        hideTime();

        continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        continueLoseButton.GetComponent<Button>().onClick.RemoveAllListeners();

        endGame.SetActive(false);
        loseGame.SetActive(false);

        ///save activity status
        if (toogleComponent.isOn)
        {
            Debug.Log("is on");
            PlayerPrefs.SetString("mission" + minigameIndex, "true");
        }
        else
        {
            Debug.Log("is off");
            PlayerPrefs.DeleteKey("mission" + minigameIndex);
        }

        PlayerPrefs.Save();
        
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
            GameSound.gameSound.PlayLoopMusic("main_bgm");
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
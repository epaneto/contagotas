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
    public GameObject skipIntroButton;

	JArray Missions;
    JArray SelectedMissions;

	GameObject endGame;
    GameObject scoreGroup;
    GameObject scoreTxt;
    GameObject continueButton;
    //GameObject giveupButton;
    GameObject titleTxt;
    GameObject hintTxt;
    //GameObject challengeTxt;
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

        scoreGroup = GameObject.Find("score_group");
        titleTxt = GameObject.Find("titulo_txt");
        //challengeTxt = GameObject.Find("desafio_txt");
        hintTxt = GameObject.Find("hint_txt");
        continueButton = GameObject.Find("bt_continuar_score");
        //giveupButton = GameObject.Find("bt_giveup");
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

        /////select missions based on the day player clicked

        int SelectedDay = PlayerPrefs.GetInt("ClickedDay",0);
        Debug.Log("SELECTED DAY " + SelectedDay);
             
        SelectedMissions = new JArray();
        JToken FirstMission;
        int FirstMissionIndex;

        if (SelectedDay == 0)
        {
            //FirstMission = Missions[SelectedDay];
            FirstMissionIndex = SelectedDay;

            SelectedMissions.Add(Missions[0]);
            SelectedMissions.Add(Missions[1]);
            SelectedMissions.Add(Missions[2]);
            SelectedMissions.Add(Missions[3]);
            SelectedMissions.Add(Missions[4]);
        }
        else
        {
            FirstMission = Missions[SelectedDay + 4];
            FirstMissionIndex = SelectedDay + 4;
            SelectedMissions.Add(FirstMission);
        }

        //for (int i = 0; i <= SelectedDay + 4; i++)
        //{
        //    if (i != FirstMissionIndex && SelectedMissions.Count < 10)
        //        SelectedMissions.Add(Missions[i]);
        //}

        /////shuffle selected missions
        //for (int i = 0; i < SelectedMissions.Count; i++)
        //{
        //    JToken temp = SelectedMissions[i];
        //    int randomIndex = Random.Range(i, SelectedMissions.Count);
        //    SelectedMissions[i] = SelectedMissions[randomIndex];
        //    SelectedMissions[randomIndex] = temp;
        //}

        ////add first mission
        //SelectedMissions.AddFirst(FirstMission);


		endGame.SetActive (false);
		loseGame.SetActive (false);
        skipIntroButton.SetActive(false);

		minigameIndex = 0;
                   
        JToken sceneName = SelectedMissions[minigameIndex]["sceneid"];
        SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);
	}

    public void ShowIntro()
    {
        skipIntroButton.SetActive(true);
    }

    public void SkipIntro()
    {
        skipIntroButton.SetActive(false);

        MiniGameDefaultBehavior mdb = GameObject.FindObjectOfType<MiniGameDefaultBehavior>();
        mdb.EndedIntro();
    }

	public void ShowLose()
	{
		///REMOVE ACTIVE MINIGAME FROM SCREEN
        JToken sceneName = SelectedMissions[minigameIndex]["sceneid"];
		SceneManager.UnloadSceneAsync (sceneName.ToString());

		loseGame.SetActive (true);

		SkeletonGraphic graphic = loseGameSadChar.GetComponent<SkeletonGraphic> ();
		graphic.AnimationState.SetAnimation(0,"sad",false);

		continueLoseButton.GetComponent<Button> ().onClick.AddListener (PlayNextMiniGame);
	}

	public void ShowResults(int score)
    {
       
        //animate screen
        scoreGroup.transform.DOScale(new Vector3(0, 0, 1), 0.8f).SetEase(Ease.OutBack).From();
        titleTxt.transform.DOScale(new Vector3(0, 0, 1), 0.9f).SetEase(Ease.OutBack).From();
        //challengeTxt.transform.DOScale(new Vector3(0, 0, 1), 1.0f).SetEase(Ease.OutBack).From();
        hintTxt.transform.DOScale(new Vector3(0, 0, 1), 1.1f).SetEase(Ease.OutBack).From();
        toogleButton.transform.DOScale(new Vector3(0, 0, 1.2f), 1.1f).SetEase(Ease.OutBack).From();
        continueButton.transform.DOScale(new Vector3(0, 0, 1.3f), 1.0f).SetEase(Ease.OutBack).From();
        //giveupButton.transform.DOScale(new Vector3(0, 0, 1.4f), 1.0f).SetEase(Ease.OutBack).From();

        endBackground.transform.DOMoveY(-1400,0.6f).SetEase(Ease.OutQuad).From();

        ///CHECK ACTIVITY STATUS
        //Debug.Log("prefs is for mission" + minigameIndex + " : " + PlayerPrefs.HasKey("mission" + minigameIndex));
        toogleComponent.isOn = false;
        if (PlayerPrefs.HasKey("mission" + minigameIndex))
        {
            toogleComponent.isOn = true;
        }
        
            
		///REMOVE ACTIVE MINIGAME FROM SCREEN
        JToken sceneName = SelectedMissions[minigameIndex]["sceneid"];
        SceneManager.UnloadSceneAsync(sceneName.ToString());

        JToken titleString = SelectedMissions[minigameIndex]["title"];
        Text titleField = titleTxt.GetComponent<Text> ();
        titleField.text = titleString.ToString ();

        JToken hintString = SelectedMissions[minigameIndex]["hint"];
        Text hintField = hintTxt.GetComponent<Text>();
        hintField.text = hintString.ToString();

        //Text challengeField = challengeTxt.GetComponent<Text>();
        //challengeField.text = "Desafio Real #" + (minigameIndex + 1).ToString();

		endGame.SetActive(true);

        playerScore = 0;
        StartCoroutine("CountTo", score);

        //giveupButton.GetComponent<Button>().onClick.AddListener(GoToMap);
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

		

			StartCoroutine(AddUserScore(score));

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

	IEnumerator AddUserScore(int score)
	{
		WWW result;
		yield return result = WWWUtils.DoWebRequestWithSpecificURL("http://www.contagotas.online/services/user/score/" + PlayerPrefs.GetInt ("user_id").ToString() + "/" + score.ToString() + "/");
		Debug.Log("url result = " + result.text);

		if (result.text == "success") {
			///save player points
			UserData.userData.playerData.playerPoints += score;
			UserData.userData.Save ();
		}
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

        //giveupButton.GetComponent<Button>().onClick.RemoveAllListeners();
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
        
        if (minigameIndex + 1 < SelectedMissions.Count)
        {
            //Debug.Log ("play next minigame " + minigameIndex);

            minigameIndex++;


            JToken sceneName = SelectedMissions[minigameIndex]["sceneid"];
            SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);

        }
        else
        {
            //Debug.Log ("that was the last minigame, show map");
            GoToMap();
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

	public void blinkSpecialTimeBar()
	{
		Image timeBarFillMaterial = timeBarFill.GetComponent<Image>();

		Sequence mySequence = DOTween.Sequence();
		mySequence.Append(timeBarFillMaterial.DOColor(Color.green, 0.2f));
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

    public void GoToMap()
    {
        PlayerPrefs.SetString("after_minigames", "true");

        //giveupButton.GetComponent<Button>().onClick.RemoveAllListeners();
        continueButton.GetComponent<Button>().onClick.RemoveAllListeners();
        continueLoseButton.GetComponent<Button>().onClick.RemoveAllListeners();

        hideTime();
        endGame.SetActive(false);
        loseGame.SetActive(false);

        GameSound.gameSound.PlayLoopMusic("main_bgm");
        SceneController.sceneController.FadeAndLoadScene("Map", true);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;


public class MapController : MonoBehaviour {

    public GameObject InfoObject;
    public GameObject hintContainer;
    public GameObject SettingsObject;
    public GameObject VolumeButtonOn;
    public GameObject VolumeButtonOff;
    public GameObject TutorialObject;
    public List<GameObject> TutorialSteps;

    JArray Missions;

    private void Start()
    {
        if (PlayerPrefs.GetString("sound") == "off"){
            VolumeButtonOn.SetActive(false);
            VolumeButtonOff.SetActive(true);
        }

        if(!PlayerPrefs.HasKey("tutorialDone"))
        {
            StartTutorial();
        }

        if(PlayerPrefs.HasKey("show_next_day_tutorial") && PlayerPrefs.HasKey("after_minigames"))
        {
            PlayerPrefs.DeleteKey("show_next_day_tutorial");
            ShowNextDayTutorial();
        }

        PlayerPrefs.DeleteKey("after_minigames");

        InfoObject.SetActive(false);
    }

    public void UpdateMapBasedInPlayerProgress(int day, int maxDays, bool isNextDay)
	{
		Debug.Log("Hello player! Today its your " + day + "th day. let's update the map.");
        day = 17;
		for (int i = 1; i <= maxDays; i++) {
			if (i < day) {
				GameObject.Find ("bt_level_" + i).GetComponent<MapButton> ().SetOldSPrite ();
			} else if (i == day) {
				GameObject.Find ("bt_level_" + i).GetComponent<MapButton> ().SetActiveSprite ();
            }else{
                GameObject.Find("bt_level_" + i).GetComponent<Button>().interactable = false;
            }
		}

        if(isNextDay)
            PlayerPrefs.SetString("show_next_day_tutorial", "true");
        
        loadHints();
	}

    void loadHints()
    {
        Debug.Log("LOAD HINTS");
        ///load mission json
        TextAsset t = (TextAsset)Resources.Load("missoes", typeof(TextAsset));
        string DataAsJSON = t.text;

        JObject o = JObject.Parse(DataAsJSON);
        Missions = (JArray)o["missoes"];

        for (int i = 0; i < Missions.Count; i++)
        {
            GameObject hint = Instantiate(Resources.Load("hint", typeof(GameObject)), new Vector3(0,0,0), hintContainer.transform.rotation) as GameObject;
            hint.transform.SetParent(hintContainer.transform, false);

            JToken titleString = Missions[i]["title"];
            JToken hintString = Missions[i]["hint"];


            Text titleField = hint.transform.GetChild(1).GetComponent<Text>();
            titleField.text = "Desafio #" + (i+1);


            Text hintField = hint.transform.GetChild(0).GetComponent<Text>();
            hintField.text = hintString.ToString();
        }

    }

    public void openHints()
    {
        GameSound.gameSound.PlaySFX("button");
        InfoObject.SetActive(true);
        InfoObject.transform.DOMoveY(-1400, 0.6f).SetEase(Ease.OutQuad).From();
    }

    public void hideHints()
    {
        GameSound.gameSound.PlaySFX("button");
        InfoObject.SetActive(false);
    }

    public void openSettings()
    {
        GameSound.gameSound.PlaySFX("button");
        SettingsObject.SetActive(true);
    }

    public void hideSettings()
    {
        GameSound.gameSound.PlaySFX("button");
        SettingsObject.SetActive(false);
    }

    public void MuteUnmuteSound()
    {
        GameSound.gameSound.MuteUnmuteSound();

        if(GameSound.gameSound.MusicON)
        {
            VolumeButtonOn.SetActive(true);   
            VolumeButtonOff.SetActive(false);
        }else{
            VolumeButtonOn.SetActive(false);
            VolumeButtonOff.SetActive(true);
        }
    }

    public void OpenGroups()
    {
        GameSound.gameSound.PlaySFX("button");
        SceneController.sceneController.FadeAndLoadScene("Group", true);
    }

	public void OpenPlayerRanking()
	{
		GameSound.gameSound.PlaySFX("button");
		SceneController.sceneController.FadeAndLoadScene("PlayerRanking", true);
	}

    public void OpenAvatar()
    {
        GameSound.gameSound.PlaySFX("button");
        SceneController.sceneController.FadeAndLoadScene("Avatar", true);
    }




    /////TUTORIAL METHODS

    int tutorialIndex = 0;
    GameObject step;
    void StartTutorial()
    {
        TutorialObject.SetActive(true);
        step = TutorialSteps[tutorialIndex];
        step.SetActive(true);
    }

    public void AdvanceTutorial()
    {
        if(tutorialIndex +1 < TutorialSteps.Count-1)
        {
            step.SetActive(false);
            tutorialIndex++;

            step = TutorialSteps[tutorialIndex];
            step.SetActive(true);

        }else{
            HideTutorial();
        }
    }

    void HideTutorial(){
        PlayerPrefs.SetString("tutorialDone","done");
        PlayerPrefs.Save();

        TutorialObject.SetActive(false);
    }

    void ShowNextDayTutorial()
    {
        tutorialIndex = TutorialSteps.Count;

        TutorialObject.SetActive(true); 
        step = TutorialSteps[TutorialSteps.Count-1];
        step.SetActive(true);
    }
}
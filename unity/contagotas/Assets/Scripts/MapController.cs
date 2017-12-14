using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;


public class MapController : MonoBehaviour {

    public GameObject InfoObject;
    public GameObject hintContainer;

    JArray Missions;

    private void Start()
    {
        InfoObject.SetActive(false);
    }

    public void UpdateMapBasedInPlayerProgress(int day, int maxDays)
	{
		Debug.Log("Hello player! Today its your " + day + "th day. let's update the map.");


		for (int i = 1; i <= maxDays; i++) {
			if (i < day) {
				GameObject.Find ("bt_level_" + i).GetComponent<MapButton> ().SetOldSPrite ();
			} else if (i == day) {
				GameObject.Find ("bt_level_" + i).GetComponent<MapButton> ().SetActiveSprite ();
			}
		}

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
        InfoObject.SetActive(true);
        InfoObject.transform.DOMoveY(-1400, 0.6f).SetEase(Ease.OutQuad).From();
    }

    public void hideHints()
    {
        InfoObject.SetActive(false);
    }

    public void OpenGroups()
    {
        SceneController.sceneController.FadeAndLoadScene("Group", true);
    }
}
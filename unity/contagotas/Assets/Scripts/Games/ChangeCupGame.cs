using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ChangeCupGame : MonoBehaviour {

    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    public List<GameObject> CupList;
    public int numBadCups = 3;
    int gamePages = 2;
    int activePage = 0;
    public GameObject gameBackground;
    public GameObject mainGroup;
    float groupInitX;
    float groupInitY;
    public List<Color> Colors;


	// Use this for initialization
	void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();

        groupInitX = mainGroup.transform.position.x;
        groupInitY = mainGroup.transform.position.y;

        for (int i = 0; i < CupList.Count; i++)
        {
            GameObject temp = CupList[i];
            int randomIndex = Random.Range(i, CupList.Count);
            CupList[i] = CupList[randomIndex];
            CupList[randomIndex] = temp;
        }

        for (int a = 0; a < CupList.Count; a++)
        {
            ChangeableItem temp = CupList[a].GetComponent<ChangeableItem>();
            temp.SendMDB(mdb);
            if (a < numBadCups) { 
                temp.setBadState();
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!mdb.gameStarted)
            return;

        if (!isPlaying)
            return;

        bool hasBadCup = false;

        for (int i = 0; i < CupList.Count; i++)
        {
            ChangeableItem temp = CupList[i].GetComponent<ChangeableItem>();
            if (!temp.isGood)
                hasBadCup = true;
        }

        if(!hasBadCup)
        {
            isPlaying = false;

            if(activePage < gamePages)
            {
                activePage++;
                LoadNextPage();
            }else{
                EndGame();
            }

            return;
        }

        if (!mdb.hasTimeLeft() && hasBadCup){
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }
	}

    void LoadNextPage()
    {
        mainGroup.transform.DOMoveX(-1500, 0.6f).SetEase(Ease.InQuad).OnComplete(SetupPage);
    }

    void SetupPage()
    {
        for (int i = 0; i < CupList.Count; i++)
        {
            GameObject temp = CupList[i];
            int randomIndex = Random.Range(i, CupList.Count);
            CupList[i] = CupList[randomIndex];
            CupList[randomIndex] = temp;
        }

        for (int a = 0; a < CupList.Count; a++)
        {
            ChangeableItem temp = CupList[a].GetComponent<ChangeableItem>();
            temp.ResetItem();
            if (a < numBadCups)
                temp.setBadState();
        }

        ShowNextPage();
    }

    void ShowNextPage()
    {
        Image background = gameBackground.GetComponent<Image>();
        background.DOColor(Colors[activePage], 0.6f);

        mainGroup.transform.position = new Vector3(1500, groupInitY, 0);

        mainGroup.transform.DOMoveX(groupInitX, 0.6f).SetEase(Ease.OutQuad);

        isPlaying = true;
    }

    void EndGame()
    {
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

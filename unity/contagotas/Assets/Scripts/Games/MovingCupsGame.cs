using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MovingCupsGame : MonoBehaviour {

    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    public List<GameObject> CupList;
    public int numBadCups = 0;
    public int neededCups = 0;


	// Use this for initialization
	void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();

        EventManager.StartListening("MiniGameStarted", OnMiniGameStarted);


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

    void OnMiniGameStarted()
    {
        EventManager.StopListening("MiniGameStarted", OnMiniGameStarted);
        MoveCups mc = GameObject.FindObjectOfType<MoveCups>();
        mc.StartMoving();
    }

    // Update is called once per frame
    void Update () {
        if (!mdb.gameStarted)
            return;

        if (!isPlaying)
            return;

        int badcups = 0;

        for (int i = 0; i < CupList.Count; i++)
        {
            ChangeableItem temp = CupList[i].GetComponent<ChangeableItem>();
            if (!temp.isGood)
            {
                badcups++;
            }
        }

        if(numBadCups - badcups >= neededCups)
        {
            isPlaying = false;
            EndGame();
            return;
        }

        if (!mdb.hasTimeLeft()){
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }
	}


    public void EndGameLose()
    {
        isPlaying = false;
        mdb.EndedGameLose();
    }

    void EndGame()
    {
        isPlaying = false;
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCupGame : MonoBehaviour {

    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    public List<GameObject> CupList;
    public int numBadCups = 3;

	// Use this for initialization
	void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();

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
            EndGame();
            return;
        }

        if (!mdb.hasTimeLeft() && hasBadCup){
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }
	}

    void EndGame()
    {
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

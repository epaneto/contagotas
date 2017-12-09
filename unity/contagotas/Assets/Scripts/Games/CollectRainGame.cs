using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRainGame : MonoBehaviour {
    
    float dropInterval = 0.5f;
    public GameObject dropParent;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;

    int neededDrops = 12;
    int collectedDrops = 0;

	// Use this for initialization
	void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
        InvokeRepeating("launchDrop", 4.5f, dropInterval);
	}
	
	// Update is called once per frame
	void Update () {
        if(!mdb.gameStarted)

            return;

        if (!isPlaying)
            return;
        
        if (!mdb.hasTimeLeft() && collectedDrops < neededDrops)
        {
            CancelInvoke();
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }
	}

    void launchDrop()
    {
        GameObject drop = Instantiate(Resources.Load("drop", typeof(GameObject)), new Vector3(Random.Range(-400,400),-220,0), dropParent.transform.rotation) as GameObject;
        drop.transform.SetParent(dropParent.transform, false);
    }

    public void missedDrop()
    {
        mdb.loseTime(1.0f);
        //mdb.EndedGameLose();
    }

    public void CollectedDrop()
    {
        collectedDrops++;

        if(collectedDrops >= neededDrops)
        {
            EndGame();
        }

    }

    void EndGame()
    {
        CancelInvoke();
        isPlaying = false;
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHolesGame : MonoBehaviour {


    float dropInterval = 0.8f;
    public GameObject dropParent;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    int maxDrops = 8;
    int neededDrops = 5;
    int collectedDrops = 0;
    int createdDrops = 0;

	// Use this for initialization
	void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();

        EventManager.StartListening("MiniGameStarted", StartHoles);

	}

    void StartHoles()
    {
        EventManager.StopListening("MiniGameStarted", StartHoles);
        InvokeRepeating("launchDrop", 4.0f, dropInterval);
    }
	
    // Update is called once per frame
    void Update()
    {
        if (!mdb.gameStarted)

            return;

        if (!isPlaying)
            return;

        if (!mdb.hasTimeLeft())
        {
            CancelInvoke();
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }
    }

    void launchDrop()
    {
        if (createdDrops >= maxDrops)
            return;
        
        createdDrops++;

        GameObject drop = Instantiate(Resources.Load("hole", typeof(GameObject)), new Vector3(Random.Range(-110, 110), -135, 0), dropParent.transform.rotation) as GameObject;
        drop.transform.SetParent(dropParent.transform, false);
    }

    public void fixedPipe()
    {
        collectedDrops++;

        if (collectedDrops >= neededDrops)
        {
            EndGame();
        }

    }

    public void missedHole()
    {
        mdb.loseTime(1.0f);
        //mdb.EndedGameLose();
    }

    void EndGame()
    {
        CancelInvoke();
        isPlaying = false;
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }

}

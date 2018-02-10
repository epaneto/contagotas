using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHolesGame : MonoBehaviour {

	public List<GameObject> holes;

    public GameObject pipe;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    
	int fixesNeed = 14;
    int collectedDrops = 0;
	float speed;

	// Use this for initialization
	void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
		speed = -1;
        EventManager.StartListening("MiniGameStarted", StartHoles);

	}

    void StartHoles()
    {
        EventManager.StopListening("MiniGameStarted", StartHoles);
    }

	void UpdatePipe ()
	{
		pipe.transform.Translate (0, speed * Time.deltaTime, 0);
		speed -= (Time.deltaTime/2);
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

		UpdatePipe ();
    }

    public void fixedPipe()
    {
        GameSound.gameSound.PlaySFX("tap");
        collectedDrops++;
		mdb.winTime (1.0f);
        if (collectedDrops >= fixesNeed)
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

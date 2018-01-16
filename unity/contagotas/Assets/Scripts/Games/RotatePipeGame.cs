using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePipeGame : MonoBehaviour {

    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    public List<GameObject> pipes;

	// Use this for initialization
	void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!mdb.gameStarted)
            return;

        if (!isPlaying)
            return;

        if (!mdb.hasTimeLeft())
        {
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }

        bool pipesReady = true;
        for (int i = 0; i < pipes.Count; i++){
            TouchRotate pipe = pipes[i].GetComponent<TouchRotate>();
            if (!pipe.isRight)
                pipesReady = false;
        }

        if (pipesReady)
        {
            isPlaying = false;
            EndGame();
        }

	}

    void EndGame()
    {
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

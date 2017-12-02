using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapHoleGame : MonoBehaviour {
    public List<GameObject> holes;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    int numHoles = 0;

    // Use this for initialization
    void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
        numHoles = holes.Count;
    }
	
	// Update is called once per frame
	void Update () {
        if (!mdb.gameStarted)
            return;

        if (!isPlaying)
            return;

        if (!mdb.hasTimeLeft() && numHoles > 0)
        {
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }
    }

    public void updateHoles()
    {
        numHoles--;
        Debug.Log("update holes " + numHoles);

        if (numHoles == 0)
            EndGame();
    }

    void EndGame()
    {
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

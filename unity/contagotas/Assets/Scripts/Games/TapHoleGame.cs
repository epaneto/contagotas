using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapHoleGame : MonoBehaviour {
    public List<GameObject> holes;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
	int holesTapped = 1;
    int numHolesToTap = 0;

    // Use this for initialization
    void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
        numHolesToTap = 15;
		int holeIndex = Random.Range (0, holes.Count);
		holes [holeIndex].SetActive (true);
    }
	
	// Update is called once per frame
	void Update () {
        if (!mdb.gameStarted)
            return;

        if (!isPlaying)
            return;

        if (!mdb.hasTimeLeft() && numHolesToTap > 0)
        {
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }
    }

    public void updateHoles()
    {
        numHolesToTap--;
		holesTapped++;
		if(holesTapped <= 5)
			RandomizeHoles (holesTapped);
        
		Debug.Log("update holes " + numHolesToTap);

        GameSound.gameSound.PlaySFX("tap");

        if (numHolesToTap == 0)
            EndGame();
    }

	private void RandomizeHoles(int numberOfHoles)
	{
		while(numberOfHoles != 0)
		{
			int holeIndex = Random.Range (0, holes.Count);

			if(!holes[holeIndex].activeSelf)
			{
				holes [holeIndex].SetActive (true);
				numberOfHoles--;
			}
		}
	}

    void EndGame()
    {
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

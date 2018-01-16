using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCups : MonoBehaviour {

    public List<GameObject> CupGroups;
    MovingCupsGame mcg;

    // Use this for initialization
    void Start()
    {
        mcg = GameObject.FindObjectOfType<MovingCupsGame>();
    }

    public void StartMoving()
    {
        for (int i = 0; i < CupGroups.Count; i++)
        {
            GameObject group = CupGroups[i];

            float point = (Screen.width * 3.8f);
            if (i == CupGroups.Count - 1)
            {
                group.transform.DOMoveX(point, 18.0f - i * 2.0f).SetEase(Ease.Linear).OnComplete(EndGame);
            }
            else
            {
                group.transform.DOMoveX(point, 18.0f - i * 2.0f).SetEase(Ease.Linear);
            }
        }
    }

    public void EndGame()
    {
        Debug.Log("endgame");
        mcg.EndGameLose();
    }
	
	// Update is called once per frame
	void Update () {
        
	}
}

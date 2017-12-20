﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBoxGame : MonoBehaviour {
    
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    public GameObject cover;

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

        Swipe();
	}

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    public void Swipe()
    {


        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();

            if(currentSwipe.y > 0  && currentSwipe.x > -0.5f  && currentSwipe.x < 0.5f)
            {
                Debug.Log("swipe up");
                GameSound.gameSound.PlaySFX("swipe");
                Vector3 activePosition = new Vector3(0, 30.0f, 0);

                StartCoroutine(smooth_move(activePosition, 10.0f));
                //cover.transform.Translate(Vector3.up * 30.0f);
            }

        }

        Debug.Log(cover.transform.localPosition.y);
        if (cover.transform.localPosition.y >= -20)
        {
            isPlaying = false;
            EndGame();
        }
    }

    IEnumerator smooth_move(Vector3 direction, float speed)
    {
        float startime = Time.time;
        Vector3 start_pos = cover.transform.position; //Starting position.
        Vector3 end_pos = cover.transform.position + direction; //Ending position.
        //Debug.Log(start_pos + " " + end_pos);
        while (start_pos != end_pos && ((Time.time - startime) * speed) < 1f)
        {
            float move = Mathf.Lerp(0, 1, (Time.time - startime) * speed);

            cover.transform.position += direction * move;

            yield return null;
        }
    }

    void EndGame()
    {
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

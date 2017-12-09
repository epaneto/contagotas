﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class MiniGameDefaultBehavior : MonoBehaviour {

	public GameObject gameMechanic;
	public GameObject gameAnimation;
    public GameObject textIntro;

	SkeletonGraphic graphic;
	MinigamesController controller;
    public bool isTimeGame = true;
	public int gameScore;
	public bool gameStarted = false;

	public float maxTime = 10.0f;
	public float totalTime = 0.0f;
	public float maxScore = 100.0f;



	// Use this for initialization
	void Start () {
		controller = GameObject.FindObjectOfType<MinigamesController> ();

		graphic = gameAnimation.GetComponent<SkeletonGraphic> ();

		gameMechanic.SetActive (false);

		//Debug.Log ("play the intro animation of " + "enter_game" + (controller.minigameIndex + 1));
		gameAnimation.SetActive (true);
		graphic.AnimationState.SetAnimation(0,"enter_game",false);
		graphic.AnimationState.Complete += EndedIntro;

//		yield return new WaitForSeconds(6.0f);
//
//		EndedGame ();

	}

    public void loseTime(float timeLost)
    {
        totalTime += timeLost;
        controller.updateTime(1.0f - totalTime / maxTime);
        controller.blinkTimeBar();
    }

	void EndedIntro(Spine.TrackEntry entry)
	{
		gameStarted = true;

		graphic.AnimationState.Complete -= EndedIntro;

		Debug.Log("ended intro animation of " + "enter_game" + (controller.minigameIndex + 1));

        controller.showTime();
        textIntro.SetActive(false);
		gameAnimation.SetActive (false);
		gameMechanic.SetActive (true);
	}

	public void EndedGameWin(float score)
	{
		gameStarted = false;

		gameScore = (int)score;
		Debug.Log ("WIN GAME WITH SCORE: " + gameScore);

		gameAnimation.SetActive (true);
		gameMechanic.SetActive (false);

        //controller.hideTime();

		graphic.AnimationState.SetAnimation(0,"exit_game",false);
		graphic.AnimationState.Complete += PlayNextGame;
	}

	public void EndedGameLose()
	{
		gameStarted = false;

		Debug.Log ("LOST GAME!");
       
        controller.hideTime();

		gameMechanic.SetActive (false);
		controller.ShowLose ();
	}

	void PlayNextGame(Spine.TrackEntry entry)
	{
		graphic.AnimationState.Complete -= PlayNextGame;
		controller.ShowResults (gameScore);
	}

	// Update is called once per frame
	void Update () {
        if (!gameStarted || !isTimeGame)
			return;
		
        controller.updateTime(1.0f - totalTime / maxTime);
		totalTime += Time.deltaTime;

		//Debug.Log (totalTime + " " + maxTime);
	}

	public bool hasTimeLeft()
	{
		return totalTime < maxTime;
	}

    public float getTimeProgress()
    {
        return totalTime / maxTime;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class MiniGameDefaultBehavior : MonoBehaviour {

	GameObject gameMechanic;
	GameObject gameAnimation;
	SkeletonGraphic graphic;
	MinigamesController controller;
	public int gameScore;
	public bool gameStarted = false;

	public float maxTime = 10.0f;
	public float totalTime = 0.0f;

	// Use this for initialization
	void Start () {
		controller = GameObject.FindObjectOfType<MinigamesController> ();

		gameAnimation = GameObject.Find ("game_animation");
		graphic = gameAnimation.GetComponent<SkeletonGraphic> ();

		gameMechanic = GameObject.Find ("game_mechanic");
		gameMechanic.SetActive (false);

		//Debug.Log ("play the intro animation of " + "enter_game" + (controller.minigameIndex + 1));

		graphic.AnimationState.SetAnimation(0,"enter_game",false);
		graphic.AnimationState.Complete += EndedIntro;

//		yield return new WaitForSeconds(6.0f);
//
//		EndedGame ();

	}

	void EndedIntro(Spine.TrackEntry entry)
	{
		gameStarted = true;

		graphic.AnimationState.Complete -= EndedIntro;

		Debug.Log("ended intro animation of " + "enter_game" + (controller.minigameIndex + 1));

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

		graphic.AnimationState.SetAnimation(0,"exit_game",false);
		graphic.AnimationState.Complete += PlayNextGame;
	}

	public void EndedGameLose()
	{
		gameStarted = false;

		Debug.Log ("LOST GAME!");
		gameAnimation.SetActive (true);
		gameMechanic.SetActive (false);

		graphic.AnimationState.SetAnimation(0,"exit_game",false);
		graphic.AnimationState.Complete += PlayNextGame;
	}

	void PlayNextGame(Spine.TrackEntry entry)
	{
		graphic.AnimationState.Complete -= PlayNextGame;
		controller.ShowResults (gameScore);
	}

	// Update is called once per frame
	void Update () {
		if (!gameStarted)
			return;
		
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

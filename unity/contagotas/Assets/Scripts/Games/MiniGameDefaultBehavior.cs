using System.Collections;
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
        GameSound.gameSound.PlayOneShotMusic("game_intro",1.0f);

		controller = GameObject.FindObjectOfType<MinigamesController> ();
        //controller.ShowIntro();

		graphic = gameAnimation.GetComponent<SkeletonGraphic> ();

		gameMechanic.SetActive (false);

		//Debug.Log ("play the intro animation of " + "enter_game" + (controller.minigameIndex + 1));
		gameAnimation.SetActive (true);
		graphic.AnimationState.SetAnimation(0,"enter_game",false);
		graphic.AnimationState.Complete += EndedIntro;


	}

    public void loseTime(float timeLost)
    {
        totalTime += timeLost;
        controller.updateTime(1.0f - totalTime / maxTime);
        controller.blinkTimeBar();
    }

	public void EndedIntro(Spine.TrackEntry entry)
    //public void EndedIntro()
	{
		gameStarted = true;
        GameSound.gameSound.PlayLoopMusic("game_music",0.2f);
		graphic.AnimationState.Complete -= EndedIntro;

		Debug.Log("ended intro animation of " + "enter_game" + (controller.minigameIndex + 1));

        controller.showTime();
        textIntro.SetActive(false);
		gameAnimation.SetActive (false);
		gameMechanic.SetActive (true);
	}

    public void EndedGameWin(float score)
	{
        StartCoroutine(ExitSounds());

		gameStarted = false;

		gameScore = (int)score;
		Debug.Log ("WIN GAME WITH SCORE: " + gameScore);
        //controller.hideTime();
        StartCoroutine(ShowExitAnimation());
	}

    IEnumerator ShowExitAnimation()
    {
        graphic.AnimationState.SetAnimation(0, "exit_game", false);

        ///This delay is to prevent the user to see the spine transition
        yield return new WaitForSeconds(0.2f);

        gameAnimation.SetActive(true);
        gameMechanic.SetActive(false);

        graphic.AnimationState.Complete += PlayNextGame;
    }

    IEnumerator ExitSounds()
    {
        GameSound.gameSound.StopMusic(0.3f);
        yield return new WaitForSeconds(0.4f);
        GameSound.gameSound.PlayOneShotMusic("game_victory", 1.0f);
    }

    public void EndedGameLose()
	{
        GameSound.gameSound.StopMusic(0.5f);

        GameSound.gameSound.PlayOneShotMusic("game_defeat",1.0f);

		gameStarted = false;

		Debug.Log ("LOST GAME!");
       
        controller.hideTime();

		gameMechanic.SetActive (false);
		controller.ShowLose ();
	}

	void PlayNextGame(Spine.TrackEntry entry)
	{
        GameSound.gameSound.PlaySFX("score_count");

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

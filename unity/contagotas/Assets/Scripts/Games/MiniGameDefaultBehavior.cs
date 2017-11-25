using System.Collections;
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

	// Use this for initialization
	IEnumerator Start () {
		controller = GameObject.FindObjectOfType<MinigamesController> ();

		gameAnimation = GameObject.Find ("game_animation");
		graphic = gameAnimation.GetComponent<SkeletonGraphic> ();

		gameMechanic = GameObject.Find ("game_mechanic");
		gameMechanic.SetActive (false);

		Debug.Log ("play the intro animation of " + "enter_game" + (controller.minigameIndex + 1));

		graphic.AnimationState.SetAnimation(0,"enter_game" + (controller.minigameIndex + 1),false);
		graphic.AnimationState.Complete += EndedIntro;

		yield return new WaitForSeconds(2.0f);

		EndedGame ();

	}

	void EndedIntro(Spine.TrackEntry entry)
	{
		graphic.AnimationState.Complete -= EndedIntro;

		Debug.Log("ended intro animation of " + "enter_game" + (controller.minigameIndex + 1));

		gameAnimation.SetActive (false);
		gameMechanic.SetActive (true);
	}

	public void EndedGame()
	{
		gameAnimation.SetActive (true);
		gameMechanic.SetActive (false);

		graphic.AnimationState.SetAnimation(0,"exit_game" + (controller.minigameIndex + 1),false);
		graphic.AnimationState.Complete += PlayNextGame;
	}

	void PlayNextGame(Spine.TrackEntry entry)
	{
		graphic.AnimationState.Complete -= PlayNextGame;
		controller.PlayNextMinigame ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}

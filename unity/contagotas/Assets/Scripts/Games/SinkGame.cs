using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class SinkGame : MonoBehaviour {

	SkeletonGraphic faucet;
	SkeletonGraphic handler;

	public GameObject faucetObject;
	public GameObject handlerObject;

	bool isPlaying = true;
	int maxTurns = 10;
	int numTurns = 0;
	MiniGameDefaultBehavior mdb;

	// Use this for initialization
	void Start () {
		mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior> ();

		faucet = faucetObject.GetComponent<SkeletonGraphic> ();
		handler = handlerObject.GetComponent<SkeletonGraphic> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!mdb.gameStarted)
			return;
		
		if (!isPlaying)
			return;

		if (!mdb.hasTimeLeft() && numTurns < maxTurns) {
			isPlaying = false;
			mdb.EndedGameLose ();
			return;
		}

		Swipe ();
	}

	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;

	public void Swipe()
	{
		
		
		if(Input.GetMouseButtonDown(0))
		{
			//save began touch 2d point
			firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
		}
		if(Input.GetMouseButtonUp(0))
		{
			//save ended touch 2d point
			secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);

			//create vector from the two points
			currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

			//normalize the 2d vector
			currentSwipe.Normalize();

			//swipe upwards
//			if(currentSwipe.y > 0  && currentSwipe.x > -0.5f  && currentSwipe.x < 0.5f)
//			{
//				Debug.Log("up swipe");
//			}
//			//swipe down
//			if(currentSwipe.y < 0  && currentSwipe.x > -0.5f  && currentSwipe.x < 0.5f)
//			{
//				Debug.Log("down swipe");
//			}
			//swipe left
//			if(currentSwipe.x < 0  && currentSwipe.y > -0.8f  && currentSwipe.y < 0.8f)
//			{
//				Debug.Log("left swipe");
//				
//			}
			//swipe right
			if(currentSwipe.x > 0  && currentSwipe.y > -0.8f  && currentSwipe.y < 0.8f)
			{
				//Debug.Log("right swipe");
				handler.AnimationState.SetAnimation(0,"loop",false);
				numTurns++;
				if (numTurns >= maxTurns) {
					isPlaying = false;
					faucet.AnimationState.SetAnimation(0,"faucet_off",false);
					faucet.AnimationState.Complete += EndGame;
				}
			}
		}
			
	}

	void EndGame(Spine.TrackEntry entry)
	{
		faucet.AnimationState.Complete -= EndGame;

		mdb.EndedGameWin (mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using UnityEngine.UI;

public class BrushTeethGame : MonoBehaviour {

	SkeletonGraphic faucet;
	SkeletonGraphic handler;

	public GameObject faucetObject;
	public GameObject handlerObject;
    public GameObject followObject;
    public GameObject sinkStep;
    public GameObject mouthStep;
    bool isMouthStep = false;
	bool isPlaying = true;
	int maxTurns = 10;
	int numTurns = 0;
	MiniGameDefaultBehavior mdb;

	// Use this for initialization
	void Start () {
		mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior> ();
        mouthStep.SetActive(false);
		faucet = faucetObject.GetComponent<SkeletonGraphic> ();
		handler = handlerObject.GetComponent<SkeletonGraphic> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!mdb.gameStarted)
			return;
		
		if (!isPlaying)
			return;

		if (!mdb.hasTimeLeft()) {
			isPlaying = false;
			mdb.EndedGameLose ();
			return;
		}

        if (isMouthStep && followObject)
        {
            followObject.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

        }

		Swipe ();
	}

	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;

	public void Swipe()
	{

        if (isMouthStep)
            return;
		
		
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

			if(currentSwipe.x > 0  && currentSwipe.y > -0.8f  && currentSwipe.y < 0.8f)
			{
				//Debug.Log("right swipe");
                GameSound.gameSound.PlaySFX("swipe");
				handler.AnimationState.SetAnimation(0,"loop",false);
				numTurns++;
				if (numTurns >= maxTurns) {
					isPlaying = false;
					faucet.AnimationState.SetAnimation(0,"faucet_off",false);
                    faucet.AnimationState.Complete += ShowMouthStep;
				}
			}
		}
			
	}

    void ShowMouthStep(Spine.TrackEntry entry)
    {
        isPlaying = true;
        isMouthStep = true;
        faucet.AnimationState.Complete -= ShowMouthStep;
        mouthStep.SetActive(true);
        sinkStep.SetActive(false);
    }

    public void cleanTeeth(GameObject teeth)
    {
        Image teethImage = teeth.GetComponent<Image>();
        Color c = teethImage.color;
        c.a -= 0.05f;
        teethImage.color = c;

        if(c.a <= 0)
        {
            isPlaying = false;
            EndGame();
        }
    }

	void EndGame()
	{
		mdb.EndedGameWin (mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
	}
}

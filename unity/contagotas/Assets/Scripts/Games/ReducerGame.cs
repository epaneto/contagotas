using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using DG.Tweening;
using Spine.Unity.Modules.AttachmentTools;

public class ReducerGame : MonoBehaviour {
    public GameObject reducerObject;
    public GameObject faucetObject;
    public GameObject sinkGroup;

    float initialRedutorPos;
    float tapSizeY;

    float sinkX;
    float sinkY;

    int numSinks = 0;
    int totalSinks = 3;
    string sinkName = "faulcet";

    SkeletonGraphic faucetSkeleton;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;

	// Use this for initialization
	void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
        faucetSkeleton = faucetObject.GetComponent<SkeletonGraphic>();

        string activeSink = sinkName + (numSinks + 1);
        faucetSkeleton.Skeleton.SetSkin(activeSink);

        initialRedutorPos = reducerObject.transform.position.y;
        tapSizeY = (faucetObject.transform.position.y - initialRedutorPos) / 15;

        sinkX = sinkGroup.transform.position.x;
        sinkY = sinkGroup.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        //if (reducerObject.transform.position.y > initialRedutorPos)
        //{
        //    reducerObject.transform.position += new Vector3(0, -(tapSizeY / 30), 0);
        //}

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
	}

    public void raiseRedutor()
    {
        if (!isPlaying)
            return;
        
        //Debug.Log("redutor " + reducerObject.transform.position.y + " faucet " + faucetObject.transform.position.y);
        GameSound.gameSound.PlaySFX("tap");
        reducerObject.transform.position += new Vector3(0, tapSizeY, 0);
        if (reducerObject.transform.position.y >= (faucetObject.transform.position.y + 20))
        {
            numSinks++;
            if(numSinks == totalSinks)
            {
                Debug.Log("success!");
                isPlaying = false;
                faucetSkeleton.AnimationState.Complete += EndGame;
                faucetSkeleton.AnimationState.SetAnimation(0, "faucet_off", false);
            }else{
                isPlaying = false;
                HideSink();
            }

        }
    }

    void HideSink()
    {
        faucetSkeleton.AnimationState.SetAnimation(0, "faucet_off", false);
        sinkGroup.transform.DOMoveX(-1500, 0.5f).SetEase(Ease.InQuad).OnComplete(ShowNextSink);
    }

    void ShowNextSink()
    {
        string activeSink = sinkName + (numSinks+1);
        faucetSkeleton.Skeleton.SetSkin(activeSink);

        faucetSkeleton.AnimationState.SetAnimation(0, "faucet_on", true);
        reducerObject.transform.position = new Vector3(reducerObject.transform.position.x, initialRedutorPos, 0);

        sinkGroup.transform.position = new Vector3(1500, sinkY, 0);
        sinkGroup.transform.DOMoveX(sinkX, 0.5f).SetEase(Ease.OutQuad);

        isPlaying = true;
    }

    void EndGame(Spine.TrackEntry entry)
    {
        faucetSkeleton.AnimationState.Complete -= EndGame;
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }

}
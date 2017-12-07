using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class ReducerGame : MonoBehaviour {
    public GameObject reducerObject;
    public GameObject faucetObject;

    float initialRedutorPos;
    float tapSizeY;

    SkeletonGraphic faucetSkeleton;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;

	// Use this for initialization
	void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
        faucetSkeleton = faucetObject.GetComponent<SkeletonGraphic>();
        initialRedutorPos = reducerObject.transform.position.y;
        tapSizeY = (faucetObject.transform.position.y - initialRedutorPos) / 15;
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
        
        Debug.Log("redutor " + reducerObject.transform.position.y + " faucet " + faucetObject.transform.position.y);
        reducerObject.transform.position += new Vector3(0, tapSizeY, 0);
        if (reducerObject.transform.position.y >= (faucetObject.transform.position.y + 20))
        {
            Debug.Log("success!");
            isPlaying = false;
            faucetSkeleton.AnimationState.Complete += EndGame;
            faucetSkeleton.AnimationState.SetAnimation(0, "faucet_off", false);
        }
    }

    void EndGame(Spine.TrackEntry entry)
    {
        faucetSkeleton.AnimationState.Complete -= EndGame;
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class DuelGame : MonoBehaviour {

    public GameObject point1;
    public GameObject point2;
    public GameObject point3;
    public GameObject point4;
    public GameObject point5;

    public GameObject enemyObject;
    public GameObject playerObject;

    SkeletonGraphic enemySkeleton;
    SkeletonGraphic playerSkeleton;
    SkeletonGraphic p1Skeleton;
    SkeletonGraphic p2Skeleton;
    SkeletonGraphic p3Skeleton;
    SkeletonGraphic p4Skeleton;
    SkeletonGraphic p5Skeleton;

    bool isEnemyActive = false;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    int numPoints = 0;

	// Use this for initialization
	void Start () {
        mdb = GameObject.FindObjectOfType<MiniGameDefaultBehavior>();
        mdb.isTimeGame = false;

        enemySkeleton = enemyObject.GetComponent<SkeletonGraphic>();
        playerSkeleton = playerObject.GetComponent<SkeletonGraphic>();

        p1Skeleton = point1.GetComponent<SkeletonGraphic>();
        p2Skeleton = point2.GetComponent<SkeletonGraphic>();
        p3Skeleton = point3.GetComponent<SkeletonGraphic>();
        p4Skeleton = point4.GetComponent<SkeletonGraphic>();
        p5Skeleton = point5.GetComponent<SkeletonGraphic>();

        InvokeRepeating("enemyAttack", 2.0f, 2.0f);
	}

    // Update is called once per frame
    void Update () {
        if (!mdb.gameStarted)
            return;

        if (!isPlaying)
            return;

        if (isEnemyActive)
            mdb.loseTime(0.07f);
        
        if (!mdb.hasTimeLeft() && numPoints < 5)
        {
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }
	}

    void enemyAttack()
    {
        bool fakeAttack = Random.Range(0, 100) < 40;

        if (fakeAttack)
        {
            enemySkeleton.AnimationState.SetAnimation(0, "up", false);
            return;
        }

        GameSound.gameSound.PlaySFX("toilet_flush");
        CancelInvoke();
        isEnemyActive = true;
        enemySkeleton.AnimationState.SetAnimation(0, "down", false);
    }

    public void playerAttack()
    {
        GameSound.gameSound.StopSFX();
        if (!isEnemyActive)
        {
            GameSound.gameSound.PlaySFX("error");
            playerSkeleton.AnimationState.SetAnimation(0, "tap", false);
            mdb.loseTime(1.0f);
            return;
        }

        GameSound.gameSound.PlaySFX("toilet_slap");

        numPoints ++;

        enemySkeleton.AnimationState.SetAnimation(0, "up", false);
        playerSkeleton.AnimationState.SetAnimation(0, "tap", false);

        isEnemyActive = false;

        if(numPoints == 1)
        {
            p1Skeleton.AnimationState.SetAnimation(0, "idle", false);
        }
        else if (numPoints == 2)
        {
            p2Skeleton.AnimationState.SetAnimation(0, "idle", false);
        }
        else if (numPoints == 3)
        {
            p3Skeleton.AnimationState.SetAnimation(0, "idle", false);
        }
        else if (numPoints == 4)
        {
            p4Skeleton.AnimationState.SetAnimation(0, "idle", false);
        }
        else if(numPoints == 5)
        {
            p5Skeleton.AnimationState.Complete += EndGame;
            p5Skeleton.AnimationState.SetAnimation(0, "idle", false);
            return;
        }

        InvokeRepeating("enemyAttack", 2.0f, 2.0f);
    }

    void EndGame(Spine.TrackEntry entry)
    {
        p3Skeleton.AnimationState.Complete -= EndGame;
        isPlaying = false;
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}
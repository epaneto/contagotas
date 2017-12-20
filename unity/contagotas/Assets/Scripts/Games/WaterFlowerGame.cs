using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class WaterFlowerGame : MonoBehaviour {
    public GameObject waterPot;
    public GameObject flower1;
    public GameObject flower2;
    public GameObject flower3;

    SkeletonGraphic flower1Skeleton;
    SkeletonGraphic flower2Skeleton;
    SkeletonGraphic flower3Skeleton;

    float flower1WaterLevel = 0;
    float flower2WaterLevel = 0;
    float flower3WaterLevel = 0;
    float waterDesired = 3.0f;

    bool flower1Done = false;
    bool flower2Done = false;
    bool flower3Done = false;

    SkeletonGraphic potSkeleton;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    bool isPotOn = false;
    bool allFlowersWatered = false;

    // Use this for initialization
    void Start () {

        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
        flower1Skeleton = flower1.GetComponent<SkeletonGraphic>();
        flower2Skeleton = flower2.GetComponent<SkeletonGraphic>();
        flower3Skeleton = flower3.GetComponent<SkeletonGraphic>();
        potSkeleton = waterPot.GetComponent<SkeletonGraphic>();
    }
	
    public void turnPotOn(bool isOn)
    {
        isPotOn = isOn;

        if(isPotOn)
        {
            GameSound.gameSound.PlayLoopSFX("regador");
            potSkeleton.AnimationState.SetAnimation(0, "on", false);
            potSkeleton.AnimationState.Complete += GoIdlePot;
        }
        else
        {
            GameSound.gameSound.StopSFX();
            potSkeleton.AnimationState.Complete -= GoIdlePot;
            potSkeleton.AnimationState.SetAnimation(0, "off", false);

            flower1Skeleton.AnimationState.SetAnimation(0, "down", true);
            flower2Skeleton.AnimationState.SetAnimation(0, "down", true);
            flower3Skeleton.AnimationState.SetAnimation(0, "down", true);
        }
    }

    void GoIdlePot(Spine.TrackEntry entry)
    {
        potSkeleton.AnimationState.Complete -= GoIdlePot;
        potSkeleton.AnimationState.SetAnimation(0, "idle", true);
    }

    // Update is called once per frame

    bool watering1 = false;
    bool watering2 = false;
    bool watering3 = false;

    void Update () {
        if (!mdb.gameStarted)
            return;

        if (!isPlaying)
            return;

        allFlowersWatered = (flower1Done && flower2Done && flower3Done);

        if (!mdb.hasTimeLeft() && !allFlowersWatered)
        {
            isPotOn = false;
            GameSound.gameSound.StopSFX();
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }else if(allFlowersWatered)
        {
            EndGame();
        }

        if (!isPotOn)
            return;

        //check flowers
        if (Mathf.Abs((waterPot.transform.position.x + 150) - flower1.transform.position.x) < 80 && !flower1Done)
        {
           
            if (flower1WaterLevel >= waterDesired)
            {
                Debug.Log("flower 1 beauty!");
                flower1Skeleton.AnimationState.SetAnimation(0, "beauty", false);
                flower1Done = true;
                return;
            }

            if (!watering1)
            {
                Debug.Log("flower 1 idle");
                flower1Skeleton.AnimationState.SetAnimation(0, "idle", true);
            }
            watering1 = true;

            flower1WaterLevel += 0.1f;

        }else{
            if (watering1 && !flower1Done)
            {
                Debug.Log("flower 1 down");
                flower1Skeleton.AnimationState.SetAnimation(0, "down", true);
            }
            watering1 = false;

        }

        if (Mathf.Abs((waterPot.transform.position.x + 150) - flower2.transform.position.x) < 80 && !flower2Done)
        {
            if (flower2WaterLevel >= waterDesired)
            {
                flower2Skeleton.AnimationState.SetAnimation(0, "beauty", false);
                flower2Done = true;
            }

            flower2WaterLevel += 0.1f;

            if (!watering2)
                flower2Skeleton.AnimationState.SetAnimation(0, "idle", true);
            watering2 = true;


        }else
        {
            if (watering2 && !flower2Done)
                flower2Skeleton.AnimationState.SetAnimation(0, "down", true);
            watering2 = false;
        }

        if (Mathf.Abs((waterPot.transform.position.x + 150) - flower3.transform.position.x) < 80 && !flower3Done)
        {
            if (flower3WaterLevel >= waterDesired)
            {
                flower3Skeleton.AnimationState.SetAnimation(0, "beauty", false);
                flower3Done = true;
            }

            flower3WaterLevel += 0.1f;

            if (!watering3)
                flower3Skeleton.AnimationState.SetAnimation(0, "idle", true);
            watering3 = true;

        }else{
            if (watering3 && !flower3Done)
                flower3Skeleton.AnimationState.SetAnimation(0, "down", true);
            watering3 = false;
        }

    }

    void EndGame()
    {
        isPotOn = false;
        GameSound.gameSound.StopSFX();
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

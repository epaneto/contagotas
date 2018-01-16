using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class WaterFlowerGame : MonoBehaviour {
    public GameObject waterPot;
    public List<GameObject> flowers;
    public List<bool> flowerReadyStatus;
    public List<float> flowerWaterStatus;
    float waterDesired = 1.0f;

    SkeletonGraphic potSkeleton;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    bool isPotOn = false;
    bool allFlowersWatered = false;

    // Use this for initialization
    void Start () {

        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
        //flower1Skeleton = flower1.GetComponent<SkeletonGraphic>();
        //flower2Skeleton = flower2.GetComponent<SkeletonGraphic>();
        //flower3Skeleton = flower3.GetComponent<SkeletonGraphic>();
        potSkeleton = waterPot.GetComponent<SkeletonGraphic>();

        for (int i = 0; i < flowers.Count; i++)
        {
            flowerReadyStatus.Add(false);
            flowerWaterStatus.Add(0);
        }
    }
	
    public void turnPotOn(bool isOn)
    {
        isPotOn = isOn;



        if(isPotOn)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = new Vector3(Input.mousePosition.x + 5.0f, Input.mousePosition.y - 50.0f, Input.mousePosition.z);
            waterPot.transform.position = mousePosition;

            GameSound.gameSound.PlayLoopSFX("regador");
            potSkeleton.AnimationState.SetAnimation(0, "on", false);
            potSkeleton.AnimationState.Complete += GoIdlePot;
        }
        else
        {
            GameSound.gameSound.StopSFX();
            potSkeleton.AnimationState.Complete -= GoIdlePot;
            potSkeleton.AnimationState.SetAnimation(0, "off", false);

            //flower1Skeleton.AnimationState.SetAnimation(0, "down", true);
            //flower2Skeleton.AnimationState.SetAnimation(0, "down", true);
            //flower3Skeleton.AnimationState.SetAnimation(0, "down", true);
        }
    }

    void GoIdlePot(Spine.TrackEntry entry)
    {
        potSkeleton.AnimationState.Complete -= GoIdlePot;
        potSkeleton.AnimationState.SetAnimation(0, "idle", true);
    }

    void Update () {
        if (!mdb.gameStarted)
            return;

        if (!isPlaying)
            return;

        allFlowersWatered = true;
        for (int i = 0; i < flowerReadyStatus.Count; i++)
        {
            if(flowerReadyStatus[i] == false){
                allFlowersWatered = false;
                break;
            }
        }

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
        for (int i = 0; i < flowers.Count; i++)
        {
            GameObject flower = flowers[i];

            if (Mathf.Abs((waterPot.transform.position.x + 150) - flower.transform.position.x) < 80 && (waterPot.transform.position.y - flower.transform.position.y) > 0 && (waterPot.transform.position.y - flower.transform.position.y) < 200)
            {
                if (flowerReadyStatus[i] == true)
                    return;

                SkeletonGraphic skeleton = flower.GetComponent<SkeletonGraphic>();
                flowerWaterStatus[i] += 0.1f;

                if (flowerWaterStatus[i] >= (waterDesired + (i * 0.4f)))
                {

                    flowerReadyStatus[i] = true;
                    skeleton.AnimationState.SetAnimation(0, "beauty", false);
                } else {

                    Debug.Log("is playing animation " + skeleton.AnimationState.GetCurrent(0).animation.name);
                    if (skeleton.AnimationState.GetCurrent(0).animation.name == "idle")
                        return;
                    
                    skeleton.AnimationState.SetAnimation(0, "idle", true);

                }

            } else {

                if (flowerReadyStatus[i] == false)
                {
                    SkeletonGraphic skeleton = flower.GetComponent<SkeletonGraphic>();
                    skeleton.AnimationState.SetAnimation(0, "down", true);
                }
            }

        }

    }

    void EndGame()
    {
        isPotOn = false;
        GameSound.gameSound.StopSFX();
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

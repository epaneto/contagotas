using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class ChangeableItem : MonoBehaviour {
    public bool isGood = true;
    bool originalStateBad = false; 
    public string badAnimation;
    public string goodAnimation;
    public GameObject colorFeedback;
    ChangeableColor changeableColor;

    MiniGameDefaultBehavior mdb;

    SkeletonGraphic itemSkeleton;

	// Use this for initialization
	void Start () {
        if(colorFeedback)
        {
            changeableColor = colorFeedback.GetComponent<ChangeableColor>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setBadState()
    {
        if (!isGood)
            return;

        originalStateBad = true;
        isGood = false;
        if(!itemSkeleton)
            itemSkeleton = this.gameObject.GetComponent<SkeletonGraphic>();
        itemSkeleton.AnimationState.SetAnimation(0, badAnimation, true);
    }

    public void SendMDB(MiniGameDefaultBehavior m)
    {
        mdb = m;
    }

    public void setGoodState()
    {
        if (!originalStateBad)
        {
            GameSound.gameSound.PlaySFX("error");
            mdb.loseTime(1.0f);
            if (changeableColor)
                changeableColor.changeColor(false);
        }

        if (isGood)
            return;
            
        GameSound.gameSound.PlaySFX("tap");
        isGood = true;
        if (changeableColor)
            changeableColor.changeColor(true);
        
        if (!itemSkeleton)
            itemSkeleton = this.gameObject.GetComponent<SkeletonGraphic>();
        itemSkeleton.AnimationState.SetAnimation(0, goodAnimation, false);
    }
}

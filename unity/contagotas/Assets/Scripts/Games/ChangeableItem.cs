using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class ChangeableItem : MonoBehaviour {
    public bool isGood = true;
    public string badAnimation;
    public string goodAnimation;

    SkeletonGraphic itemSkeleton;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setBadState()
    {
        if (!isGood)
            return;
        
        isGood = false;
        if(!itemSkeleton)
            itemSkeleton = this.gameObject.GetComponent<SkeletonGraphic>();
        itemSkeleton.AnimationState.SetAnimation(0, badAnimation, true);
    }

    public void setGoodState()
    {
        if (isGood)
            return;

        isGood = true;
        if (!itemSkeleton)
            itemSkeleton = this.gameObject.GetComponent<SkeletonGraphic>();
        itemSkeleton.AnimationState.SetAnimation(0, goodAnimation, false);
    }
}

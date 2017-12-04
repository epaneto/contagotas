using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class HoleDrop : MonoBehaviour {

    SkeletonGraphic dropSkeleton;
    PipeHolesGame controller;
    bool isClosed = false;
	// Use this for initialization
	void Start () {
        controller = FindObjectOfType<PipeHolesGame>();
        dropSkeleton = this.gameObject.GetComponent<SkeletonGraphic>();
	}
	
	// Update is called once per frame
	void Update () {
        
        this.gameObject.transform.Translate(Vector3.down * 11.0f);
        
        Debug.Log(this.gameObject.transform.position.y);
        if (this.gameObject.transform.position.y < 0)
        {
            Debug.Log("hole saiu fora");
            if(!isClosed)
                controller.missedHole();
        }
	}

    public void closedHole()
    {
        
        if (isClosed)
            return;
        
        Debug.Log("HOLE TOCOU");

        isClosed = true;
        dropSkeleton.AnimationState.SetAnimation(0, "off", false);
        controller.fixedPipe();
    }
}

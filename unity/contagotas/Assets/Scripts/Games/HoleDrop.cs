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
    float speed;

	// Use this for initialization
	void Start () {
        speed = -(Screen.height * 0.5f);
        controller = FindObjectOfType<PipeHolesGame>();
        dropSkeleton = this.gameObject.GetComponent<SkeletonGraphic>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("SCREEN HEIGHT " + speed);

        this.gameObject.transform.Translate(0,speed * Time.deltaTime,0);

        if (this.gameObject.transform.position.y < 0)
        {
            Debug.Log("hole saiu fora");
            if (!isClosed)
            {
                controller.missedHole();
                Destroy(this.gameObject);
            }
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

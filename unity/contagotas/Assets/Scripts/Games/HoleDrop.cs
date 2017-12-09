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
    float speed = 0;

	// Use this for initialization
	void Start () {
        controller = FindObjectOfType<PipeHolesGame>();
        dropSkeleton = this.gameObject.GetComponent<SkeletonGraphic>();
        speed = Screen.height * 0.01f;
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("SCREEN HEIGHT " + speed);

        this.gameObject.transform.Translate(Vector3.down * speed);

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

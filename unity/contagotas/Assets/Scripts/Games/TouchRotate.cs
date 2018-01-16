using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour {
    public bool isRight;
	public bool canRotate = false;
    public TouchRotateDrag TouchRotateDragScript;
    Vector3 DownInputValues;
	// Use this for initialization
	void Start () {
       
        int randomizeAngle = (int)Random.Range(1, 3);
        this.gameObject.transform.Rotate(new Vector3(0, 0, randomizeAngle * 90));
		this.enabled = false;
	}

    public void DownInput()
    {
        DownInputValues = Input.mousePosition;
    }
	
    public void RotateMe()
    {
        if (Input.mousePosition != DownInputValues)
            return;
        
        GameSound.gameSound.PlaySFX("tap");
        this.gameObject.transform.Rotate(new Vector3(0,0,90));
		Debug.Log ("ROTATED");
        CheckPipe();
    }

    public void CheckPipe()
    {
        int angle = (int)Mathf.Round(this.gameObject.transform.rotation.eulerAngles.z);

        if (angle == 0 && TouchRotateDragScript.isRight)
        {
            isRight = true;
            Debug.Log("CHECK!");
        }
    }
}

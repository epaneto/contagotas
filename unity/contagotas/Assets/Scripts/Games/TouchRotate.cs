using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotate : MonoBehaviour {
    public bool isRight;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RotateMe()
    {
        //this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        this.gameObject.transform.Rotate(new Vector3(0,0,90));

        int angle = (int)Mathf.Round(this.gameObject.transform.rotation.eulerAngles.z);

        if ( angle == 0 ){
            isRight = true;
            Debug.Log("CHECK!");
        }
    }
}

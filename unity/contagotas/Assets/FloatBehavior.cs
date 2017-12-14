using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatBehavior : MonoBehaviour {
    public float rangeY;
	// Use this for initialization
	void Start () {
        this.gameObject.transform.DOMoveY(this.gameObject.transform.position.y + rangeY,3.0f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutQuad).From();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

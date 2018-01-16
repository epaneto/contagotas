using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PipeHoleBehaviour : MonoBehaviour {

	PipeHolesGame controller;
	bool isClosed = false;
	float speed;

	[SerializeField]
	Transform bottomEndedTransform;

	// Use this for initialization
	void Start () {
		controller = FindObjectOfType<PipeHolesGame>();
		speed = -1;
	}

	void Update () {
		UpdatePipe ();

		if (this.gameObject.transform.position.y < bottomEndedTransform.transform.position.y)
		{
			Debug.Log("hole saiu fora");
			if (!isClosed)
			{
				controller.missedHole();
				Destroy(this.gameObject);
			}
		}
	}

	void UpdatePipe ()
	{
		this.transform.Translate (0, speed * Time.deltaTime, 0);
		speed -= (Time.deltaTime/2);
	}

	public void TappedHole()
	{
		if (isClosed)
			return;
		
		isClosed = true;
		controller.fixedPipe();
		SkeletonGraphic graphic = this.GetComponent<SkeletonGraphic> ();
		graphic.AnimationState.SetAnimation(0,"off",false);
	}
}

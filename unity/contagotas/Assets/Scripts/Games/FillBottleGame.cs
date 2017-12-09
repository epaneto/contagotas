using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class FillBottleGame : MonoBehaviour {

	public GameObject fillObject;
	public GameObject panObject;
	MiniGameDefaultBehavior mdb;
	bool isPlaying = true;
	bool isFilling = false;
	float maxOilFill = 22.0f;
	float minOilFill = 20.0f;
	SkeletonGraphic panGraph;

	// Use this for initialization
	void Start () {
		mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior> ();
		panGraph = panObject.GetComponent<SkeletonGraphic> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!mdb.gameStarted)
			return;

		if (!isPlaying)
			return;

		if (!mdb.hasTimeLeft()) {
			isPlaying = false;
			mdb.EndedGameLose ();
			return;
		}

		if (isFilling) {
			
			fillObject.transform.localScale += new Vector3(0, 0.4f, 0);

			if (fillObject.transform.localScale.y > maxOilFill) {
				isPlaying = false;
				mdb.EndedGameLose ();
				return;
			}

		}
	}

	public void pressPan()
	{
		isFilling = true;

		panGraph.AnimationState.SetAnimation(0,"on",false);
	}

	public void leavePan()
	{
		isFilling = false;
		panGraph.AnimationState.SetAnimation(0,"off",false);

		if (fillObject.transform.localScale.y > minOilFill && fillObject.transform.localScale.y < maxOilFill) {
			isPlaying = false;
			EndGame ();
		}
	}

	void EndGame()
	{
		isPlaying = false;
		mdb.EndedGameWin (mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using DG.Tweening;

public class FillBottleGame : MonoBehaviour {


	public GameObject[] oilObjects;
	public GameObject[] bottleObjects;

	private GameObject fillObject;
	private int fillOBjectIndex = 0;
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
		fillObject = oilObjects [fillOBjectIndex];
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
        GameSound.gameSound.PlayLoopSFX("oil_bottle");
		panGraph.AnimationState.SetAnimation(0,"on",false);
	}

	public void leavePan()
	{
		isFilling = false;
        GameSound.gameSound.StopSFX();
		panGraph.AnimationState.SetAnimation(0,"off",false);

		if (fillObject.transform.localScale.y > minOilFill && fillObject.transform.localScale.y < maxOilFill) {
			
			if (fillOBjectIndex < 2) {
				TranslateBottle ();
				fillOBjectIndex++;
				fillObject = oilObjects [fillOBjectIndex];
			} else {
				isPlaying = false;
				EndGame ();
			}
		}
	}

	private void TranslateBottle()
	{
		oilObjects [0].transform.DOMoveX (oilObjects [0].transform.position.x - 3, 1f);
		bottleObjects [0].transform.DOMoveX (bottleObjects [0].transform.position.x - 3, 1f);
		oilObjects [1].transform.DOMoveX (oilObjects [1].transform.position.x - 3, 1f);
		bottleObjects [1].transform.DOMoveX (bottleObjects [1].transform.position.x - 3, 1f);
		oilObjects [2].transform.DOMoveX (oilObjects [2].transform.position.x - 3, 1f);
		bottleObjects [2].transform.DOMoveX (bottleObjects [2].transform.position.x - 3, 1f);
	}

	void EndGame()
	{
		isPlaying = false;
		mdb.EndedGameWin (mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
	}
}

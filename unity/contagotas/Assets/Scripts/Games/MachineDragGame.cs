using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class MachineDragGame : MonoBehaviour {

	public List<GameObject> dishes;

	MiniGameDefaultBehavior mdb;
	bool isPlaying = true;
	bool allClothClean = false;

	// Use this for initialization
	void Start () {
		mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!mdb.gameStarted)
			return;

		if (!isPlaying)
			return;

		if (!mdb.hasTimeLeft() && !allClothClean) {
			isPlaying = false;
			mdb.EndedGameLose ();
			return;
		}


		allClothClean = true;
		for (int i = 0; i < dishes.Count; i++) {
			
			DragableObject obj = dishes [i].GetComponent<DragableObject> ();
			if (obj.isAtDestination == false) {
				allClothClean = false;
				break;
			}
		}

		if (allClothClean)
			EndGame ();
	}

	void EndGame()
	{
		isPlaying = false;
		mdb.EndedGameWin (mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
	}
}

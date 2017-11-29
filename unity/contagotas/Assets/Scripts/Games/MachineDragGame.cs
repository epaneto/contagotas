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
	Vector3 lastMouseCoordinate = Vector3.zero;
	bool isPlaying = true;
	bool allDishesClean = false;

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

		if (!mdb.hasTimeLeft() && !allDishesClean) {
			isPlaying = false;
			mdb.EndedGameLose ();
			return;
		}

		RaycastHit2D hitInfo = Physics2D.Raycast (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, 0);

		if (Input.GetMouseButton (0)) {

			Vector3 mouseDelta = Input.mousePosition - lastMouseCoordinate;
			lastMouseCoordinate = Input.mousePosition;

			if (hitInfo) {
				
				if (mouseDelta.x > 5 || mouseDelta.y > 5) {
					GameObject colliderObject = hitInfo.collider.gameObject;
					Image colliderImage = colliderObject.GetComponent<Image> ();
					Color c = colliderImage.color;
					Debug.Log (c.a);
					if (c.a > 0) {
						Debug.Log ("lower alpha");
						c.a -= 0.03f;
						colliderImage.color = c;

						float totalAlpha = 0;
						for (int i = 0; i < dishes.Count; i++) {
							GameObject dish = dishes [i];
							Image dishImage = dish.transform.GetComponent<Image> ();
							totalAlpha += dishImage.color.a;
						}

						if (totalAlpha <= 0) {
							isPlaying = false;
							allDishesClean = true;
							EndGame ();
						}
					}
				}


			}
		} 
	}

	void EndGame()
	{
		//faucet.AnimationState.Complete -= EndGame;

		mdb.EndedGameWin (mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
	}
}

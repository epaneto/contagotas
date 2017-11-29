using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class BathGame : MonoBehaviour {

	public GameObject sponge;
	Vector3 spongeInitialPos;

	MiniGameDefaultBehavior mdb;
	Vector3 lastMouseCoordinate = Vector3.zero;
	bool isPlaying = true;
	float dirtAlpha = 0;
	float maxScore = 100.0f;

	// Use this for initialization
	void Start () {
		mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior> ();

		spongeInitialPos = sponge.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (!mdb.gameStarted)
			return;

		if (!isPlaying)
			return;

		if (!mdb.hasTimeLeft() && dirtAlpha > 0) {
			isPlaying = false;
			mdb.EndedGameLose ();
			return;
		}

		RaycastHit2D hitInfo = Physics2D.Raycast (new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y), Vector2.zero, 0);

		if (Input.GetMouseButton (0)) {

			Transform newSpongeTransform = sponge.transform;
			newSpongeTransform.position = hitInfo.point;


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
						c.a -= 0.007f;
						dirtAlpha = c.a;
						colliderImage.color = c;

						if (dirtAlpha <= 0) {
							isPlaying = false;
							EndGame ();
						}
					}
				}


			} else {
				sponge.transform.position = spongeInitialPos;
			}
		} 
	}

	void EndGame()
	{
		//faucet.AnimationState.Complete -= EndGame;

		mdb.EndedGameWin (mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
	}
}

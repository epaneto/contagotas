using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerRankingBaseAssetsGroupManager : MonoBehaviour {

	[Header("Group Screen References")]
	[SerializeField]
	GameObject GroupGameObjects;

	[SerializeField]
	GameObject[] ObjectsToTween;

	private float[] originalY;

	protected PlayerRankingScreenManager screenManager;

	void InitializeOriginalYPositions ()
	{
		if (ObjectsToTween != null && ObjectsToTween.Length > 0) {
			int index = 0;
			originalY = new float[ObjectsToTween.Length];
			foreach (var item in ObjectsToTween) {
				originalY [index] = item.transform.position.y;
				item.transform.DOMoveY (-5000, 0.01f, false);
				index++;
			}
		}

	}


	public void Initialize(PlayerRankingScreenManager screenManager)
	{
		this.screenManager = screenManager;
		InitializeOriginalYPositions ();

	}

	public void SetScreen(bool value)
	{
		GroupGameObjects.SetActive (value);

		if (ObjectsToTween != null && ObjectsToTween.Length > 0) {
			if (value) {
				float time = 1;
				for (int i = 0; i < ObjectsToTween.Length; i++) {	
					ObjectsToTween [i].transform.DOMoveY (originalY [i], time, true);
					time += 0.2f;
				}
			} else {
				float time = 1;
				for (int i = ObjectsToTween.Length - 1; i >= 0; i--) {
					ObjectsToTween [i].transform.DOMoveY (-5000, time, true);
					time += 0.2f;
				}
			}
		}
	}

}

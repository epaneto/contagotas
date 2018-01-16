using DG.Tweening;
using UnityEngine;


public class SpinnerRotation : MonoBehaviour {

	[SerializeField]
	GameObject objectToRotate;

	// Update is called once per frame
	void Update () {
		objectToRotate.transform.DORotate (new Vector3 (0, 0, 999), 5, RotateMode.FastBeyond360); 
	}
}

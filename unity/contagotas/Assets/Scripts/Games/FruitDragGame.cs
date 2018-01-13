using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class FruitDragGame : MonoBehaviour {

	float dropInterval = 0.5f;
	public GameObject dropParent;
	MiniGameDefaultBehavior mdb;
	bool isPlaying = true;

	int neededDrops = 20;
	int collectedDrops = 0;

	// Use this for initialization
	void Start () {
		mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();

		EventManager.StartListening("MiniGameStarted", StartRain);
	}

	void StartRain()
	{
		EventManager.StopListening("MiniGameStarted", StartRain);
		InvokeRepeating("launchDrop", 0.5f, dropInterval);
	}

	// Update is called once per frame
	void Update () {
		if(!mdb.gameStarted)

			return;

		if (!isPlaying)
			return;

		if (!mdb.hasTimeLeft() && collectedDrops < neededDrops)
		{
			CancelInvoke();
			isPlaying = false;
			mdb.EndedGameLose();
			return;
		}
	}

	void launchDrop()
	{
		int randomNumber = (int)Random.Range (0, 10);

		GameObject drop;
		switch (randomNumber) {
		case 2:
			drop = Instantiate (Resources.Load ("drop_trash_1", typeof(GameObject)), new Vector3 (Random.Range (-400, 400), 0, 0), dropParent.transform.rotation) as GameObject;
			break;
		case 3:
			drop = Instantiate (Resources.Load ("drop_trash_2", typeof(GameObject)), new Vector3 (Random.Range (-400, 400), 0, 0), dropParent.transform.rotation) as GameObject;
			break;
		case 4:
			drop = Instantiate (Resources.Load ("drop_trash_3", typeof(GameObject)), new Vector3 (Random.Range (-400, 400), 0, 0), dropParent.transform.rotation) as GameObject;
			break;
		case 5:
			drop = Instantiate (Resources.Load ("drop_orange", typeof(GameObject)), new Vector3 (Random.Range (-400, 400), 0, 0), dropParent.transform.rotation) as GameObject;
			break;
		case 6:
			drop = Instantiate (Resources.Load ("drop_pear", typeof(GameObject)), new Vector3 (Random.Range (-400, 400), 0, 0), dropParent.transform.rotation) as GameObject;
			break;
		case 7:
		case 8:
		case 9:
			drop = Instantiate (Resources.Load ("fruit_drop_special", typeof(GameObject)), new Vector3 (Random.Range (-400, 400), 0, 0), dropParent.transform.rotation) as GameObject;
			break;

		default:
			drop = Instantiate(Resources.Load("drop_apple", typeof(GameObject)), new Vector3(Random.Range(-400,400),0,0), dropParent.transform.rotation) as GameObject;
			break;
		}
		drop.transform.SetParent(dropParent.transform, false);
	}

	public void SpecialCollected()
	{
		mdb.winTime(2.0f);
        GameSound.gameSound.PlaySFX("collect_X");
	}

	public void missedDrop()
	{
		mdb.loseTime(1.0f);
		//mdb.EndedGameLose();
	}

	public void CollectedDrop()
	{
		collectedDrops++;
		GameSound.gameSound.PlaySFX("rain_drop");
		if(collectedDrops >= neededDrops)
		{
			EndGame();
		}

	}

	void EndGame()
	{
		CancelInvoke();
		isPlaying = false;
		mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
	}
}

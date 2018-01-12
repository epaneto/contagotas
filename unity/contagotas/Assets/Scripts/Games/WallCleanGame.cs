using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

public class WallCleanGame : MonoBehaviour {

    public List<GameObject> dishes;
    public GameObject sponge;
    MiniGameDefaultBehavior mdb;
    Vector3 lastMouseCoordinate = Vector3.zero;
    bool isPlaying = true;
    bool allDishesClean = false;

    // Use this for initialization
    void Start()
    {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mdb.gameStarted)
            return;

        if (!isPlaying)
            return;

        if (!mdb.hasTimeLeft() && !allDishesClean)
        {
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }

        Transform newSpongeTransform = sponge.transform;
        newSpongeTransform.position = Input.mousePosition;

        float totalAlpha = 0;
        for (int i = 0; i < dishes.Count; i++)
        {
            GameObject dish = dishes[i];
            Image dishImage = dish.transform.GetComponent<Image>();
            totalAlpha += dishImage.color.a;
        }

        if (totalAlpha <= 0)
        {
            isPlaying = false;
            allDishesClean = true;
            EndGame();
        }

    }

    public void ChangeDustAlpha(GameObject dust)
    {
        Image colliderImage = dust.GetComponent<Image>();
        Color c = colliderImage.color;

        if (c.a > 0)
        {
            c.a -= 0.1f;
            colliderImage.color = c;
        }
    }

    void EndGame()
    {
        //faucet.AnimationState.Complete -= EndGame;

        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ValveGame : MonoBehaviour {
    
    public List<GameObject> valves;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;
    public GameObject valveGroup;

    Vector3 mouse_pos;
    Transform target;
    Vector3 object_pos;
    float angle;
    float prevAngle;
    float totalAnglesRotated = 0;
    float totalRotations = 0;
    float neededRotations = 10;
    int valveIndex = 0;
    GameObject valveObject;



    // Use this for initialization
    void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
        valveObject = valves[valveIndex];
    }
	
	// Update is called once per frame
	void Update () {
        if (!mdb.gameStarted)
            return;

        if (!isPlaying)
            return;

        if (!mdb.hasTimeLeft() && totalRotations < neededRotations)
        {
            isPlaying = false;
            mdb.EndedGameLose();
            return;
        }
    }

   

    public void onDragValve()
    {
        if (!isPlaying)
            return;
        
        mouse_pos = Input.mousePosition;
        mouse_pos.z = -20;
        object_pos = Camera.main.WorldToScreenPoint(valveObject.transform.position);
        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;
        angle = (Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg) + 180;
        valveObject.transform.rotation = Quaternion.Euler(0, 0, angle);

        if(angle < prevAngle && Mathf.Abs(prevAngle - angle) < 180)
        {
            totalAnglesRotated += Mathf.Abs(prevAngle - angle);
            totalRotations = totalAnglesRotated / 360;

            Debug.Log(totalRotations);
            if (totalRotations >= neededRotations)
            {
                if(valveIndex + 1 < valves.Count)
                {
                    isPlaying = false;
                    ShowNextValve();
                }else{
                    isPlaying = false;
                    EndGame();
                }

            }
        }
        
        prevAngle = angle;
    }

    void ShowNextValve()
    {
        Debug.Log("Show Next Valve");

        valveIndex++;
        valveObject = valves[valveIndex];

        angle = 0;
        prevAngle = 0;
        totalAnglesRotated = 0;
        totalRotations = 0;
        neededRotations = 10;

        valveGroup.transform.DOLocalMoveX(-1900 * valveIndex, 0.6f).SetEase(Ease.InQuad).OnComplete(releaseGame);
    }

    void releaseGame()
    {
        isPlaying = true;
    }


    void EndGame()
    {
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}
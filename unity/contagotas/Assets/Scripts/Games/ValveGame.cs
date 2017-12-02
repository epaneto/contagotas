using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveGame : MonoBehaviour {

    public GameObject valveObject;
    MiniGameDefaultBehavior mdb;
    bool isPlaying = true;

    Vector3 mouse_pos;
    Transform target;
    Vector3 object_pos;
    float angle;
    public bool isLocal;
    float prevAngle;
    float totalAnglesRotated = 0;
    float totalRotations = 0;
    float neededRotations = 10;


    // Use this for initialization
    void Start () {
        mdb = this.gameObject.GetComponent<MiniGameDefaultBehavior>();
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
                isPlaying = false;
                EndGame();
            }
        }
        
        prevAngle = angle;
    }

    void EndGame()
    {
        mdb.EndedGameWin(mdb.maxScore - (mdb.maxScore * mdb.getTimeProgress()));
    }
}
// --------------------------------------------------------- 
// CameraManager.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class CameraManager : MonoBehaviour
{
    private Vector3 targetTransfrom;


    private Vector3 targetPosition;
    private Vector3 targetStartPosition;

    Vector3 backGroundPosition;
    Vector3 myPosition;

    Vector3 backGroundStartPosition;
    Vector3 myStartPosition;

    private bool _isStart = false;
    IReadOnlyList<StationaryPart> stationaryParts = default;
    private void Awake()
    {


        myPosition = this.transform.position;

        myStartPosition = myPosition;
        backGroundStartPosition = backGroundPosition;
    }



    public void CameraStart()
    {
        stationaryParts = GameObject.FindWithTag("Player").GetComponent<Player>().Parts;
        //float x = default, y = default, z = default;
        //for (int i = 0; i < stationaryParts.Count; i++)
        //{
        //    Debug.Log(stationaryParts[i].gameObject.transform.position.x);
        //    x += stationaryParts[i].gameObject.transform.position.x;
        //    y += stationaryParts[i].gameObject.transform.position.y;
        //    z += stationaryParts[i].gameObject.transform.position.z;
        //}
        //x = x / stationaryParts.Count;
        //y = y / stationaryParts.Count;
        //z = z / stationaryParts.Count;
        targetTransfrom = stationaryParts[0].gameObject.transform.position;

        targetStartPosition = targetPosition;
        targetPosition = targetTransfrom;
        targetPosition.z -= 10f;

    }

    private void Update()
    {

        

        if (GameObject.FindWithTag("Player") != null)
        {
            stationaryParts = GameObject.FindWithTag("Player").GetComponent<Player>().Parts;

        }

        if (stationaryParts == null)
        {
            return;
        }
        else if (!_isStart && stationaryParts.Count != 0)
        {
            targetTransfrom = stationaryParts[0].gameObject.transform.position;
            targetStartPosition = targetPosition;
            targetPosition = targetTransfrom;
            targetPosition.z -= 10f;
            _isStart = true;
        }


        if (stationaryParts.Count == 0)
        {
            transform.position = myStartPosition;
            return;
        }



        backGroundPosition.x -= targetStartPosition.x - stationaryParts[0].gameObject.transform.position.x;


        myPosition.x -= targetStartPosition.x - stationaryParts[0].gameObject.transform.position.x;
        this.transform.position = myPosition;

        if(transform.position.x < myStartPosition.x)
        {
            this.transform.position = myStartPosition;
        }

        if(transform.position.x > 15f)
        {
            Vector3 pos = new Vector3(15f, transform.position.y, transform.position.z);
            transform.position = pos;
        }


        backGroundPosition = backGroundStartPosition;
        myPosition = myStartPosition;

    }

}
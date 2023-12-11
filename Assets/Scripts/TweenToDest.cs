using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenToDest : MonoBehaviour
{
    private Vector3 _startPos;
    public float timeToReachDestination = 1f;
    private float _time;

    bool _running;

    public GameObject destination;
    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        if(_running)
        {
            _time += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPos, destination.transform.position, _time / timeToReachDestination);
        }
    }

    public void StartTween()
    {
        _startPos = transform.position;
        _time = 0;
        _running = true;
    }
}

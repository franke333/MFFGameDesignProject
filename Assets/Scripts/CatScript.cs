using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatScript : MonoBehaviour
{
    public GameObject destination;
    public GameObject destinationCinematic;

    [Range(1, 120)]
    public float timeToReachDestination = 1f;
    [Range(0.5f, 10)]
    public float cinematicLength = 1f;

    private Vector3 _startPos;
    private Vector3 _startScale;
    private float _time;
    private bool _isCinematic = false;
    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
        _startScale = transform.localScale;
        _time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        transform.position = Vector3.Lerp(_startPos, _isCinematic ? destinationCinematic.transform.position : destination.transform.position, _time / timeToReachDestination);
    }
        

    public void StartCinematic()
    {
        _startPos = transform.position;
        transform.localScale = _startScale;
        _time = 0;
        transform.position = destinationCinematic.transform.position;
        _isCinematic = true;
        timeToReachDestination = cinematicLength;
    }

    public void Enlarge()
    {
        if(_isCinematic)
        {
            return;
        }
        transform.localScale *= 1.2f;
    }
}

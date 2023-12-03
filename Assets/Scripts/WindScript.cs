using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    [SerializeField]
    GameObject _leafPrefab;
    [SerializeField]
    RepairMan _repairMan;

    [SerializeField]
    int _leafCount = 10;

    [SerializeField]
    float _leafSpeedMultiplier = 1f, _leafFallSpeed;

    public List<GameObject> _leafs = new List<GameObject>();

    [SerializeField]
    AnimationCurve _windForceCurve;
    [ReadOnly(true)]
    [SerializeField]
    private float _windForce = 0f;
    private float _startForce = 0f, _targetForce = 0f, _windTimeMultiplier = 1f;
    private float _currentTime = 0f, _segmentTime = 1f, _totalTime = 0f;

    [Header("Difficulty by Time")]
    [SerializeField]
    private float _skyrocketSpeedAt = 60f;
    [SerializeField]
    private float _addForcePerSecond = 0.03f;

    // leaf area
    [Header("Leaf Area")]
    [SerializeField]
    private float _leftSide = -10f;
    [SerializeField]
    private float _rightSide = 10f, _topSide = 10f, _bottomSide = -10f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _leafCount; i++)
        {
            var leaf = Instantiate(_leafPrefab);
            leaf.transform.position = new Vector3(Random.Range(_leftSide, _rightSide), Random.Range(_bottomSide, _topSide), 0f);
            leaf.transform.parent = transform;
            leaf.transform.localScale = Vector3.one;
            _leafs.Add(leaf);
        }
    }

    void ChooseNewWindTarget()
    {
        _currentTime = 0f;
        _segmentTime = Random.Range(2f, 5f);
        _targetForce = Random.Range(-1f, 1f);
        _startForce = _windForce;
    }

    // Update is called once per frame
    void Update()
    {
        _windForce = Mathf.Lerp(_startForce, _targetForce, _windForceCurve.Evaluate(_currentTime/_segmentTime));

        _currentTime += Time.deltaTime;
        _totalTime += Time.deltaTime;

        
        _windTimeMultiplier += _addForcePerSecond * Time.deltaTime;

        if (_totalTime > _skyrocketSpeedAt)
        {
            _windTimeMultiplier += 1f * Time.deltaTime;
        }

        if (_currentTime > _segmentTime)
        {
            ChooseNewWindTarget();
        }

        foreach(GameObject leaf in _leafs)
        {
            float dy = -(1f - Mathf.Abs(_windForce) * 0.5f) * _leafFallSpeed * Time.deltaTime ;
            float dx = _windForce * _leafSpeedMultiplier * Time.deltaTime * _windTimeMultiplier; 
            leaf.transform.position += new Vector3(dx, dy, 0f) * transform.lossyScale.x;
            if (leaf.transform.position.x > _rightSide)
            {
                leaf.transform.position = new Vector3(_leftSide, Random.Range(_bottomSide, _topSide), 0f);
            }
            else if (leaf.transform.position.x < _leftSide)
            {
                leaf.transform.position = new Vector3(_rightSide, Random.Range(_bottomSide, _topSide), 0f);
            }
            if (leaf.transform.position.y < _bottomSide)
            {
                leaf.transform.position = new Vector3(Random.Range(_leftSide, _rightSide), _topSide, 0f);
            }
        }   
        _repairMan.WindForce = -_windForce * _windTimeMultiplier;

    }

    private void OnDrawGizmosSelected()
    {
        //display leaf area
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(_leftSide, _bottomSide, 0f), new Vector3(_leftSide, _topSide, 0f));
        Gizmos.DrawLine(new Vector3(_rightSide, _bottomSide, 0f), new Vector3(_rightSide, _topSide, 0f));
    }
}

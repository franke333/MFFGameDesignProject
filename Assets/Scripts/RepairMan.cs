using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairMan : MonoBehaviour
{
    // bunch of rotational forces
    [SerializeField]
    float overAllForceMultiplier = 1f, inputForceMultiplier = 1f, gravityForceMultiplier = 1f, windForceMultiplier = 1f;

    [SerializeField]
    AnimationCurve rotationGravityForce;

    Rigidbody2D rb;

    public float WindForce = 0f;

    // in degrees
    float gameOverLimit = 45f;
    bool _gameOver = false;

    public EndSceneDramaticScript endSceneDramaticScript;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    bool IsGameOver()
    {
        if(_gameOver)
            return true;
        //get z rotation in degrees
        float z = transform.rotation.eulerAngles.z;

        _gameOver = z > gameOverLimit && z < 360f-gameOverLimit;
        return _gameOver;
    }

    void ApplyRotForce(float x)
    {
        rb.AddTorque(x * overAllForceMultiplier);
    }

    void Update()
    {
        if (IsGameOver())
        {
            Debug.Log("Game Over");
            endSceneDramaticScript.Run();
            Destroy(this); // to stop running Update()
            return;
        }

        // input Force
        float x = Input.GetAxis("Horizontal"); 
        ApplyRotForce(-x * Time.deltaTime * inputForceMultiplier);

        // gravity tilt force
        x = transform.rotation.w * transform.rotation.z * 2f;
        x /= gameOverLimit; // [-1, 1]
        x = rotationGravityForce.Evaluate(x) * Mathf.Sign(x);
        ApplyRotForce(x * Time.deltaTime * gravityForceMultiplier);

        //TODO wind
        ApplyRotForce(WindForce * Time.deltaTime * windForceMultiplier);
    }




}

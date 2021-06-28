using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLanding : MonoBehaviour
{
    public float DownSpeed = 0.1f;
    public float UpSpeed = 0.1f;

    public float DownAmount;

    public GameObject ParticleSystem;

    public float AfterTheLandTimer;
    float _Timer;

    public float DivisionY;

    PlayerMovement Pm;
    
    void Awake()
    {
        Pm = GetComponentInParent<PlayerMovement>();
    }
    
    void Update()
    {
        _Timer -= Time.deltaTime;

        if (Pm.isGrounded && !Pm.isTouching)
        {
            _Timer = Timer;
        }


        if (_Timer > 0)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, (Pm.GetComponent<Rigidbody>().velocity.y / DivisionY) + -DownAmount, 0), DownSpeed * Time.timeScale);
        }
        if (_Timer <= 0)
        {
            transform.position = Vector3.Lerp(transform.position, UpPos.transform.position, UpSpeed * Time.timeScale);
        }
    }
}

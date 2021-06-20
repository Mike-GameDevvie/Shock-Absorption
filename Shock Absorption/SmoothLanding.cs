using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLanding : MonoBehaviour
{
    public float DownSpeed = 0.1f;
    public float UpSpeed = 0.1f;
    public GameObject DownPos;
    public GameObject UpPos;

    public GameObject ParticleSystem;

    public float Timer;
    float _Timer;

    public float DivisionY;

    public PlayerMovement Pm;
    public bool isTouching;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _Timer -= Time.deltaTime;

        if (Pm.isGrounded && !Pm.isTouching)
        {
            _Timer = Timer;
        }


        if (_Timer > 0)
        {
            transform.position = Vector3.Lerp(transform.position, DownPos.transform.position + new Vector3(0, Pm.GetComponent<Rigidbody>().velocity.y / DivisionY   , 0), DownSpeed * Time.timeScale);
        }
        if (_Timer <= 0)
        {
            transform.position = Vector3.Lerp(transform.position, UpPos.transform.position, UpSpeed * Time.timeScale);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 17f;
    public Vignette Vignette;
    public ChromaticAberration _ChromaticAberration;
    public ColorAdjustments _ColourAdjustments;
    public LensDistortion _LensDistortion;
    public float Sensitivity = 1.5f;
    public float Countermovement = 8f;
    public float JumpVel = 15f;
    public float AirSpeed = 15f;
    public float StepHeight = 0.1f;
    public float SmoothSpeed = 0.1f;
    public float Show = 0.1f;
    public float Health = 100f;
    public float Stamina = 100;
    public float StaminaMultiplier = 1.5f;
    public float SlowDownFactor = 100;
    public KeyCode Key = KeyCode.C;
    public Volume V;
    float RegSpeed;
    public float  JumpIndex;
    Rigidbody rb;
    [HideInInspector]
    public bool isGrounded;
    public bool CanRun;
    public bool isTouching;
    public GameObject DeathEffect;
    public GameObject JumpEffect;
    public GameObject Knee;
    public GameObject ParticleSystem;
    public GameObject Feet;
    public GameObject PostProcessFolme;
    public Slider S;
    public Slider St;

    float originalScale;

float X;

    // Start is called before the first frame update
    void Start()
    {
        V.profile.TryGet(out Vignette);
        V.profile.TryGet(out _LensDistortion);
        V.profile.TryGet(out _ChromaticAberration);
        V.profile.TryGet(out _ColourAdjustments);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        RegSpeed = MoveSpeed;
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        originalScale = Time.fixedDeltaTime;
        Knee.transform.position = new Vector3(Knee.transform.position.x, StepHeight, Knee.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

        S.value = Mathf.Lerp(S.value, Health, 0.05f);
        St.value = Mathf.Lerp(St.value, Stamina, 0.05f);

        if (isGrounded)
        {
            rb.drag = Countermovement;
        }
        else
        {
            rb.drag = 0;
        }

        if(Input.GetKey(KeyCode.LeftShift) && St.value < 10)
        {
            CanRun = false;
        }
        else
        {
            CanRun = true;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {

        }
        else
        {
            Stamina += 3f * Time.deltaTime;
        }

        if(Stamina > 100)
        {
            Stamina = 100;
        }

        if (Input.GetKey(KeyCode.LeftShift) && St.value > 0)
        {
            if(St.value < 1)
            {
                StaminaMultiplier = 0.5f;
            }

            if (isGrounded)
            {
                MoveSpeed = 2000 * StaminaMultiplier;
            }
            else
            {
                MoveSpeed = 15 * 5;
            }
            
            if (Input.GetKey("w"))
            {
                Stamina -= 0.05f * Time.timeScale;
            }
            if (Input.GetKey("s"))
            {
                Stamina -= 0.05f * Time.timeScale;
            }
            if (Input.GetKey("a"))
            {
                Stamina -= 0.05f * Time.timeScale;
            }
            if (Input.GetKey("d"))
            {
                Stamina -= 0.05f * Time.timeScale;
            }
        }
        else if(Input.GetKey(KeyCode.LeftShift))
        {
            Stamina += 0.35f;
        }

        Movement();

        if (Health <= 0)
        {
            Health = 100;
            Instantiate(DeathEffect, transform.position, transform.rotation);
        }

        DetectForGround();
        Rotate();

        if (Input.GetKey(Key))
        {
            AudioSource[] Audios = FindObjectsOfType<AudioSource>();
            foreach(AudioSource AU in Audios){AU.pitch = Random.Range(0.2f, 0.5f);}
            _ColourAdjustments.saturation.value = Mathf.Lerp(_ColourAdjustments.saturation.value, -25f, SmoothSpeed);
            _ChromaticAberration.intensity.value = Mathf.Lerp(_ChromaticAberration.intensity.value, 1, SmoothSpeed); 
            _LensDistortion.intensity.value = Mathf.Lerp(_LensDistortion.intensity.value, -0.26f, SmoothSpeed); 
            Vignette.intensity.value = Mathf.Lerp(Vignette.intensity.value, 0.34f, SmoothSpeed / 2);
            Time.timeScale = Mathf.Lerp(Time.timeScale, SlowDownFactor, SmoothSpeed);
            Time.fixedDeltaTime = Time.timeScale * .02f;}
            else{_LensDistortion.intensity.value = Mathf.Lerp(_LensDistortion.intensity.value, -0f, SmoothSpeed);
            Vignette.intensity.value = Mathf.Lerp(Vignette.intensity.value, 0f, SmoothSpeed / 2);
            _ChromaticAberration.intensity.value = Mathf.Lerp(_ChromaticAberration.intensity.value, 0.3f, SmoothSpeed);
            _ColourAdjustments.saturation.value = Mathf.Lerp(_ColourAdjustments.saturation.value, 00f, SmoothSpeed);
            AudioSource[] Audios = FindObjectsOfType<AudioSource>();
            foreach (AudioSource AU in Audios){ AU.pitch = Random.Range(0.6f, 1.2f);}
            Time.fixedDeltaTime = originalScale;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1, SmoothSpeed);
        }

        if (isGrounded)
        {
            JumpIndex = 2;
        }
        

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && JumpIndex == 2)
        {
            Instantiate(JumpEffect, transform.position - new Vector3(0, FeetPosition, 0), transform.rotation);
            JumpIndex -= 1;
            Stamina -= 10;
            rb.velocity = new Vector3(rb.velocity.x, JumpVel, rb.velocity.z);
        }
        if(Input.GetKeyDown(KeyCode.Space) && !isGrounded && JumpIndex == 2)
        {
            JumpIndex = 0;
            Stamina -= 10;
            rb.velocity = new Vector3(rb.velocity.x, JumpVel, rb.velocity.z);
        }
    }

    private void FixedUpdate()
    {
        
    }

    void Movement()
    {
        if (Input.GetKey("w"))
        {
            rb.AddForce(transform.forward * MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey("s"))
        {
            rb.AddForce(transform.forward * -MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            rb.AddForce(transform.right * -MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(transform.right * MoveSpeed * Time.deltaTime);
        }
    }

    void DetectForGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, Show))
        {
            isGrounded = true;
            if (hit.normal != Vector3.up)
            {
                if (rb.velocity.y < 0)
                {
                    GetComponent<Collider>().material.staticFriction = 10;
                    GetComponent<Collider>().material.dynamicFriction = 10;
                }
                else
                {
                    GetComponent<Collider>().material.staticFriction = 0;
                    GetComponent<Collider>().material.dynamicFriction = 0;
                }
            }
            else
            {
                GetComponent<Collider>().material.staticFriction = 0;
                GetComponent<Collider>().material.dynamicFriction = 0;
            }
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {

        }
        else if (isGrounded)
        {
            MoveSpeed = RegSpeed;
        }
        else if(!isGrounded)
        {
            MoveSpeed = AirSpeed;
        }
            
    }

    public float FeetPosition;
    private void OnCollisionExit(Collision collision)
    {
        isTouching = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        isTouching = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isGrounded)
        {
            Instantiate(ParticleSystem, transform.position - new Vector3(0, FeetPosition, 0), Quaternion.identity);
        }
    }

    void Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Sensitivity;

        mouseY -= X;

        Camera.main.transform.Rotate(Vector3.left * mouseY);
        transform.Rotate(transform.up * mouseX);
    }
}

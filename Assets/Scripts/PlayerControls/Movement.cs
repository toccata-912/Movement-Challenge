using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
//using static DG.Tweening.DOTweenModuleUtils;

public class Movement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [Header("Speed")]
    [SerializeField] public float sprintSpeed = 22f;
    [SerializeField] public float normalSpeed = 11f;
    [SerializeField] float crouchSpeed = 6f;
    [SerializeField] float speed = 11f;

    [Header("AUDIO")]
    [SerializeField] AudioClip walknGrass;
    [SerializeField] float timeBetweenSteps = 0.6f;
    float walkAudioCooldown = 0;

    bool moving = false;
    bool jumpPressed = false;
    bool crouchPressed;
    bool crouching = false;
    bool cancelCrouch = false;
    bool sprinting = false;
    bool changedToBigFov = false;
    bool changedToSmallFov = false;
    bool sliding = false;

    [Header("Size normal/crouch")]
    [SerializeField] Vector3 crouchSize = new Vector3(1, 0.5f, 1);
    [SerializeField] Vector3 normalSize = new Vector3(1, 1, 1);
    [SerializeField] Vector3 headCollider = new Vector3(0, 2.7f, 0);

    [Header("Jumping and gravity")]
    [SerializeField] float jumpHeight = 3.5f;
    [SerializeField] float gravity = -30f;
    Vector2 horizontalInput;
    Vector3 verticalVecolcity = Vector3.zero;

    [Header("What is ground")]
    [SerializeField] LayerMask groundMask;
    bool isGrounded;

    Vector2 smoothInputVelocity;

    [Header("Sliperines")]
    [SerializeField] float groundSmoothTime = 0.05f;
    [SerializeField] float airSmoothTime = 0.3f;
    float smoothTime;


    Vector3 currentInput;

    [Header("FOV settings")]
    [SerializeField] float sprintFOVChangeSpeed = 10f;
    [SerializeField] float normalFov = 90;
    [SerializeField] float runningFov = 120;

    Coroutine sprintFOVCoroutine;



    [SerializeField] CinemachineVirtualCamera playerCamera;
    [SerializeField] GameObject playerShape;
    [SerializeField] GameObject headPosition;

    Vector3 force = Vector3.zero;

    [SerializeField] float firstForceDamping = 2;
    [SerializeField] float secondForceDamping = 5;

    public float smoothTime2 = 0.2f;                 // Time to smoothly transition between FOV values
    public float velocityMultiplier = 0.5f;         // How much the velocity affects the FOV

    private float currentFOV;                       // Internal variable to track the current FOV
    private float fovVelocity;                      // Used by SmoothDamp to smooth the FOV transition

    float sprintTime = 2;
    float sprintTimePassed = 0;

    bool jumpCancelled = false;

    private void Update()
    {
        //checks if player is on the ground 
        if (crouching)
        {
            isGrounded = Physics.CheckSphere(transform.position + new Vector3(0, 1, 0), 0.62f, groundMask);
        }
        else
        {
            isGrounded = Physics.CheckSphere(transform.position + new Vector3(0, 0.5f, 0), 0.62f, groundMask);
        }



        //if player on ground , set drag to ground drag and stop applying gravity
        if (isGrounded)
        {
            verticalVecolcity.y = 0f;
            if (!sliding)
                smoothTime = groundSmoothTime;
        }
        else //if player in the air , if he hits his head stop the up velocity
        {
            if (Physics.CheckSphere(transform.position + headCollider, 0.5f, groundMask) && verticalVecolcity.y > 0)
            {
                verticalVecolcity.y = 0;
            }
            smoothTime = airSmoothTime;
        }



        //Movement
        currentInput = UnityEngine.Vector2.SmoothDamp(currentInput, horizontalInput, ref smoothInputVelocity, smoothTime);
        UnityEngine.Vector3 horizontalVelocity = (transform.right * currentInput.x + transform.forward * currentInput.y) * speed;


        // handles sprinting and FOV
        if (!moving)
        {
            sprinting = false;
            speed = normalSpeed;
        }

        // Get the current speed (velocity magnitude) of the character
        float currentSpeed = controller.velocity.magnitude;

        // Calculate the FOV based on the current speed relative to baseSpeed and maxSpeed
        float t = Mathf.InverseLerp(normalSpeed, sprintSpeed * 1.2f, currentSpeed); // Normalizes speed between 0 and 1
        float targetFOV = Mathf.Lerp(normalFov, runningFov, t);               // Interpolates between baseFOV and maxFOV

        // Smoothly transition to the target FOV using SmoothDamp
        currentFOV = Mathf.SmoothDamp(currentFOV, targetFOV, ref fovVelocity, smoothTime);

        // Apply the calculated FOV to the camera
        playerCamera.m_Lens.FieldOfView = currentFOV;



        //jumping
        if (jumpPressed)
        {
            if (isGrounded)
            {
                verticalVecolcity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
            jumpPressed = false;
        }

        if(verticalVecolcity.y >= 0 && jumpCancelled)
        {
            verticalVecolcity.y = 0.1f;
            jumpCancelled = false;
        }

        Debug.Log("jump presed "+jumpPressed);
        Debug.Log("jump cancelled " + jumpCancelled);

        //gravity

        verticalVecolcity.y += gravity * Time.deltaTime;

        //force 
        if (force.magnitude > 10)
            force = (force / (1 + Time.deltaTime * firstForceDamping));
        else if (force.magnitude > 0.1)
            force = (force / (1 + Time.deltaTime * secondForceDamping));
        else
            force = Vector3.zero;

        controller.Move((verticalVecolcity + horizontalVelocity + force) * Time.deltaTime);

        if (walknGrass != null)
        {
            if (walkAudioCooldown <= 0)
            {
                if (moving && isGrounded)
                {
                    SoundManager.instance.PlaySFXClip(walknGrass, transform, 1f);
                    walkAudioCooldown = timeBetweenSteps;
                }
            }
            else
            {
                walkAudioCooldown -= Time.deltaTime;
            }
        }


        // slwoing the sprint after time

        if (sprinting)
        {
            if (sprintTimePassed < 2)
            {
                sprintTimePassed += Time.deltaTime;
            }
            else
            {
                if (speed > normalSpeed)
                {
                    speed -= Time.deltaTime * 4;
                    Debug.Log(speed);
                }
            }
        }



    }

    public void OnSprintPressed()
    {
        if (!sprinting)
        {
            sprintTimePassed = 0;
        }
        if (!crouching && moving)
        {
            sprinting = true;
            timeBetweenSteps /= 2f;
            speed = sprintSpeed;
        }

    }
    public void OnSprintReleased()
    {
        if (!crouching)
        {
            sprinting = false;
            timeBetweenSteps *= 2f;
            speed = normalSpeed;
            Debug.Log("changed to slow");
        }
    }

    public void OnJumpPressed()
    {
        jumpPressed = true;
    }
    public void OnJumpCancelled()
    {
        jumpCancelled = true;
    }
    
    IEnumerator Sliding()
    {
        sliding = true;
        float time = 1.2f   ;
        Vector3 forceToAdd = controller.velocity.normalized * 30;
        forceToAdd.y = 0;
        force += forceToAdd;
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        smoothTime = groundSmoothTime;
        sliding = false;

    }
    public void OnCrouchPressed()
    {
        if (!crouching)
        {
            if (controller.velocity.magnitude > 17)
            {
                //smoothTime = airSmoothTime;
                StartCoroutine(Sliding());
            }



            speed = crouchSpeed;
            controller.height = 2f;
            crouching = true;
            headPosition.transform.localPosition = new Vector3(0, 1.95f, 0);
            playerShape.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (!Physics.CheckSphere(transform.position + new Vector3(0, 3.06f, 0), 0.55f))
            {
                //make size normal 
                controller.height = 3f;

                headPosition.transform.localPosition = new Vector3(0, 2.5f, 0);
                playerShape.transform.localScale = new Vector3(1, 1.5f, 1);

                //make speed normal
                speed = normalSpeed;
                //set crounching to false
                crouching = false;

                cancelCrouch = false;
            }
        }


    }

    public void OnCrouchCancelled()
    {
        

    }

    //if passed true , it changes to big fov , else to small fov
    public IEnumerator FovSprintChange(bool makeBig)
    {
        if (makeBig)
        {
            while (true)
            {
                if (playerCamera.m_Lens.FieldOfView < runningFov)
                {
                    playerCamera.m_Lens.FieldOfView += sprintFOVChangeSpeed * Time.deltaTime;
                }
                else
                {
                    playerCamera.m_Lens.FieldOfView = runningFov;
                    yield break;
                }
                yield return null;
            }
            
        }
        else
        {
            while (true)
            {
                if (playerCamera.m_Lens.FieldOfView > normalFov)
                {
                    playerCamera.m_Lens.FieldOfView -= sprintFOVChangeSpeed * Time.deltaTime;
                }
                else
                {
                    playerCamera.m_Lens.FieldOfView = normalFov;
                    yield break; ;
                }
                yield return null;
            }
            
        }
        
    }

    public void RecieveInput(UnityEngine.Vector2 _horizontalInput)
    {
        horizontalInput = _horizontalInput;
        if(horizontalInput!= UnityEngine.Vector2.zero)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
    }
}

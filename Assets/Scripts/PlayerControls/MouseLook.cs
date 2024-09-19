using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    [SerializeField] float sensitivityX = 8f;
    [SerializeField] float sensitivityY = 0.5f;
    float mouseX, mouseY;
    float xRotation = 0f;
    [SerializeField] float xClamp = 85f;

    [SerializeField] Transform playerCamera;
    [SerializeField] LayerMask uiMask;

    float pickUpDistance = 10f;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
    }
    private void Start()
    {
        
        //Cursor.visible = false;
    }

    private void Update()
    {
        //rotates both camera and player horizontally
        transform.Rotate(Vector3.up,mouseX);

        //rotates around the x "axis" 
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -xClamp,xClamp);

        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;
        playerCamera.eulerAngles = targetRotation;



        //RAYCASTING
        if(Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, pickUpDistance , uiMask))
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(hit.collider.name);
                Button button = hit.collider.gameObject.GetComponent<Button>();
                if (button != null) { 
                    button.onClick.Invoke();
                }
            }
        }
        


    }

    public void ReceiveInput(Vector2 mouseInput)
    {
        mouseX = mouseInput.x * sensitivityX;
        mouseY = mouseInput.y * sensitivityY;
    }

}

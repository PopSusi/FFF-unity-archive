using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public PlayerInput input;
    private Vector2 moveInput;
    private Vector2 lookInput;

    [SerializeField] private float moveSpeed = 1f;

    public Camera cam;
    [Range(.1f, 1f)] public float lookSens  = .3f;


    public static PlayerController instance;

    
    [SerializeField] private AudioSource audioFoot;

    

    private void Awake()
    {
        if (instance == null){
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        input.actions.FindActionMap("Base").Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempMove = new Vector3(moveInput.x, 0f, moveInput.y);
        if (tempMove != Vector3.zero)
        {
            transform.position = transform.TransformDirection(tempMove * Time.deltaTime * moveSpeed) + transform.position;
            PlayerAttacks.instance.moving = true;
            if (!audioFoot.isPlaying)
            {
                audioFoot.Play();
            }
        } else
        {
            PlayerAttacks.instance.moving = false;
            if (audioFoot.isPlaying)
            {
                audioFoot.Stop();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        if (!UIManager.instance.paused)
        {
            lookInput = context.ReadValue<Vector2>();
            transform.Rotate(Vector3.up * lookInput.x * lookSens);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;
using Unity.Cinemachine;
using TMPro.EditorUtilities;

public class PlayerController : MonoBehaviour
{
    private PlayerController _playercontroller;

    private Rigidbody _rb;

    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _FPS;
    [SerializeField] private Transform _TPS;
    public CinemachineCamera _cam;
    public CinemachineInputAxisController _axiscontroller;

    [SerializeField] private float jumpheight = 1;

    [SerializeField] private float movespeed = 1;

    private Vector3 gravity = new Vector3(0f, -9.81f, 0f);

    private Vector2 input;

    private bool isgrounded;
    

    
   
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playercontroller = new PlayerController();
        _rb = GetComponent<Rigidbody>();
        
    }
    private void FixedUpdate()
    {
        _rb.AddForce(gravity, ForceMode.Acceleration);


        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 MoveDirection = (forward * input.y + right * input.x) * movespeed;

        MoveDirection.y = _rb.linearVelocity.y;

        _rb.linearVelocity = MoveDirection;


    }

    
    private void OnChangeCameraPosition()
    {

        if (_cam.Follow == _TPS.transform)
        {
            _cam.Follow = _FPS.transform;
            _cam.GetComponent<CinemachineInputAxisController>().enabled = true;
            
           //Debug.Log("FPS");

        }
        else
        {
            _cam.GetComponent<CinemachinePanTilt>().PanAxis.Value = 0;
            _cam.GetComponent<CinemachinePanTilt>().TiltAxis.Value = 0;
            _cam.Follow = _TPS.transform;
            _cam.GetComponent<CinemachineInputAxisController>().enabled = false;
            
            //Debug.Log("TPS");
        }
    }

    private void OnMove(InputValue inputvalue)
    {
        input = inputvalue.Get<Vector2>();
    }

    private void OnJump()
    {
        if(isgrounded) 
        {
            //Debug.Log("jump");
            _rb.AddForce(Vector3.up * jumpheight, ForceMode.Impulse);
        }
        
    }
    private void OnGravity()
    {
        //Debug.Log("odwrot grawitacji");
        gravity = -gravity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor jumpable"))
        {
            //Debug.Log("jestem na ziemi");
            isgrounded = true;
        }
        else
        {
            //Debug.Log("nie moge tu skakac");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Debug.Log("w powietrzu jestem");
        isgrounded = false;
    }
    
}

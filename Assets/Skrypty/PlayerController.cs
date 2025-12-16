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

    [Header("Ustawienia kamery")]
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _FPS;
    [SerializeField] private Transform _TPS;
    public CinemachineCamera _cam;
    public CinemachineInputAxisController _axiscontroller;

    [Header("Ustawienia poruszania")]
    [SerializeField] private float jumpheight = 1;
    [SerializeField] private float movespeed = 10;

    [Header("Ustawienia BunnyHop")]
    [SerializeField] private float _bhopwindow = 0.15f;
    [SerializeField] private float _bhopboost = 10f;
    [SerializeField] private float _maxspeed = 10f;

    [Header("Ustawienia ziemi i powietrza")]
    [SerializeField] private float _groundAcceleration = 80f;
    [SerializeField] private float _airAcceleration = 20f;
    [SerializeField] private float _groundfriction = 10f;

    private Vector3 gravity = new Vector3(0f, -9.81f, 0f);

    private Vector2 input;

    private bool isgrounded;

    private float _LastGroundedTime;
    
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

        /*Vector3 MoveDirection = (forward * input.y + right * input.x) * movespeed;
        MoveDirection.y = _rb.linearVelocity.y;
        _rb.linearVelocity = MoveDirection;*/


        Vector3 _wishdir = (forward * input.y + right * input.x).normalized;

        Vector3 _desiredvelocity = _wishdir * movespeed;
        Vector3 _currentvel = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z);

        Vector3 _newvel = Vector3.MoveTowards(_currentvel, _desiredvelocity, movespeed * Time.fixedDeltaTime);

        if (_currentvel.magnitude < _maxspeed)
        {
            _rb.linearVelocity = new Vector3(_newvel.x, _rb.linearVelocity.y, _newvel.z);
        }

        if (isgrounded && _wishdir.magnitude < 0.1f)
        {
            Vector3 _friction = _currentvel * _groundfriction * Time.fixedDeltaTime;
            _currentvel -= _friction;
        }

        Vector3 _desiredvel = _wishdir * movespeed;

        float accel;
        if (isgrounded)
        {
            accel = _groundAcceleration;
        }
        else
        {
            accel = _airAcceleration;
        }

        Vector3 _newVel = Vector3.MoveTowards(_currentvel, _desiredvel, accel * Time.fixedDeltaTime);

        _rb.linearVelocity = new Vector3(_newVel.x, _rb.linearVelocity.y, _newVel.z);
    }
    
    private void OnChangeCameraPosition()
    {

        if (_cam.Follow == _TPS.transform)
        {
            _cam.Follow = _FPS.transform;
            _cam.GetComponent<CinemachineInputAxisController>().enabled = true;
            
           

        }
        else
        {
            _cam.GetComponent<CinemachinePanTilt>().PanAxis.Value = 0;
            _cam.GetComponent<CinemachinePanTilt>().TiltAxis.Value = 0;
            _cam.Follow = _TPS.transform;
            _cam.GetComponent<CinemachineInputAxisController>().enabled = false;
            
            
        }
    }

    private void OnMove(InputValue inputvalue)
    {
        input = inputvalue.Get<Vector2>();
    }

    private void OnJump()
    {
        bool _bhopsucc = (Time.time - _LastGroundedTime) <= _bhopwindow;
        if (isgrounded) 
        {
            _rb.AddForce(Vector3.up * jumpheight, ForceMode.Impulse);
        }
        if(_bhopsucc)
        {
            _rb.AddForce(Vector3.forward * _bhopboost, ForceMode.Acceleration);
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
            isgrounded = true;
            _LastGroundedTime = Time.time;
        }
        
    }

    private void OnCollisionExit(Collision collision)
    {
        isgrounded = false;
    }
    
}

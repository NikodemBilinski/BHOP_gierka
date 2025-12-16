using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Player_UI : MonoBehaviour
{

    protected float _speed;

    private Rigidbody _rb;
    [SerializeField] private TextMeshProUGUI _Text;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //_Text = 
    }
    private void FixedUpdate()
    {
        _speed = _rb.linearVelocity.magnitude;
        _Text.text = _speed.ToString();

    }


}

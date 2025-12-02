using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    

    Vector3 _starting_position;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       _starting_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Lose"))
        {
            transform.position = _starting_position;
        }
    }
}

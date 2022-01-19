using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float MoveRight = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float MoveForward = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(MoveRight, 0.0f, MoveForward);
    }
}

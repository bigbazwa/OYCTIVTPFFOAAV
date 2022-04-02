using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalAxis { get; private set; }

    public float VerticalAxis { get; private set; }

    public bool Dig { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.HorizontalAxis = Input.GetAxisRaw("Horizontal");
        this.VerticalAxis = Input.GetAxisRaw("Vertical");

        this.Dig = Input.GetButtonDown("Jump");
    }
}

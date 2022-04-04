using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalAxis { get; private set; }

    public float VerticalAxis { get; private set; }

    public bool Dig { get; private set; }

    public bool Jump { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.HorizontalAxis = Input.GetAxisRaw("Horizontal");
        this.VerticalAxis = Input.GetAxisRaw("Vertical");

        this.Dig = Input.GetButton("Fire1") || Input.GetMouseButton(0);
        this.Jump = Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(1);

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioListener.pause = !AudioListener.pause;
        }
    }
}

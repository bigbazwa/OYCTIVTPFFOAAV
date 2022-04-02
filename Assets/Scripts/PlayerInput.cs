using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float HorizontalAxis { get; set; }

    public float VerticalAxis { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.HorizontalAxis = Input.GetAxisRaw("Horizontal");
        this.VerticalAxis = Input.GetAxisRaw("Vertical");
    }
}

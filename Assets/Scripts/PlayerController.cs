using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    private PlayerInput input;

    public float walkSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.characterController = GetComponent<CharacterController>();
        this.input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        float walkAmount = walkSpeed * Time.deltaTime;
        this.characterController.Move(new Vector3(input.HorizontalAxis * walkAmount, -9.81f * Time.deltaTime, input.VerticalAxis * walkAmount));
        //this.characterController.SimpleMove(new Vector3(input.HorizontalAxis * walkSpeed, 0.0f, input.VerticalAxis * walkSpeed));
    }
}
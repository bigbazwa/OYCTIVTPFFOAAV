using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    private PlayerInput input;

    private Volcano volcano;

    private float yVelocity = 0.0f;

    public float walkSpeed = 5.0f;

    public float jumpImpulse = 0.75f;

    public Transform mesh;

    public delegate void OnHurt();
    public static event OnHurt onHurt;

    // Start is called before the first frame update
    void Start()
    {
        this.characterController = GetComponent<CharacterController>();
        this.input = GetComponent<PlayerInput>();
        this.volcano = GameObject.FindObjectOfType<Volcano>();

        PlayerHealth.onDead += PlayerHealth_onDead;
    }

    private void PlayerHealth_onDead()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        float walkAmount = walkSpeed * Time.deltaTime;
        Vector3 movementVector = new Vector3(input.HorizontalAxis * walkAmount, 0f, input.VerticalAxis * walkAmount);

        if (movementVector.magnitude > 0.0f)
        {
            this.mesh.LookAt(this.transform.position + movementVector);
        }

        Vector3 wantPosition = this.transform.position + movementVector;

        if (Physics.Raycast(wantPosition, Vector3.down, out RaycastHit raycastHit, 25f))
        {
            if (raycastHit.collider.GetComponent<VolcanoTile>() is VolcanoTile volcanoTile)
            {

            }
            else
            {
                movementVector = Vector3.zero;
            }
        }
        else
        {
            movementVector = Vector3.zero;
        }

        bool isGrounded = false;

        if (this.yVelocity <= 0.0f && Physics.Raycast(this.transform.position, Vector3.down, out raycastHit, 2.5f - this.mesh.forward.z))
        {
            isGrounded = true;

            if (raycastHit.collider.GetComponent<VolcanoTile>() is VolcanoTile volcanoTile)
            {
                if (volcanoTile.LavaLevel > 0.0f)
                {
                    isGrounded = false;
                    this.yVelocity = this.jumpImpulse;

                    onHurt?.Invoke();
                }
            }
        }

        if (isGrounded)
        {
            this.yVelocity = -9.0f;

            if (this.input.Jump)
            {
                this.yVelocity = jumpImpulse;
            }
        }


        if (yVelocity > 0.0f)
        {
            this.yVelocity += -1.81f * Time.deltaTime;
        }
        else
        {
            this.yVelocity += -5.81f * Time.deltaTime;
        }
        

        movementVector.y = this.yVelocity;

        this.characterController.Move(movementVector);
        //this.characterController.SimpleMove(new Vector3(input.HorizontalAxis * walkSpeed, 0.0f, input.VerticalAxis * walkSpeed));

        if (input.Dig)
        {
            Vector3 facingVector = this.mesh.forward;
            facingVector.y = facingVector.z;
            if (this.volcano.Dig(this.transform.position + facingVector * 1.5f, 2.5f, 0.25f))
            {
                // If successful dig, put dirt behind.
                this.volcano.Dig(this.transform.position - facingVector * 1.5f, 2.0f, -0.1f);
            }
        }
    }
}

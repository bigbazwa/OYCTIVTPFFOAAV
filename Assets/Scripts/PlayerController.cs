using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    private PlayerInput input;

    private Volcano volcano;

    private float yVelocity = 0.0f;

    public float walkSpeed = 5.0f;

    public float jumpImpulse = 0.75f;

    public float digCooldown = 0.5f;

    private float digCooldownTimer = 0.0f;

    public Transform mesh;

    public Animator animator;

    public ParticleSystem dirtParticles;

    public delegate void OnHurt();
    public static event OnHurt onHurt;

    public GameObject[] ScoreTexts;

    public GameObject[] FinalScoreTexts;

    public AudioClip jumpSFX;

    public AudioClip digSFX;

    public AudioClip hurtSFX;

    public AudioSource audioSource;

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
        int finalScore = this.volcano.score;

        this.ScoreTexts.ToList().ForEach(x => x.SetActive(false));
        this.FinalScoreTexts.ToList().ForEach(x => x.SetActive(true));
        this.FinalScoreTexts[1].GetComponent<Text>().text = $"{finalScore}";

        if (finalScore > PlayerPrefs.GetInt($"HS{this.volcano.LevelId}", 0))
        {
            PlayerPrefs.SetInt($"HS{this.volcano.LevelId}", finalScore);
        }

        GameObject.FindObjectOfType<LoseSequence>()?.Begin();
        this.GetComponentsInChildren<Renderer>().Where(x => x.enabled).FirstOrDefault(x => x.enabled = false);
        Destroy(this.gameObject, 3f);
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
            //if (raycastHit.collider.GetComponent<VolcanoTile>() is VolcanoTile volcanoTile)
            if (raycastHit.collider.tag.Equals("Valid"))
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

        this.animator.SetFloat("Movement", movementVector.magnitude);

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
                this.audioSource.PlayOneShot(this.jumpSFX);
            }
        }

        this.animator.SetBool("Falling", !isGrounded);


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

        this.digCooldownTimer -= Time.deltaTime;

        if (this.digCooldownTimer <= 0.0f)
        {
            this.digCooldownTimer = 0.0f;
        }

        if (input.Dig && this.digCooldownTimer <= 0.0f)
        {
            Vector3 facingVector = this.mesh.forward;
            facingVector.y = facingVector.z;
            this.digCooldownTimer = this.digCooldown;
            this.animator.SetTrigger("Dig");
            this.audioSource.PlayOneShot(this.digSFX);
            if (this.volcano.Dig(this.transform.position + facingVector * 1.5f, 2.5f, 0.25f))
            {
                this.dirtParticles.Play();
                // If successful dig, put dirt behind.
                this.volcano.Dig(this.transform.position - facingVector * 1.5f, 2.0f, -0.1f);
            }
        }
    }

    public void OnHitByBoulder()
    {
        this.yVelocity = this.jumpImpulse / 2.0f;
        onHurt?.Invoke();
    }
}

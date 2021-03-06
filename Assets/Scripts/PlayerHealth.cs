using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float hurtCooldownTimer = 0.0f;

    public float hurtCooldown = 1.0f;

    public int StartingHealth = 3;

    public int Health { get; private set; } = 3;

    public GameObject[] healthHearts;

    public delegate void OnDead();
    public event OnDead onDead;

    // Start is called before the first frame update
    void Start()
    {
        this.Health = this.StartingHealth;
        this.GetComponent<PlayerController>().onHurt += OnHurt;
        //PlayerController.onHurt += OnHurt;
    }

    // Update is called once per frame
    void Update()
    {
        this.hurtCooldownTimer -= Time.deltaTime;

        if (hurtCooldownTimer < 0.0f)
        {
            this.hurtCooldownTimer = 0.0f;
        }
    }

    private void OnHurt()
    {
        

        if (this.hurtCooldownTimer > 0.0f)
        {
            // Not hurt.
            return;
        }

        var pc = this?.GetComponent<PlayerController>();
        pc?.audioSource.PlayOneShot(pc.hurtSFX);

        this.hurtCooldownTimer = this.hurtCooldown;

        this.Health -= 1;

        if (this.Health <= 0)
        {
            this.Health = 0;
            onDead?.Invoke();
        }

        for (int i = 0; i < this.StartingHealth; i++)
        {
            this.healthHearts[i].SetActive(i < this.Health);
        }
    }
}

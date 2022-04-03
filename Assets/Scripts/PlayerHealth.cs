using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float hurtCooldownTimer = 0.0f;

    public float hurtCooldown = 1.0f;

    public int StartingHealth = 3;

    public int Health { get; private set; } = 3;

    public delegate void OnDead();
    public static event OnDead onDead;

    // Start is called before the first frame update
    void Start()
    {
        this.Health = this.StartingHealth;
        PlayerController.onHurt += OnHurt;
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

        this.hurtCooldownTimer = this.hurtCooldown;

        this.Health -= 1;

        if (this.Health <= 0)
        {
            onDead?.Invoke();
        }
    }
}

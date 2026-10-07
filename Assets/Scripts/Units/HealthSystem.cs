using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    [SerializeField] private float healthMax = 100;
    [SerializeField] private float health;

    private void Awake()
    {
        health = healthMax;
    }

    public float GetHealthNormalized()
    {
        return health / healthMax;
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;

        if(health < 0) health = 0;

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health == 0) Die();
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}

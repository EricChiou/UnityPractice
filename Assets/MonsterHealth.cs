using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{

    public float health = 20f;

    private HealthBarController healthBarPoint;
    private bool canBeAttack = true;
    private float currentHealth;

    // Use this for initialization
    void Start()
    {
        currentHealth = health;
        healthBarPoint = GetComponentInChildren<HealthBarController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void beAttacked(float power)
    {
        if (canBeAttack)
        {
            currentHealth -= power;
            healthBarPoint.countHealth(currentHealth / health);
        }
    }

    public void setCanBeAttack(bool state)
    {
        canBeAttack = state;
    }

    public float getHealth()
    {
        return currentHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float health = 10f;

    private float currentHealth = 0f;

    // Use this for initialization
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void beAttacked(float str)
    {
        currentHealth = currentHealth - str;
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowUI : MonoBehaviour
{

    private Text bulletAmount;
    private Text totalAmmo;
    private Text health;
    private SoldierMotion player;
    private PlayerHealth playerHealth;

    // Use this for initialization
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Controller").GetComponent<PlayerHealth>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<SoldierMotion>();
        bulletAmount = transform.FindChild("Ammo").FindChild("Amount").GetComponent<Text>();
        totalAmmo = transform.FindChild("Ammo").FindChild("Total").GetComponent<Text>();
        health = transform.FindChild("HealthPoint").FindChild("Health").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletAmount.text = player.getCurrentBullet().ToString();
        totalAmmo.text = player.totalAmmo.ToString();
        health.text = playerHealth.getCurrentHealth().ToString();
    }
}

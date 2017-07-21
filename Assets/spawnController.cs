using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnController : MonoBehaviour
{

    public GameObject monster;
    public int amount = 10;
    public float range = 10f;
    public bool keepSpawn = false;
    public float spawnTime = 10f;

    private float startTime;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject newMonster = GameObject.Instantiate(monster);
            newMonster.transform.position = new Vector3(transform.position.x + Random.Range(range / 2 * -1f, range / 2), transform.position.y, transform.position.z + Random.Range(range / 2 * -1f, range / 2));
        }
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (keepSpawn && (Time.time - startTime) > spawnTime)
        {
            GameObject newMonster = GameObject.Instantiate(monster);
            newMonster.transform.position = new Vector3(transform.position.x + Random.Range(range / 2 * -1f, range / 2), transform.position.y, transform.position.z + Random.Range(range / 2 * -1f, range / 2));
            startTime = Time.time;
        }
    }
}

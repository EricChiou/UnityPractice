using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToPlayer : MonoBehaviour
{
    private GameObject player;
    private float startPosition;
    private bool down;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player") as GameObject;
        startPosition = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);
        transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
        if (transform.localPosition.y > startPosition) { down = true; }
        if (transform.localPosition.y < (startPosition - 1.5f)) { down = false; }
        if (down)
        {
            transform.localPosition = new Vector3(0f, transform.localPosition.y - (2 * Time.deltaTime), 0f);
        }
        else
        {
            transform.localPosition = new Vector3(0f, transform.localPosition.y + (2 * Time.deltaTime), 0f);
        }
    }
}

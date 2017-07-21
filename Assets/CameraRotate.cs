using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    public Transform lookPos;
    public float x = 0f;
    public float y = 0f;
    public float xSpeed = 200f;
    public float ySpeed = 200f;
    public float yMax = 40f;
    public float yMin = -40f;
    public float distance = 0.7f;
    public float disMin = 0.5f;
    public float disMax = 0.8f;

    private Quaternion rotationEuler;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        if (x > 360 || x < -360) { x = 0; }
        y = Mathf.Clamp(y, yMin, yMax);
        rotationEuler = Quaternion.Euler(y, x, 0);
        transform.rotation = rotationEuler;

        distance -= Input.GetAxis("Mouse ScrollWheel") * 100f * Time.deltaTime;
        distance = Mathf.Clamp(distance, disMin, disMax);

        transform.position = lookPos.transform.position;
        transform.position += transform.forward * -1f * distance;
        transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
    }
}

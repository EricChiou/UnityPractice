using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void countHealth(float percent)
    {
        transform.localPosition = new Vector3(1188f - (1188f * percent), 0f, 0f);
        if (percent <= 0.33f)
        {
            transform.GetComponent<UnityEngine.UI.Image>().color = new Color32(225, 0, 0, 100);
        }
        else if (percent <= 0.5f)
        {
            transform.GetComponent<UnityEngine.UI.Image>().color = new Color32(225, 115, 21, 100);
        }
    }
}

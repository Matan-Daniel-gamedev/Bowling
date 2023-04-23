using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    AudioSource source;
    Rigidbody rb;
    public float power;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.z == -7)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                rb.AddForce(Vector3.forward * power);
                source.Play();
            }
        }
    }
}

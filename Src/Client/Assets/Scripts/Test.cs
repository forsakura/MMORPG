using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }
}

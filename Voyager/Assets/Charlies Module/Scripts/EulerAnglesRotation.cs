using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EulerAnglesRotation : MonoBehaviour
{
    public Vector3 angles;
    public Quaternion randomRotation, rotation;

    public float speed = 30f,
                 time = 10f;

	// Use this for initialization
	void Start () {
        StartCoroutine(RotateObject());
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.Rotate(rotation.eulerAngles, speed * Time.deltaTime);
	}

    IEnumerator RotateObject()
    {
        randomRotation = Random.rotation;
        angles = randomRotation.eulerAngles;
        rotation = Quaternion.Euler(angles);

        yield return new WaitForSeconds(time);

        StartCoroutine(RotateObject());
    }
}

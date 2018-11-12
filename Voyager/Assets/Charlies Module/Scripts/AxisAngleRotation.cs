using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAngleRotation : MonoBehaviour
{
    public Vector3 euler, axis;
    public float angle;
    public Quaternion rotation;
    public float speed = 15f,
                 time = 10f;

	// Use this for initialization
	void Start () {
        StartCoroutine(RotateAxisAngle());
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(rotation.eulerAngles, speed * Time.deltaTime);
	}

    IEnumerator RotateAxisAngle()
    {
        rotation.eulerAngles = euler;
        rotation.ToAngleAxis(out angle, out axis);
        rotation = Quaternion.AngleAxis(angle, axis);

        yield return new WaitForSeconds(time);
        StartCoroutine(RotateAxisAngle());
    }
}

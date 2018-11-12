using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitColliderToChildren : MonoBehaviour {

    // Since the player is just an empty game object with some other object as a child, this function
    // will set the parent's box collider to that of the child object as if the child were its own game object
    // The reason I'm doing this is because Blender objects exported as .FBX files will have their Y and Z axes
    // switched around when imported to Unity; importing a .BLEND file directly may eliminate the need to do this
    // but if the player is composed of multiple objects, then the box collider will adjust to fit to enclose
    // every child object; therefore, it's a good idea to use this script, even if there's only one child
    // Source: https://answers.unity.com/questions/22019/auto-sizing-primitive-collider-based-on-child-mesh.html
    // If needed, this could be made into its own script

    private void Start()
    {
        SetBoxCollider(gameObject);
    }

    public void SetBoxCollider(GameObject parentObject)
    {
        BoxCollider bc = parentObject.GetComponent<BoxCollider>();

        if (bc == null)
            bc = parentObject.AddComponent<BoxCollider>();

        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasBounds = false;

        Renderer[] renderers = parentObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer render in renderers)
        {
            if (hasBounds)
            {
                bounds.Encapsulate(render.bounds);
            }
            else
            {
                bounds = render.bounds;
                hasBounds = true;
            }
        }
        if (hasBounds)
        {
            bc.center = bounds.center - parentObject.transform.position;
            bc.size = bounds.size;
        }
        else
        {
            bc.size = bc.center = Vector3.zero;
            bc.size = Vector3.zero;
        }
    }
}

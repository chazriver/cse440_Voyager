using System.Collections;
using UnityEngine;

// The script to attach to the Ghost prefab
 public class FollowPlayer : MonoBehaviour
 {
     private Transform target = null ;
     private float speed = 0 ;

     void Update()
     {
         if( target != null )
             transform.Translate((target.position - transform.position).normalized * speed * Time.deltaTime ) ;
     }

     public void SetTarget(GameObject newTarget, float chaseSpeed )
     {
         target = newTarget.transform;
         speed = chaseSpeed;
     }

 }

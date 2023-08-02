using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logtest : MonoBehaviour
{
   private void OnCollisionEnter(Collision collision)
   {
      if (collision.transform.CompareTag("Enemy"))
      {
         Debug.Log("collision");
      }
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Enemy"))
      {
         Debug.Log("trigger");
      }
   }
}

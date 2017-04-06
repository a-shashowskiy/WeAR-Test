using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.SceneUtils
{
    public class MinionEnemy : MonoBehaviour
    {

        //The Minion current health point total
        public int currentHealth = 3;

        public void Damage(int damageAmount)
        {
            //subtract damage amount when Damage function is called
            currentHealth -= damageAmount;

            //Check if health has fallen below zero
            if (currentHealth <= 0)
            {
                //if health has fallen below zero, deactivate it 
                Invoke ("Dead", 0.01f); 
                Debug.Log("Minion is Dead");              
            }
        }
        void Dead() { gameObject.SetActive(false); }
        void OnDisable() { CancelInvoke(); }
        void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Bullet collisin with Minion");
            if (collision.gameObject.tag == "Bullet") {
                Damage(25);              
            }

            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
        }
        public void OnObjectReuse()
        {
            transform.position.Set(Random.Range(-40, 40), 0.22f, Random.Range(-40, 40));
        }
    }
}
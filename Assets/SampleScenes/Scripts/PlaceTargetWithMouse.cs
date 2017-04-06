using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UnityStandardAssets.SceneUtils
{
    public class PlaceTargetWithMouse : MonoBehaviour
    {
        public int gunDamage = 1;                                           // Set the number of hitpoints that this gun will take away from shot objects with a health script
        public float fireRate = 0.25f;                                      // Number in seconds which controls how often the player can fire
        public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
        public float hitForce = 100f;                                       // Amount of force which will be added to objects with a rigidbody shot by the player
        public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun

        private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
        private WaitForSeconds timeRenuw = new WaitForSeconds(5f);
        private AudioSource gunAudio;                                       // Reference to the audio source which will play our shooting sound effect
        private float nextFire;                                             // Float to store the time the player will be allowed to fire again, after firing
        float temps;
        public float bulletSpeed = 10;
        public GameObject bulletPrefab;
        public float surfaceOffset = 1.5f;
        public GameObject setTargetOn;
        public GameObject MinionPrefab;
        public int PoolQuant = 5;
        GameObject[] enemyOnScen;

        void Start()
        {
            // Get and store a reference to our AudioSource component
            gunAudio = GetComponent<AudioSource>();
            PoolManager.instance.CreatePool(MinionPrefab, PoolQuant);
            for (int i = 0; i < PoolQuant; i++)
            {
                PoolManager.instance.ReuseObject(MinionPrefab, new Vector3(UnityEngine.Random.Range(-10, 10), 0.22f, UnityEngine.Random.Range(-10, 10)), Quaternion.identity);
            }
            if (enemyOnScen == null)
                enemyOnScen = GameObject.FindGameObjectsWithTag("Enemy");
           
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                temps = Time.time; 
            }
            if (Input.GetMouseButtonUp(0) && (Time.time - temps) < 0.2 && Time.time > nextFire)
            {
                // Update the time when our player can fire next
                nextFire = Time.time + fireRate;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit))
                {
                    return;
                }
                setTargetOn.transform.LookAt (hit.point + hit.normal) ;
                setTargetOn.GetComponent<Animator>().SetBool("Shoot",true);
                // Start our ShotEffect coroutine
                StartCoroutine(ShotEffect());
                
                if (setTargetOn != null)
                {
                    setTargetOn.SendMessage("SetTarget", transform);
                 }
            }

            if (Input.GetMouseButtonUp(0) && (Time.time - temps) > 0.2)
            {
                // Long Click
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (!Physics.Raycast(ray, out hit))
                {
                    return;
                }
                transform.position = hit.point + hit.normal * surfaceOffset;
                if (setTargetOn != null)
                {
                    setTargetOn.SendMessage("SetTarget", transform);
                }
            }
            
            StartCoroutine(RenewEnemy());
                
           
        }
        void Fire()
        {
            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                gunEnd.position,
                gunEnd.rotation);
            bullet.tag = "Bullet";
            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.TransformDirection(new Vector3(0, bulletSpeed, 0));

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 5.0f);
        }
        private IEnumerator RenewEnemy()
        {
            int count = 0;
            for (int i = 0; i < enemyOnScen.Length; i++)
            {

                if (enemyOnScen[i].activeSelf == false)
                {
                    count++;
                }
                   if(count == PoolQuant) { 
                    Debug.Log("{0}", enemyOnScen[i]);
                    Debug.Log("Start Coroutine RenewEnemy()");
                    for (int j = 0; j< PoolQuant; j++)
                    {
                        PoolManager.instance.ReuseObject(MinionPrefab, new Vector3(UnityEngine.Random.Range(-10, 10), 0.22f, UnityEngine.Random.Range(-10, 10)), Quaternion.identity);
                    }
                }
            }
            yield return timeRenuw;
        }
        private IEnumerator ShotEffect()
        {
            // Play the shooting sound effect
            gunAudio.Play();
            Fire();
            setTargetOn.GetComponent<Animator>().SetBool("Shoot", false);
            //Wait for .07 seconds
            yield return shotDuration;

        }
    }

}

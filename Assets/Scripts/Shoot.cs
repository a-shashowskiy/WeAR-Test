using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    public int gunDamage = 1;                                           // Set the number of hitpoints that this gun will take away from shot objects with a health script
    public float fireRate = 0.25f;                                      // Number in seconds which controls how often the player can fire
    public float weaponRange = 50f;                                     // Distance in Unity units over which the player can fire
    public float hitForce = 100f;                                       // Amount of force which will be added to objects with a rigidbody shot by the player
    public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun

    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
    private AudioSource gunAudio;                                       // Reference to the audio source which will play our shooting sound effect
    private float nextFire;                                             // Float to store the time the player will be allowed to fire again, after firing
    float temps;
    public float bulletSpeed = 10;
    public GameObject bulletPrefab;
   

    void Start()
    {
        // Get and store a reference to our AudioSource component
        gunAudio = GetComponent<AudioSource>();

    }


    void Update()
    {
        // Check if the player has pressed the fire button and if enough time has elapsed since they last fired
        if (Input.GetMouseButtonDown(0))
        {
            temps = Time.time;
        }
        if (Input.GetMouseButtonUp(0) && (Time.time - temps) < 0.2 && Time.time > nextFire)
        {
            // Update the time when our player can fire next
            nextFire = Time.time + fireRate;

            // Start our ShotEffect coroutine to turn our laser line on and off
            StartCoroutine(ShotEffect());
        }
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
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.TransformDirection(new Vector3(0, 0, bulletSpeed));

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 5.0f);
    }

    private IEnumerator ShotEffect()
    {
        // Play the shooting sound effect
        gunAudio.Play();
        Fire();

        //Wait for .07 seconds
        yield return shotDuration;

    }
}

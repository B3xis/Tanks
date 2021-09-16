using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using UnityEditor;

public class Bufs : MonoBehaviour
{
   
    ShellExplosion shellExplosion;
   
    public bool on, medOn, speedOn;
    TankHealth tankHealth;
    TankShooting tankShooting;
    TankMovement tankMovement;
    SpawnBufs spawnBufs;
    public GameObject spikes, artOb;
   
   
    // Start is called before the first frame update
    void Start()
    {
     
        shellExplosion = Resources.Load<ShellExplosion>("Shell");
        tankHealth = Resources.Load<TankHealth>("Tank");
    
        
    }

   
 
  
   
  
    private void RandBuf() // Generating buf
    {
        Transform targetRigidbody1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        tankHealth = targetRigidbody1.GetComponent<TankHealth>();
        tankShooting = targetRigidbody1.GetComponent<TankShooting>();
        tankMovement = targetRigidbody1.GetComponent<TankMovement>();
        int debuf =  Random.Range(0,15);
        switch (debuf)
        {
            case 1:
                {
                   
                    Debug.Log("Med");
                    medOn = true;

                    break;
                }
            case 2:
                {
                  
                    Debug.Log("Speed");
                    speedOn = true;
                    break;
                }
            case 3:
                {
                    this.tag = "Shield";
                   
                    Debug.Log("Shield");
                  
                    break;
                }
            case 4:
                {
                    GunX2Buf();
                    tankShooting.gunX2On = true;
                    Debug.Log("Gunx2");
                    tankHealth.bufOn = true;
                    break;
                }
            case 5:
                {
                    this.tag = "Minigun";
                    Debug.Log("Minigun");
                    tankShooting.minigunOn = true;
                    tankHealth.bufOn = true;
                    break;
                }
            case 6:
                {
                    this.tag = "Lazer";
                    Debug.Log("Lazer");
                    tankShooting.lazerOn = true;
                    tankHealth.bufOn = true;
                    break;
                }
            case 7:
                {
                    this.tag = "Shotgun";
                    Debug.Log("Shotgun");
                    tankShooting.shotgunOn = true;
                    tankHealth.bufOn = true;
                    break;
                }
            case 8:
                {
                    this.tag = "Ise";
                    Debug.Log("Ise");
                    tankHealth.iseOn = true;
                    tankHealth.bufOn = true;
                    break;
                }
            case 9:
                {
                    this.tag = "Fire";
                    Debug.Log("Fire");
                    tankHealth.fireOn = true;
                    tankHealth.bufOn = true;
                    break;
                }
            case 10:
                {
                    tankHealth.IseDamage();
                   
                    break;
                }
            case 11:
                {
                    tankHealth.FireDamage();
                    
              
                    break; 
                }
            case 12:
                {
                   tankMovement.chainOn = true;

                    break;
                }
            case 13:
                {
                    Transform targetRigidbody = GameObject.FindGameObjectWithTag("GameController").GetComponent<Transform>();
                    
                    SpawnBufs spawnBufs = targetRigidbody.GetComponent<SpawnBufs>();
                    for (int i = 0; i < 5; i++)
                    {
                        spawnBufs.Spawn();
                        PhotonNetwork.Instantiate(spikes.name, spawnBufs.pos, Quaternion.identity);
                    }
                    

                    break;
                }
            case 14:
                {
                    Transform targetRigidbody = GameObject.FindGameObjectWithTag("GameController").GetComponent<Transform>();

                    SpawnBufs spawnBufs = targetRigidbody.GetComponent<SpawnBufs>();
                    for (int i = 0; i < 5; i++)
                    {
                        spawnBufs.Spawn();
                        PhotonNetwork.Instantiate(artOb.name, spawnBufs.pos = new Vector3(spawnBufs.pos.x, spawnBufs.pos.y = 8f, spawnBufs.pos.z), Quaternion.Euler(-90, 0, 0));
                    }
                    

                    break;
                }


            default:
                Debug.Log("Error Debufs");
                break;
        }
        GameObject.Destroy(gameObject);
    }
  
    private void GunX2Buf()
    {
        
            shellExplosion.m_MaxDamage = 200f;


        GameObject.Destroy(gameObject);

    }
  
   
    
    private void OnTriggerEnter(Collider hit)
    {
        Transform targetRigidbody1 = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        tankHealth = targetRigidbody1.GetComponent<TankHealth>();
        if (hit.tag == "Player")
        {
            if(this.tag == "Rand")
            {
                RandBuf();
            }
            else if(this.tag == "Gunx2")
            {
                GunX2Buf();
                tankHealth.bufOn = true;
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
            
        }
    }
   
}

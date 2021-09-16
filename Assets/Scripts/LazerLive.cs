using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class LazerLive : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 0.5f);
    }
    private void OnTriggerEnter(Collider hit)
    {
        Rigidbody targetRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        TankShooting tankShooting = targetRigidbody.GetComponent<TankShooting>();
        Debug.Log(hit.tag);
           if(hit.tag == "Player")
        {
            tankShooting.lazerOn = false;
            DestroyLazer();
        }
           
        
    }
   
    public void DestroyLazer()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}

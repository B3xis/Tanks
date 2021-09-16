using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBufs : MonoBehaviour, IPunObservable
{
    public int timer;
    public List<GameObject> buf = new List<GameObject>();
    public Vector3 pos;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(timer);
        }
        else
        {
            timer = (int)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += 10; 
        if(timer >= 5000)
        {
            Spawn();
            timer = 0;
        }
    }
    public void Spawn() // Spawning bufs on position
    {
        int randPos = Random.Range(1, 20);
        switch (randPos)
        {
            case 1:
                {
                    pos = new Vector3(-25.94f,1,16.5f);
                    break;
                }
            case 2:
                {
                    pos = new Vector3(-25.94f, 1, -16.5f);
                    break;
                }
            case 3:
                {
                   pos = new Vector3(-13.35f, 1, 16.5f);
                    break;
                }
            case 4:
                {
                   pos = new Vector3(-13.35f, 1, -16.5f);
                    break;
                }
            case 5:
                {
                   pos = new Vector3(-20.11f, 1, -5.97f);
                    break;
                }
            case 6:
                {
                   pos = new Vector3(-20.11f, 1, 5.97f);
                    break;
                }
            case 7:
                {
                   pos = new Vector3(-11.37f, 1, 0f);
                    break;
                }
            case 8:
                {
                   pos = new Vector3(-5.3f, 1, 0f);
                    break;
                }
            case 9:
                {
                   pos = new Vector3(-5.3f, 1, 16.5f);
                    break;
                }
            case 10:
                {
                   pos = new Vector3(-5.3f, 1, -16.5f);
                    break;
                }
            case 11:
                {
                   pos = new Vector3(25.94f, 1, 16.5f);
                    break;
                }
            case 12:
                {
                   pos = new Vector3(25.94f, 1, -16.5f);
                    break;
                }
            case 13:
                {
                   pos = new Vector3(13.35f, 1, 16.5f);
                    break;
                }
            case 14:
                {
                   pos = new Vector3(13.35f, 1, -16.5f);
                    break;
                }
            case 15:
                {
                   pos = new Vector3(20.11f, 1, -5.97f);
                    break;
                }
            case 16:
                {
                   pos = new Vector3(20.11f, 1, 5.97f);
                    break;
                }
            case 17:
                {
                   pos = new Vector3(11.37f, 1,0f);
                    break;
                }
            case 18:
                {
                   pos = new Vector3(5.3f, 1, 0f);
                    break;
                }
            case 19:
                {
                   pos = new Vector3(5.3f, 1, -16.5f);
                    break;
                }
            case 20:
                {
                   pos = new Vector3(-5.3f, 1, 16.5f);
                    break;
                }

            default:
                break;
        }

        int who = Random.Range(0, 10);
        PhotonNetwork.Instantiate(buf[who].name, pos, Quaternion.Euler(buf[who].transform.rotation.x, 0, 0));
       
    }
}

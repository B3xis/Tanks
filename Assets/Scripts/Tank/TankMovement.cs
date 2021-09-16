using Photon.Pun;
using UnityEngine;

public class TankMovement : MonoBehaviourPun, IPunObservable
{
    public int m_PlayerNumber = 1;               
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;
    public Transform m_CameraRigTransform;
    public GameObject m_FixedJoystickObject, speedPartl, chainPartl;


    public float m_Speed = 6f;
    private float m_TurnSpeed = 180f;
    private Rigidbody m_Rigidbody;         
    private float m_MovementVerticalValue;    
    private float m_MovementHorizontalValue;        
    private float m_OriginalPitch;
    public Vector3 m_NetworkPos;     
    public Quaternion m_NetworkRot;
    private FixedJoystick m_FixedJoystick;

    public bool onSpeed, iseOn, chainOn;
   
  
    private int timerSpeed, timerChain;

    Bufs bufs;
    TankHealth tankHealth;

    private void Awake()
    {
        
        m_Rigidbody = GetComponent<Rigidbody>();
        m_FixedJoystick = m_FixedJoystickObject.GetComponent<FixedJoystick>();

        m_FixedJoystickObject.SetActive(photonView.IsMine);
        m_NetworkPos = transform.position;
        m_NetworkRot = transform.rotation;
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementVerticalValue = 0f;
        m_MovementHorizontalValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_OriginalPitch = m_MovementAudio.pitch;
        m_Speed = 6f;
        Collider cube = GameObject.FindGameObjectWithTag("Finish").GetComponent<Collider>();

        bufs = cube.GetComponent<Bufs>();
    }

    private void Update()
    {
        EngineAudio();
    }
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == "Speed") 
        {
             onSpeed = true;
             speedPartl.gameObject.SetActive(true);
        }
        
        
       
    }
        private void EngineAudio()
        {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.

        if (Mathf.Abs(m_MovementVerticalValue) < 0.1f && Mathf.Abs(m_MovementHorizontalValue) < 0.1f)
        {
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else 
        {
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        
        if (!photonView.IsMine)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, m_NetworkPos, Time.fixedDeltaTime * m_Speed);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, m_NetworkRot, Time.fixedDeltaTime * m_TurnSpeed);
        }
        else
        {
            Vector3 movement = m_CameraRigTransform.forward * m_FixedJoystick.Vertical + m_CameraRigTransform.right * m_FixedJoystick.Horizontal;
            movement.y = 0f;

            if (movement != Vector3.zero)
            {
                this.transform.position += movement * m_Speed * Time.fixedDeltaTime;
                this.transform.forward = movement * m_TurnSpeed * Time.fixedDeltaTime;
            }
        }
        if(onSpeed == true || bufs.speedOn == true) // If triggered with speed buf 
        {
            timerSpeed += 10;
            

                m_Speed = 12f;
                if (timerSpeed >= 3000)
                {
                    m_Speed = 6f;
                    onSpeed = false;
                    timerSpeed = 0;
                    bufs.speedOn = false;
                    Debug.Log(m_Speed);
                    speedPartl.gameObject.SetActive(false);
                }
            
        }
        else if (chainOn == true) // If triggered with chains
        {
            
            chainPartl.gameObject.SetActive(true);
            ChainTriggered();
        }


    }

    public void IseOnDamage()
    {
        m_Speed = 2f;
    }
    public void IseOffDamage()
    {
        m_Speed = 6f;
    }
    void ChainTriggered()
    {
        
        m_Speed = 0;
        timerChain++;
        Invoke("ChainTriggered", 0.3f);
        if (timerChain >= 10)
        {   
            chainPartl.gameObject.SetActive(false);
            timerChain = 0;
            CancelInvoke("ChainTriggered");
            chainOn = false;
        }
        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
            stream.SendNext(speedPartl.activeSelf);
            stream.SendNext(chainPartl.activeSelf);
        }
        else
        {
            m_NetworkPos = (Vector3)stream.ReceiveNext();
            m_NetworkRot = (Quaternion)stream.ReceiveNext();
            speedPartl.SetActive((bool)stream.ReceiveNext());
            chainPartl.SetActive((bool)stream.ReceiveNext());
        }
    }
}
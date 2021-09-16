using Photon.Pun;
using UnityEngine;

public class ShellExplosion : MonoBehaviourPun
{
    #region PUBLIC
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_MaxLifeTime = 3f;                  
    public float m_ExplosionRadius = 5f;
    public float m_TurnSpeed = 180f;


    #endregion

    #region PRIVATE
    private Rigidbody m_Rigidbody;
    private Vector3 m_LastVelocity;
    TankShooting tankShooting;
    #endregion

    #region STATIC
    public static readonly float m_Speed = 15f;
    #endregion


    private void Awake()
    {
        tankShooting = Resources.Load<TankShooting>("Tank");
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
        Rigidbody targetRigidbody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
            TankShooting tankShooting = targetRigidbody.GetComponent<TankShooting>();
    }

    private void LateUpdate()
    {
        m_LastVelocity = m_Rigidbody.velocity;
    }

    void RefBack()
    {
       
        tankShooting.reflectMinigun = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            
            
            
            if (tankShooting.reflectMinigun == true)
            {
                
                Destroy(gameObject);
                Invoke("RefBack", 2.4f);
            }
            else if(tankShooting.reflectMinigun == false)
            {
                Vector3 inDirection = m_LastVelocity.normalized;
                Vector3 inNormal = collision.contacts[0].normal;
                Vector3 reflectFIrection = Vector3.Reflect(inDirection, inNormal);
                m_Rigidbody.velocity = reflectFIrection * m_Speed;
            }
           


        }
        else
        {
            // Find all the tanks in an area around the shell and damage them.
            Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);

            foreach (var collider in colliders)
            {
                Rigidbody targetRigidbody = collider.GetComponent<Rigidbody>();

                if (!targetRigidbody)
                    continue;

                TankHealth tankHealth = targetRigidbody.GetComponent<TankHealth>();

                if (!tankHealth)
                    continue;

                float damage = CalculateDamage(targetRigidbody.position);

                tankHealth.TakeDamage(damage);
            }
         
            OnDestroy();
        }
    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

        float damage = relativeDistance * m_MaxDamage;

        damage = Mathf.Max(0f, damage);

        return damage;
    }

    private void OnDestroy()
    {
        
        m_ExplosionParticles.transform.parent = null;
        m_ExplosionParticles.Play();

        m_ExplosionAudio.Play();

        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);
        PhotonNetwork.Destroy(gameObject);
        
    }
}
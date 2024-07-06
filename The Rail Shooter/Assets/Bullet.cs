using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float m_speed = 30f;
    public Transform m_AimTarget = null;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Death");
    }

    // Update is called once per frame
    void Update()
    {   
        if(m_AimTarget == null)
        {
            transform.position += transform.forward * m_speed;
        }else 
        {
            transform.position = Vector3.MoveTowards(transform.position, m_AimTarget.position, m_speed);
        }

    }
    public void SetAimTarget(Transform target = null)
    {
        m_AimTarget=target;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInChildren<HitTarget>() != null)
        {
            collision.gameObject.GetComponentInChildren<HitTarget>().Hit(10f);
        }
    }
    public IEnumerator Death()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);

    }
}

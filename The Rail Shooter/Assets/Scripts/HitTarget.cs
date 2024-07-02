using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTarget : MonoBehaviour
{
    public float m_Health = 10f;
    public Animation m_DeathAnimation;
    public bool m_HasAnimationFinished = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(float damage)
    {
        Debug.Log("I have been hit");
        m_Health = m_Health -  damage;
        Debug.Log(m_Health.ToString());
        if (m_Health <= 0)
        {
            StartCoroutine("Death");
        }
    }

    public IEnumerator Death() 
    {
        m_DeathAnimation.Play();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        yield return null;

    }
}

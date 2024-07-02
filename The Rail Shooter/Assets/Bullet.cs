using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float m_speed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Death");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * m_speed;

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_Speed = 0f;
    public float m_LookSpeed = 0f;
    public Transform m_AimTarget;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        RotationLook(h, v, m_LookSpeed);
        MovePlayer(h,v,m_Speed);
        HorizontalLean(transform, h, 80, .1f);

    }

    public void MovePlayer(float x, float y, float speed)
    {
        transform.localPosition += new Vector3 (x, y, 0) * speed * Time.deltaTime;
        ClampPosition();
    }
    
    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint (pos);
    }

    void RotationLook(float h, float v, float speed)
    {
        m_AimTarget.parent.position = Vector3.zero; 
        m_AimTarget.localPosition = new Vector3(h, v, 1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(m_AimTarget.position), Mathf.Deg2Rad * speed * Time.deltaTime);
    }
    void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * leanLimit, lerpTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_AimTarget.position, .5f);
       Gizmos.DrawSphere(m_AimTarget.position, .15f);

    }
}

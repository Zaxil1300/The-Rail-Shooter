using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_Speed = 0f;
    public float m_LookSpeed = 0f;
    public Transform m_AimTarget;
    public LineRenderer m_LineRenderer;
    Camera m_Camera;
    public GameObject m_Bullet;
    public Transform m_GunBarrelL;
    public Transform m_GunBarrelR;

    public Animation m_BarrelRoll;

    int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = Camera.main;
        layerMask = LayerMask.GetMask("Target");
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        RotationLook(h, v, m_LookSpeed);
        MovePlayer(h,v,m_Speed);
        HorizontalLean(transform, h, 80, .1f);
        
        
        /*
        //laser
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            m_LineRenderer.SetPosition(0, transform.position);
            m_LineRenderer.SetPosition(1, transform.TransformDirection(Vector3.forward) * hit.distance);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            m_LineRenderer.SetPosition(0, transform.position);
            m_LineRenderer.SetPosition(1, transform.TransformDirection(Vector3.forward) * 10000);

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
        */
        if (Input.GetMouseButtonDown(0))
        {
            Transform aimTarget = null;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100f, layerMask);
            GameObject closestObject = null;
            float nearestObject = 100000f;
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log("We hit " + hitCollider.gameObject.name);
                float distance = Vector3.Distance(hitCollider.transform.position, transform.position); ;
                if (distance < nearestObject)
                {
                    distance = nearestObject;
                    closestObject = hitCollider.gameObject;
                }
            }
            if (closestObject != null) 
            {
                aimTarget = closestObject.transform;
            }

            //Vector3 mousePos = Input.mousePosition;
            //Vector3 newMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            //Quaternion shotAngle = Quaternion.FromToRotation(m_GunBarrel.position, newMousePos);
            //GameObject bulletTransform =  Instantiate(m_Bullet, m_GunBarrel.position, shotAngle);
            GameObject leftBullet = Instantiate(m_Bullet, m_GunBarrelL.position, m_GunBarrelL.rotation);
            GameObject rightBullet = Instantiate(m_Bullet, m_GunBarrelR.position, m_GunBarrelR.rotation);

            leftBullet.GetComponent<Bullet>().SetAimTarget(aimTarget);
            rightBullet.GetComponent<Bullet>().SetAimTarget(aimTarget);




        }

        if (Input.GetKeyDown("r")) 
        {
            //DO A BARREL ROLL
            m_BarrelRoll.Play();
        }

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
        Gizmos.DrawWireSphere(transform.position, 100f);
        //Gizmos.DrawSphere(transform.position, 100f);

    }
}

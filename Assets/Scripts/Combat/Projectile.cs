using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
    [SerializeField] private float speed = 1;
    [SerializeField] private bool isHoming = true;
    [SerializeField] private GameObject impactEffect = null;
    [SerializeField] private float lifeTime = 4f;
    [SerializeField] private GameObject[] destroyOnHit = null;
    [SerializeField] private float lifeAfterImpact = 2f;
    private Health target = null;
    private Vector3 targetOffset;
    private Collider targetCollider;
    private float damage = 0;

    private void Start() 
    {
        transform.LookAt(GenerateRandomTargetOffset());
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (target == null) return;
        if (isHoming && !target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);      
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }
    private Vector3 GenerateRandomTargetOffset()
    {
        targetCollider = target.GetComponent<Collider>();
        print(targetCollider.bounds.center);
        float randomX = Random.Range(-targetCollider.bounds.extents.x, targetCollider.bounds.extents.x);
        float randomZ = Random.Range(-targetCollider.bounds.extents.z, targetCollider.bounds.extents.z);
        float randomY = Random.Range(0f, targetCollider.bounds.extents.y * 2);
        print(targetCollider.bounds.extents.y);
        targetOffset = new Vector3(randomX, randomY, randomZ);
        print(transform.position + targetOffset);
        return target.transform.position + targetOffset;
    }
    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<Health>() != target) return;
        if(target.IsDead()) return;
        target.TakeDamage(damage); 

        speed = 0;

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }
        foreach (GameObject toDestroy in destroyOnHit)
        {
            Destroy(toDestroy);
        }
        Destroy(gameObject, lifeAfterImpact); 
    }

}
}

using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        private Health target = null;
        private Collider targetCollider;
        private Vector3 targetOffset;
        private float damage = 0f;
        private void Start() 
        {
            if(target == null) return;
            transform.LookAt(GenerateRandomTargetOffset());
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;
        }

        private void Update() 
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
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
        //TODO fix 
        private void OnTriggerEnter(Collider other) 
        {
            if(other.GetComponent<Health>() != target) return;  
            target.TakeDamage(damage); 
            Destroy(gameObject); 
        }

    }
}


using System;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        [SerializeField] private AnimatorOverrideController animatorOverride = null;
        [SerializeField] private GameObject equippedPrefab = null;
        [SerializeField] private Projectile projectile = null;
        [SerializeField] private float damage = 2f;
        [SerializeField] private float range = 2f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private bool isRightHanded = true;
        const string weaponName = "Weapon";

        public float GetDamage { get => damage;}
        public float GetRange { get => range;}
        public float TimeBetweenAttacks { get => timeBetweenAttacks;}
        public bool HasProjectile{ get => projectile != null;}

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            if (equippedPrefab != null)
            {
                GameObject weapon = Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));
                weapon.name = weaponName;
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            else
            {
                var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
                if(overrideController != null)
                {
                    animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                }
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform previousWeapon = rightHand.Find(weaponName);

            if(previousWeapon == null) previousWeapon = leftHand.Find(weaponName);

            if(previousWeapon == null) return;

            previousWeapon.name = "Destroying";
            Destroy(previousWeapon.gameObject);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return isRightHanded ? rightHand : leftHand;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, damage);
        }
    }
}

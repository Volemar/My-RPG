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

        public float GetDamage { get => damage;}
        public float GetRange { get => range;}
        public float TimeBetweenAttacks { get => timeBetweenAttacks;}
        public bool HasProjectile{ get => projectile != null;}

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Instantiate(equippedPrefab, GetHandTransform(rightHand, leftHand));
            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
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

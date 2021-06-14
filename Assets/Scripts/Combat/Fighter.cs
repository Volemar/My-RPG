using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon;

        private float timeSinceLastAttack = Mathf.Infinity;
        private Weapon currentWeapon = null;
        
        Health target;
        Mover mover;
        ActionScheduler action;
        Animator animator;
        private void Awake() {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }
        private void Start() {
            EquipWeapon(defaultWeapon);
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(rightHandTransform,leftHandTransform, animator);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if(target == null) return;
            if(target.IsDead()) return;
            if (!GetIsInRange())
            {
                mover.MoveTo(target.transform.position, 1f);
            }
            else
            {
                mover.Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack >= currentWeapon.TimeBetweenAttacks)
            {
                animator.ResetTrigger("stopAttacking");
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttacking");
            target = null;
            mover.Cancel();
        }

        //Animation event
        private void Hit()
        {
            if (target == null) return;
            if (currentWeapon.HasProjectile)
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
                return;
            }
            target.TakeDamage(currentWeapon.GetDamage);
        }

        private void Shoot()
        {
            Hit();
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null || !combatTarget.GetComponent<Health>() || combatTarget.GetComponent<Health>().IsDead())
            {
                return false;
            }
            return true;
        }
    }
}
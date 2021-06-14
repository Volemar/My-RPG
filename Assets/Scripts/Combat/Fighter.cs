using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private Transform handTransform = null;
        [SerializeField] private Weapon weapon;

        float timeSinceLastAttack = Mathf.Infinity;
        
        Health target;
        Mover mover;
        ActionScheduler action;
        Animator animator;
        private void Awake() {
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();
        }
        private void Start() {
            SpawnWeapon();
        }

        private void SpawnWeapon()
        {
            if(weapon == null) return;
            weapon.Spawn(handTransform, animator);
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
            if(timeSinceLastAttack >= weapon.TimeBetweenAttacks)
            {
                animator.ResetTrigger("stopAttacking");
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weapon.GetRange;
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
            target.TakeDamage(weapon.GetDamage);
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
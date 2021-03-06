using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health = 100f;

        private Animator animator;
        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        private void Start() {
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            if (isDead) return;
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead()) return;
            isDead = true;
            animator.SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return health;
        }

        public void RestoreState(object state)
        {
            health = (float)state;
            if (health <= 0)
            {
                Die();
            }
        }
    }
}
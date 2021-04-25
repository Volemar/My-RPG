using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour 
    {
        [SerializeField] float health = 100f;

        private Animator animator;

        public bool IsDead()
        {
            return health == 0;
        }

        private void Start() {
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            if (health == 0) return;
            health = Mathf.Max(health - damage, 0);
            print(health);
            if (health == 0)
            {
                animator.SetTrigger("die");
            }
        }
    }
}
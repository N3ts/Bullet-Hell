using DotNetsBH.Scripts.Actors.Interfaces;
using UnityEngine;

namespace DotNetsBH.Scripts.Actors.ShootingActor
{
    public abstract class ShootingActor : Actor, IDamageable
    {
        [SerializeField]
        protected BaseProjectile loadedProjectile;
        public BaseProjectile LoadedProjectile => loadedProjectile;

        [SerializeField] private float colliderRadiusOffset = 0f;
    
        [SerializeField]
        private int hitPoints;
        public int HitPoints
        {
            get => hitPoints;
            protected set => hitPoints = value > 0 ? value : 0; 
        }
    
        protected Vector2 AimDir;

        protected override void Awake()
        {
            base.Awake();
            if(loadedProjectile == null)
                loadedProjectile = GetComponent<BaseProjectile>();
        }

        protected float GetProjectileSpawnRadius()
        {
            return colliderRadius + loadedProjectile.ColliderRadius + colliderRadiusOffset;
        }
    
        /// <summary>
        ///     <para>
        ///         Sets value of AimDir. Should be normalized upon set. 
        ///     </para>
        /// </summary>
        protected abstract void UpdateAimDir();
    
        protected abstract void Shoot();

        /// <summary>
        ///     <para>
        ///         <paramref name="dmg"/> parameter substracts from hit points. The parameter can be adaptet within method-override.
        ///     </para>
        /// </summary>
        /// <param name="dmg">Integer Value that is used to subtract hit points.</param>
        public virtual void TakeDamage(int dmg) => HitPoints -= dmg;

        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Projectile"))
            {
                BaseProjectile proj = col.gameObject.GetComponent<BaseProjectile>();
                TakeDamage(proj.Damage);
                DebugOut("Damage Taken!\nDamage value: " + proj.Damage + "\nHP left: " + hitPoints);
            }
        }
    }
}

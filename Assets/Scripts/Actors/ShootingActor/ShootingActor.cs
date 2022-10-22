using DotNetsBH.Scripts.Actors.Interfaces;
using UnityEngine;

namespace DotNetsBH.Scripts.Actors.ShootingActor
{
    public abstract class ShootingActor : Actor, IDamageable
    {
        [SerializeField]
        protected BaseProjectile loadedProjectile;
        public BaseProjectile LoadedProjectile => loadedProjectile;

        [SerializeField] 
        private float colliderRadiusOffset = 0.1f;
        public float ColliderRadiusOffset
        {
            get => colliderRadiusOffset;
            set => colliderRadiusOffset = value <= 0? 0.1f : value;
        }
    
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

        /// <summary>
        ///     <para>
        ///         Adds Actors ColliderRadius, loadedProjectiles ColliderRadius and the colliderRadiusOffset together.
        ///     </para> 
        /// </summary>
        
        protected float GetProjectileSpawnRadius()
        {
            return colliderRadius + loadedProjectile.ColliderRadiusPreInstantiation + colliderRadiusOffset;
        }

        /// <summary>
        ///     <para>
        ///         Sets value of AimAt, the Point, that represents the target. 
        ///     </para>
        ///     <para>
        ///         Has to be implemented in child's FixedUpdate() function.
        ///     </para>
        /// </summary>        
        protected abstract void UpdateAimAt();
        
        /// <summary>
        ///     <para>
        ///         Sets value of AimDir. Should be normalized upon set. 
        ///     </para>
        ///     <para>
        ///         Has to be implemented in child's FixedUpdate() function.
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

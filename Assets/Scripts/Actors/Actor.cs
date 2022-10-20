using System;
using UnityEngine;

namespace DotNetsBH.Scripts.Actors
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public abstract class Actor : MonoBehaviour
    {
        [SerializeField]
        protected bool debug;
    
        protected Rigidbody2D Rb;
    
        // Collider fields
        protected Collider2D Col;
        protected float colliderRadius;
        public float ColliderRadius => colliderRadius;

        [SerializeField]
        protected float movementSpeed = 10;
        public float MovementSpeed => movementSpeed;

        protected Vector2 MoveDir;
    
        protected virtual void Awake()
        {
            if (Rb == null)
                Rb = GetComponent<Rigidbody2D>();
            if (Col == null)
                Col = GetComponent<Collider2D>();
            DetermineColliderRadius();
        }

        /// <summary>
        ///     <para>
        ///         Needs to take ChildObjects Collider and calculate largest minimum Radius of the Collider.
        ///     </para>
        /// </summary>
        protected abstract void DetermineColliderRadius();

        /// <summary>
        ///     <para>
        ///         Sets value of MoveDir. Should be normalized upon set. 
        ///     </para>
        /// </summary>
        protected abstract void UpdateMoveDir();
    
        /// <summary>
        ///     <para>
        ///         Sets Velocity of RigidBody by normalized MoveDir and movementSpeed.
        ///     </para>
        ///     <para>
        ///         Has to be implemented in child's FixedUpdate() function.
        ///     </para>
        /// </summary>
        protected virtual void Move()
        {
            Rb.velocity = MoveDir * movementSpeed;
        }

        protected virtual void DebugOut(String a)
        {
            if(debug)
                Debug.Log(a);
        }
    }
}
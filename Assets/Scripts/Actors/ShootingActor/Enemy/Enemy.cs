using DotNetsBH.Scripts.Actors.Interfaces;
using UnityEngine;

namespace DotNetsBH.Scripts.Actors.ShootingActor.Enemy
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Enemy : ShootingActor, IDamageable
    {
        protected override void UpdateMoveDir()
        {
            throw new System.NotImplementedException();
        }
    
        protected override void UpdateAimDir()
        {
            throw new System.NotImplementedException();
        }

        protected override void Shoot()
        {
            throw new System.NotImplementedException();
        }
    
        protected override void DetermineColliderRadius()
        {
            colliderRadius = GetComponent<CircleCollider2D>().radius;
        }
    }
}


using UnityEngine;

namespace DotNetsBH.Scripts.Actors.ShootingActor.Player
{
    public class CrossHair : MonoBehaviour
    {
        public void RotateCrossHair(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}
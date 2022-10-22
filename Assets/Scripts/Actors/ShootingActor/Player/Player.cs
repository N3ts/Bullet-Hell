using UnityEngine;
using UnityEngine.InputSystem;

namespace DotNetsBH.Scripts.Actors.ShootingActor.Player
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Player : ShootingActor, PlayerInputActions.IPlayerActions
    {
        /// Main input asset. Has Input Maps.
        private PlayerInputActions _playerInputActions;
        /// Input map for Player from asset
        private PlayerInputActions.PlayerActions _playerActions;
    
        private Camera _mainCam;

        private CrossHair _crossHair;
        private Quaternion _crossHairRotation;
        [SerializeField] private float crossHairRotationOffset = -90.0f;
    
        protected Vector2 MoveInput;
        protected Vector2 AimInput;
        protected Vector2 AimAt;

        protected override void Awake()
        {
            base.Awake();
            // Input Handler Generation
            _playerInputActions ??= new PlayerInputActions();
            _playerActions = _playerInputActions.Player;
            _playerActions.SetCallbacks(this);
        
            _mainCam = Camera.main;
            _crossHair = GetComponentInChildren<CrossHair>();
        }
      
        protected virtual void Update()
        {
            UpdateCrossHairRotation();
        }

        protected void FixedUpdate()
        {
            UpdateMoveDir();
            UpdateAimAt();
            UpdateAimDir();
            _crossHair.RotateCrossHair(_crossHairRotation);
        
            Move(); 
        }

        private void OnEnable()
        {
            Cursor.visible = false;
            _playerInputActions.Enable();
            //_playerActions.Enable();
        }

        private void OnDisable()
        {
            Cursor.visible = true;
            _playerInputActions.Disable();
            //_playerActions.Disable();
        }

        protected override void DetermineColliderRadius()
        {
            colliderRadius = GetComponent<CircleCollider2D>().radius;
        }

        protected override void UpdateMoveDir()
        {
            MoveDir = MoveInput.normalized;
        }

        protected override void UpdateAimAt()
        {
            // Muss noch auf Controller angepasst werden!!!
            Vector3 mousePos = _mainCam.ScreenToWorldPoint(AimInput);
            AimAt = mousePos.normalized;
        }
        
        protected override void UpdateAimDir()
        {
            AimDir = AimAt - (Vector2) transform.position;
        }
    
        /// <summary>
        ///     <para>
        ///         Calculates the Rotation of the CrossHair using _aimPos, _mouseDir, the current position and
        ///         adds crossHairRotationOffset.
        ///     </para> 
        /// </summary>
        protected virtual void UpdateCrossHairRotation()
        {
            float rotZ = Mathf.Atan2(AimAt.y, AimAt.x) * Mathf.Rad2Deg + crossHairRotationOffset;
            _crossHairRotation = Quaternion.Euler(0, 0, rotZ);
        }
 
        /// <summary>
        ///     <para>
        ///         Takes input from the shoot button and determines whether projectile ist instantiated.
        ///     </para>
        /// </summary>
        protected override void Shoot()
        {
            // AimAt point is interpreted as a direction Vector (point at which mouse is aimed at).
            Vector2 dir = AimAt == Vector2.zero ? Vector2.one : AimAt; 
            // The current Position of the Player:
            Vector2 position = (Vector2)transform.position;
            /*
             * ProjectileSpawnRadius equals the length from the player position to the spawn point on the
             * radius. It is divided by the length of the direction vector in which the Projectile is
             * supposed to move. This division equals the factor, with which the dir Vector can be stretched
             * to reach the spawnradius (crossing point).
             * This in turn equals the direction Vector from the player position to the ProjectileSpawnRadius.
             */
            Vector2 projSpawnDir = (GetProjectileSpawnRadius() / dir.magnitude) * dir;
            /*
             * Now the newly calculated Direction Vector, is added onto the Player Position, to calculate
             * the Spawn point for the projectile.
             */
            Vector2 projSpawnPoint = position + projSpawnDir; 
            
            BaseProjectile spawnedProj = Instantiate(loadedProjectile, projSpawnPoint, _crossHairRotation);
            spawnedProj.AimedAtDir = dir;
            if(debug) 
                Debug.DrawLine(position, projSpawnPoint, Color.green, 1);
        }
    
        #region Interface Implementation
        /// <summary>
        ///     <para>
        ///         Reads the current movement Input from the Keyboard or the left control stick
        ///     </para>
        /// </summary>
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        /// <summary>
        ///     <para>
        ///         Reads the position of the current aim, be it mouse position or right control stick direction.
        ///     </para>
        /// </summary>
        public void OnAim(InputAction.CallbackContext context)
        {
            AimInput = context.ReadValue<Vector2>();
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if(context.performed)
                Shoot();
        }
    
        #endregion
    }
}
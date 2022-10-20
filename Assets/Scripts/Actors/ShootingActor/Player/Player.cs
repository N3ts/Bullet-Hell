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
            UpdateAimDir();
            _crossHair.RotateCrossHair(_crossHairRotation);
        
            Move(); 
            //Shoot();
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

        protected override void UpdateAimDir()
        {
            // Muss noch auf Controller angepasst werden!!!
            Vector3 mousePos = _mainCam.ScreenToWorldPoint(AimInput);
            AimDir = mousePos.normalized;
        }
    
        /// <summary>
        ///     <para>
        ///         Calculates the Rotation of the CrossHair using _aimPos, _mouseDir, the current position and
        ///         adds crossHairRotationOffset.
        ///     </para> 
        /// </summary>
        protected virtual void UpdateCrossHairRotation()
        {
            // HIER NOCH REINSCHAUEN!!!
            float rotZ = Mathf.Atan2(AimDir.y, AimDir.x) * Mathf.Rad2Deg + crossHairRotationOffset;
            _crossHairRotation = Quaternion.Euler(0, 0, rotZ);
        }
 
        /// <summary>
        ///     <para>
        ///         Takes input from the shoot button and determines whether projectile ist instantiated.
        ///     </para>
        /// </summary>
        protected override void Shoot()
        {
            Vector2 projSpawnPoint =  (Vector2) transform.position + AimDir * GetProjectileSpawnRadius(); 
            BaseProjectile spawnedProj = Instantiate(loadedProjectile, projSpawnPoint, _crossHairRotation);
            spawnedProj.AimedAtDir = AimDir;
            Debug.DrawLine(transform.position, projSpawnPoint, Color.green, 1);
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
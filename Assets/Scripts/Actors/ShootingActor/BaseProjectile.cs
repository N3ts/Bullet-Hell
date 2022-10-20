using UnityEngine;

namespace DotNetsBH.Scripts.Actors.ShootingActor
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class BaseProjectile : Actor
    {
        private int _ownerID;
        public int OwnerID
        {
            set => _ownerID = value;
        }

        [SerializeField] private float aliveForS = 4;
        private float _tSinceSpawn;
        private float _tSpawned;
        private float _tToDeSpawn;

        [SerializeField] private int damage;
        public int Damage => damage;

        private Vector2 _aimedAtDir;
        public Vector2 AimedAtDir
        {
            set => _aimedAtDir = value;
        }

        protected override void Awake()
        {
            base.Awake();
            gameObject.tag = "Projectile";
        }
    
        protected virtual void Start()
        {
            InitSpawnTimeValues();
        }

        public virtual void Update()
        {
            UpdateMoveDir();
            IncTimeAlive();
            DeSpawnOnTimeUp();
        }

        protected void FixedUpdate()
        {
            Move();
        }
        
        
        protected virtual void OnCollisionEnter2D(Collision2D col)
        {
            Destroy(gameObject);
        }
        

        protected void OnDisable()
        {
            Destroy(gameObject);
        }

        protected override void DetermineColliderRadius()
        {
            colliderRadius = GetComponent<CircleCollider2D>().radius;
        }

        protected override void UpdateMoveDir()
        {
            MoveDir = _aimedAtDir;
        }
    
        private void InitSpawnTimeValues()
        {
            _tSpawned = _tSinceSpawn = Time.realtimeSinceStartup;
            _tToDeSpawn = _tSpawned + aliveForS;
        }
    
        /// <summary>
        ///     <para>
        ///         Increments <paramref name="-tSinceSpawn"/> by time since each frame passed.
        ///     </para>
        /// </summary>
        private void IncTimeAlive()
        {
            _tSinceSpawn += Time.deltaTime;
        }
    
        private void DeSpawnOnTimeUp()
        {
            if (_tSinceSpawn > _tToDeSpawn)
                Destroy(gameObject);
        }
    }
}
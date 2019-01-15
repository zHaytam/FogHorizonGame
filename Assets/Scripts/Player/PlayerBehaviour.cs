using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerBehaviour : PhysicsObject
    {

        #region Singleton

        // Check to see if we're about to be destroyed.
        private static bool _mShuttingDown;
        private static readonly object MLock = new object();
        private static PlayerBehaviour _mInstance;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static PlayerBehaviour Instance
        {
            get
            {
                if (_mShuttingDown)
                {
                    Debug.LogWarning("[Singleton] Instance PlayerBehaviour already destroyed. Returning null.");
                    return null;
                }

                lock (MLock)
                {
                    if (_mInstance == null)
                    {
                        // Search for existing instance.
                        _mInstance = (PlayerBehaviour)FindObjectOfType(typeof(PlayerBehaviour));

                        // Create new instance if one doesn't already exist.
                        if (_mInstance == null)
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            _mInstance = singletonObject.AddComponent<PlayerBehaviour>();
                            singletonObject.name = "PlayerBehaviour (Singleton)";

                            // Make instance persistent.
                            DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return _mInstance;
                }
            }
        }


        private void OnApplicationQuit()
        {
            _mShuttingDown = true;
        }


        private void OnDestroy()
        {
            _mShuttingDown = true;
        }

        #endregion

        #region Fields

        private SkeletonMecanim _skeletonMecanim;
        [SerializeField] private float _maxSpeed = 7;
        [SerializeField] private float _jumpTakeOffSpeed = 7;
        private Animator _animator;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _skeletonMecanim = GetComponent<SkeletonMecanim>();
            _animator = GetComponent<Animator>();
        }

        protected override void ComputeVelocity()
        {
            Vector2 move = Vector2.zero;

            move.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && Grounded)
            {
                Velocity.y = _jumpTakeOffSpeed;
            }
            else if (Input.GetButtonUp("Jump"))
            {
                if (Velocity.y > 0)
                {
                    Velocity.y = Velocity.y * 0.5f;
                }
            }

            if (move.x > 0.01f && Mathf.Approximately(transform.localScale.x, -1))
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            else if (move.x < -0.01f && Mathf.Approximately(transform.localScale.x, 1))
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }

            _animator.SetBool("grounded", Grounded);
            _animator.SetFloat("velocityX", Mathf.Abs(Velocity.x) / _maxSpeed);

            TargetVelocity = move * _maxSpeed;
        }

        #endregion

        #region Public Methods

        public void ChangeSlotAttachment(string slotName, string toAttachment)
        {
            _skeletonMecanim.Skeleton.SetAttachment(slotName, toAttachment);
        }

        #endregion

    }
}

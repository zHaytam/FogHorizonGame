using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PhysicsObject : MonoBehaviour
    {

        public float MinGroundNormalY = .65f;
        public float GravityModifier = 1f;

        protected Vector2 TargetVelocity;
        protected bool Grounded;
        protected Vector2 GroundNormal;
        protected Rigidbody2D Rb2D;
        protected Vector2 Velocity;
        protected ContactFilter2D ContactFilter;
        protected RaycastHit2D[] HitBuffer = new RaycastHit2D[16];
        protected List<RaycastHit2D> HitBufferList = new List<RaycastHit2D>(16);

        protected const float MinMoveDistance = 0.001f;
        protected const float ShellRadius = 0.01f;

        private void OnEnable()
        {
            Rb2D = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            ContactFilter.useTriggers = false;
            ContactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
            ContactFilter.useLayerMask = true;
        }

        private void Update()
        {
            TargetVelocity = Vector2.zero;
            ComputeVelocity();
        }

        protected virtual void ComputeVelocity() { }

        private void FixedUpdate()
        {
            Velocity += GravityModifier * Physics2D.gravity * Time.deltaTime;
            Velocity.x = TargetVelocity.x;

            Grounded = false;

            Vector2 deltaPosition = Velocity * Time.deltaTime;

            Vector2 moveAlongGround = new Vector2(GroundNormal.y, -GroundNormal.x);

            Vector2 move = moveAlongGround * deltaPosition.x;

            Movement(move, false);

            move = Vector2.up * deltaPosition.y;

            Movement(move, true);
        }

        private void Movement(Vector2 move, bool yMovement)
        {
            float distance = move.magnitude;

            if (distance > MinMoveDistance)
            {
                int count = Rb2D.Cast(move, ContactFilter, HitBuffer, distance + ShellRadius);
                HitBufferList.Clear();
                for (int i = 0; i < count; i++)
                {
                    HitBufferList.Add(HitBuffer[i]);
                }

                for (int i = 0; i < HitBufferList.Count; i++)
                {
                    Vector2 currentNormal = HitBufferList[i].normal;
                    if (currentNormal.y > MinGroundNormalY)
                    {
                        Grounded = true;
                        if (yMovement)
                        {
                            GroundNormal = currentNormal;
                            currentNormal.x = 0;
                        }
                    }

                    float projection = Vector2.Dot(Velocity, currentNormal);
                    if (projection < 0)
                    {
                        Velocity = Velocity - projection * currentNormal;
                    }

                    float modifiedDistance = HitBufferList[i].distance - ShellRadius;
                    distance = modifiedDistance < distance ? modifiedDistance : distance;
                }


            }

            Rb2D.position = Rb2D.position + move.normalized * distance;
        }

    }
}

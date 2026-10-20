using UnityEngine;
using Game.Application;
using Game.Domain;

namespace Game.Presentation
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallView : MonoBehaviour
    {
        private BallService m_ballService;
        private Rigidbody m_rigidBody;
        private Camera m_mainCam;
        private Renderer m_renderer;

        void Awake()
        {
            m_rigidBody = GetComponent<Rigidbody>();
            m_mainCam = Camera.main;
            m_renderer = GetComponent<Renderer>();
        }

        public void Init(BallService ballService)
        {
            m_ballService = ballService;
            
            if (m_rigidBody == null) 
            {
                m_rigidBody = GetComponent<Rigidbody>();
            }
            if (m_renderer == null) 
            {
                m_renderer = GetComponent<Renderer>();
            }
            
            m_ballService.OnWeightChanged += ApplyWeight;
            ApplyWeight(m_ballService.CurrentWeight);
        }

        void FixedUpdate()
        {
            if (m_ballService == null) 
            {
                return;
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 forward = Vector3.ProjectOnPlane(m_mainCam.transform.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(m_mainCam.transform.right, Vector3.up).normalized;
            Vector3 moveDir = (forward * v + right * h);

            var stats = BallStats.Get(m_ballService.CurrentWeight);

            if (Input.GetKey(KeyCode.Space))
            {
                m_rigidBody.linearDamping = 10f;
            }
            else
            {
                m_rigidBody.linearDamping = stats.Drag;
                m_rigidBody.AddForce(moveDir * stats.Force, ForceMode.Force);
            }
        }

        private void ApplyWeight(BallWeight weight)
        {
            var stats = BallStats.Get(weight);
            m_rigidBody.mass = stats.Mass;
            m_rigidBody.linearDamping = stats.Drag;
            
            if (weight == BallWeight.Wood) 
            {
                m_renderer.material.color = new Color(0.6f, 0.3f, 0f);
            }
            else if (weight == BallWeight.Stone) 
            {
                m_renderer.material.color = Color.gray;
            }
            else if (weight == BallWeight.Paper) 
            {
                m_renderer.material.color = Color.white;
            }
        }
    }
}
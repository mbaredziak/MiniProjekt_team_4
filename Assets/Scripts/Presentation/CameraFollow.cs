using UnityEngine;

namespace Game.Presentation
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Cel kamery")]
        [Tooltip("Transform kuli. Jeśli pozostanie puste, skrypt spróbuje znaleźć obiekt z tagiem 'Player'.")]
        [SerializeField] private Transform target;

        [Header("Pozycja i płynność")]
        [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -8f);
        [SerializeField] private float smoothSpeed = 8f;

        [Header("Obrót (Q / E / Mysz)")]
        [SerializeField] private float keyboardRotationSpeed = 100f;
        [SerializeField] private bool enableMouseRotation = false;
        [SerializeField] private float mouseRotationSpeed = 3f;

        private float _currentYaw = 0f;

        private void Start()
        {
            if (target == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    target = player.transform;
                }
                else
                {
                    Debug.LogWarning("[CameraFollow] Nie znaleziono obiektu z tagiem 'Player' w scenie!");
                }
            }

            // Inicjalizacja kąta bazując na początkowym ustawieniu offsetu
            _currentYaw = transform.eulerAngles.y;
        }

        private void LateUpdate()
        {
            if (target == null) return;

            HandleRotationInput();
            FollowTarget();
        }

        private void HandleRotationInput()
        {
            // Obrót klawiszami Q i E
            float rotationInput = 0f;
            if (Input.GetKey(KeyCode.Q)) rotationInput -= 1f;
            if (Input.GetKey(KeyCode.E)) rotationInput += 1f;

            _currentYaw += rotationInput * keyboardRotationSpeed * Time.deltaTime;

            // Opcjonalny obrót prawym przyciskiem myszy
            if (enableMouseRotation && Input.GetMouseButton(1))
            {
                _currentYaw += Input.GetAxis("Mouse X") * mouseRotationSpeed;
            }
        }

        private void FollowTarget()
        {
            // Obliczenie rotacji w osi Y wokół kuli
            Quaternion rotation = Quaternion.Euler(0f, _currentYaw, 0f);

            // Pozycja docelowa z uwzględnieniem obrotu
            Vector3 desiredPosition = target.position + (rotation * offset);

            // Płynne przejście kamery (Vector3.Lerp)
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Kamera stale patrzy na środek kuli
            transform.LookAt(target.position);
        }
    }
}
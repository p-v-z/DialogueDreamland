using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Implementation;
using Lightbug.Utilities;
using UnityEngine;

namespace DD
{
    [AddComponentMenu("DDCamera")]
    [DefaultExecutionOrder(ExecutionOrder.CharacterGraphicsOrder + 100)]  // <--- Do your job after everything else
    public class DDCamera  : MonoBehaviour
	{
        [Header("Inputs")]

        [SerializeField] InputHandlerSettings inputHandlerSettings = new InputHandlerSettings();
        [SerializeField] string axes = "Camera";
        [SerializeField] string zoomAxis = "Camera Zoom";

        [Header("Target")]
        [Tooltip("Select the graphics root object as your target, the one containing all the meshes, sprites, animated models, etc. \n\nImportant: This will be the considered as the actual target (visual element).")]
        [SerializeField] Transform targetTransform = null;
        [SerializeField] Vector3 offsetFromHead = Vector3.zero;
        [Tooltip("The interpolation speed used when the height of the character changes.")]
        [SerializeField] float heightLerpSpeed = 10f;

        [Header("View")]
        public CameraMode cameraMode = CameraMode.ThirdPerson;

        [Header("First Person")]
        public bool hideBody = true;
        [SerializeField] GameObject bodyObject = null;

        [Header("Yaw")]
        public bool updateYaw = true;
        public float yawSpeed = 180f;
        
        [Header("Pitch")]
        public bool updatePitch = true;
        [SerializeField] float initialPitch = 45f;
        public float pitchSpeed = 180f;
        [Range(1f, 85f)] public float maxPitchAngle = 80f;
        [Range(1f, 85f)] public float minPitchAngle = 80f;
        
        [Header("Roll")]
        public bool updateRoll = false;
        
        [Header("Zoom (Third person)")]
        public bool updateZoom = true;
        [Min(0f)][SerializeField] float distanceToTarget = 5f;
        [Min(0f)] public float zoomInOutSpeed = 40f;
        [Min(0f)] public float zoomInOutLerpSpeed = 5f;
        [Min(0f)] public float minZoom = 2f;
        [Min(0.001f)] public float maxZoom = 12f;
        
        [Header("Collision")]
        public bool collisionDetection = true;
        public bool collisionAffectsZoom = false;
        public float detectionRadius = 0.5f;
        public LayerMask layerMask = 0;
        public bool considerKinematicRigidbodies = true;
        public bool considerDynamicRigidbodies = true;

        // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        // ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
        
        CharacterActor characterActor = null;
        Rigidbody characterRigidbody = null;

        float currentDistanceToTarget;
        float smoothedDistanceToTarget;

        float deltaYaw = 0f;
        float deltaPitch = 0f;
        float deltaZoom = 0f;

        Vector3 lerpedCharacterUp = Vector3.up;       
        Vector3 previousLerpedCharacterUp = Vector3.up;

        Transform viewReference = null;
        Renderer[] bodyRenderers = null;
        RaycastHit[] hitsBuffer = new RaycastHit[10];
        RaycastHit[] validHits = new RaycastHit[10];
        Vector3 characterPosition = default(Vector3);
        float lerpedHeight;

        public enum CameraMode
        {
            FirstPerson,
            ThirdPerson,
        }

        public void ToggleCameraMode()
        {
            cameraMode = cameraMode == CameraMode.FirstPerson ? CameraMode.ThirdPerson : CameraMode.FirstPerson;
        }

        private void OnValidate()
        {
            initialPitch = Mathf.Clamp(initialPitch, -minPitchAngle, maxPitchAngle);
        }

        private void Awake()
        {
            if (targetTransform == null)
            {
                Debug.Log("The target graphics object is not active and enabled.");
                this.enabled = false;

                return;
            }

            characterActor = targetTransform.GetComponentInBranch<CharacterActor>();
            if (characterActor == null || !characterActor.isActiveAndEnabled)
            {
                Debug.Log("The character actor component is null, or it is not active/enabled.");
                this.enabled = false;

                return;
            }

            characterRigidbody = characterActor.GetComponent<Rigidbody>();
            inputHandlerSettings.Initialize(gameObject);
            var referenceObject = new GameObject("Camera referece");
            viewReference = referenceObject.transform;

            if (bodyObject != null)
            {
                bodyRenderers = bodyObject.GetComponentsInChildren<Renderer>();
            }
        }

        private void OnEnable()
        {
            if (characterActor == null)
                return;

            characterActor.OnTeleport += OnTeleport;
        }

        private void OnDisable()
        {
            if (characterActor == null)
                return;

            characterActor.OnTeleport -= OnTeleport;
        }

        private void Start()
        {
            characterPosition = targetTransform.position;
            
            previousLerpedCharacterUp = targetTransform.up;
            lerpedCharacterUp = previousLerpedCharacterUp;
            
            currentDistanceToTarget = distanceToTarget;
            smoothedDistanceToTarget = currentDistanceToTarget;

            viewReference.rotation = targetTransform.rotation;
            viewReference.Rotate(Vector3.right, initialPitch);

            lerpedHeight = characterActor.BodySize.y;
        }

        private void Update()
        {
            if (targetTransform == null)
            {
                this.enabled = false;
                return;
            }
            
            // Update direction if right mouse button is pressed
            var inputAxis = inputHandlerSettings.InputHandler.GetVector2(axes);
            var cameraAxes = Input.GetMouseButton(1) ? inputAxis : Vector2.zero;
            if (updatePitch) deltaPitch = -cameraAxes.y;
            if (updateYaw) deltaYaw = cameraAxes.x;
            if (updateZoom) deltaZoom = -inputHandlerSettings.InputHandler.GetFloat(zoomAxis);

            // An input axis value (e.g. mouse x) usually gets accumulated over time. So, the higher the frame rate the smaller the value returned.
            // In order to prevent inconsistencies due to frame rate changes, the camera movement uses a fixed delta time, instead of the old regular
            // delta time.
            var dt = Time.fixedDeltaTime;

            UpdateCamera(dt);
        }

        private void OnTeleport(Vector3 position, Quaternion rotation)
        {
            viewReference.rotation = rotation;
            transform.rotation = viewReference.rotation;

            lerpedCharacterUp = characterActor.Up;
            previousLerpedCharacterUp = lerpedCharacterUp;

        }


        private void HandleBodyVisibility()
        {
            if (cameraMode == CameraMode.FirstPerson)
            {
                HandleFirstPerson();

            }
            else
            {
                HandleThirdPerson();
            }
        }
        
        private void HandleFirstPerson()
        {

            if (bodyRenderers != null)
            {
                foreach (var bodyRenderer in bodyRenderers)
                {
                    if (bodyRenderer is SkinnedMeshRenderer skinnedMeshRenderer)
                    {
                        skinnedMeshRenderer.forceRenderingOff = hideBody;
                    }
                    else
                    {
                        bodyRenderer.enabled = !hideBody;
                    }
                }
            }
        }

        private void HandleThirdPerson()
        {
            if (bodyRenderers != null) 
            {
                foreach (var bodyRenderer in bodyRenderers)
                {
                    SetRenderer(bodyRenderer);
                }
            }
        }
        
        private static void SetRenderer(Renderer renderer)
        {
            if (renderer == null)
                return;

            if (renderer.GetType().IsSubclassOf(typeof(SkinnedMeshRenderer)))
            {
                var skinnedMeshRenderer = (SkinnedMeshRenderer)renderer;
                if (skinnedMeshRenderer != null)
                    skinnedMeshRenderer.forceRenderingOff = false;
            }
            else
            {
                renderer.enabled = true;
            }
        }

        private void UpdateCamera(float dt)
        {
            // Body visibility ---------------------------------------------------------------------
            HandleBodyVisibility();

            // Rotation -----------------------------------------------------------------------------------------
            lerpedCharacterUp = targetTransform.up;

            // Rotate the reference based on the lerped character up vector 
            Quaternion deltaRotation = Quaternion.FromToRotation(previousLerpedCharacterUp, lerpedCharacterUp);
            previousLerpedCharacterUp = lerpedCharacterUp;

            viewReference.rotation = deltaRotation * viewReference.rotation;

            // Yaw rotation -----------------------------------------------------------------------------------------        
            viewReference.Rotate(lerpedCharacterUp, deltaYaw * yawSpeed * dt, Space.World);

            // Pitch rotation -----------------------------------------------------------------------------------------            
            var angleToUp = Vector3.Angle(viewReference.forward, lerpedCharacterUp);
            var minPitch = -angleToUp + (90f - minPitchAngle);
            var maxPitch = 180f - angleToUp - (90f - maxPitchAngle);
            var pitchAngle = Mathf.Clamp(deltaPitch * pitchSpeed * dt, minPitch, maxPitch);
            viewReference.Rotate(Vector3.right, pitchAngle);

            // Roll rotation -----------------------------------------------------------------------------------------    
            if (updateRoll)
            {
                viewReference.up = lerpedCharacterUp;//Quaternion.FromToRotation( viewReference.up , lerpedCharacterUp ) * viewReference.up;
            }

            // Position of the target -----------------------------------------------------------------------
            characterPosition = targetTransform.position;

            lerpedHeight = Mathf.Lerp(lerpedHeight, characterActor.BodySize.y, heightLerpSpeed * dt);
            var targetPosition = characterPosition + targetTransform.up * lerpedHeight + targetTransform.TransformDirection(offsetFromHead);
            viewReference.position = targetPosition;

            var finalPosition = viewReference.position;

            // ------------------------------------------------------------------------------------------------------
            if (cameraMode == CameraMode.ThirdPerson)
            {
                currentDistanceToTarget += deltaZoom * zoomInOutSpeed * dt;
                currentDistanceToTarget = Mathf.Clamp(currentDistanceToTarget, minZoom, maxZoom);

                smoothedDistanceToTarget = Mathf.Lerp(smoothedDistanceToTarget, currentDistanceToTarget, zoomInOutLerpSpeed * dt);
                var displacement = -viewReference.forward * smoothedDistanceToTarget;

                if (collisionDetection)
                {
                    bool hit = DetectCollisions(ref displacement, targetPosition);

                    if (collisionAffectsZoom && hit)
                    {
                        currentDistanceToTarget = smoothedDistanceToTarget = displacement.magnitude;
                    }
                }

                finalPosition = targetPosition + displacement;
            }
            
            transform.position = finalPosition;
            transform.rotation = viewReference.rotation;
        }

        private bool DetectCollisions(ref Vector3 displacement, Vector3 lookAtPosition)
        {
            var hits = Physics.SphereCastNonAlloc(
                lookAtPosition,
                detectionRadius,
                Vector3.Normalize(displacement),
                hitsBuffer,
                currentDistanceToTarget,
                layerMask,
                QueryTriggerInteraction.Ignore
            );

            // Order the results
            var validHitsNumber = 0;
            for (var i = 0; i < hits; i++)
            {
                var hitBuffer = hitsBuffer[i];
                var detectedRigidbody = hitBuffer.collider.attachedRigidbody;

                // Filter the results ---------------------------
                if (hitBuffer.distance == 0)
                    continue;

                if (detectedRigidbody != null)
                {
                    if (considerKinematicRigidbodies && !detectedRigidbody.isKinematic)
                        continue;

                    if (considerDynamicRigidbodies && detectedRigidbody.isKinematic)
                        continue;

                    if (detectedRigidbody == characterRigidbody)
                        continue;
                }

                //----------------------------------------------            
                validHits[validHitsNumber] = hitBuffer;
                validHitsNumber++;
            }

            if (validHitsNumber == 0)
                return false;
            
            var distance = Mathf.Infinity;
            for (var i = 0; i < validHitsNumber; i++)
            {
                var hitBuffer = validHits[i];

                if (hitBuffer.distance < distance)
                    distance = hitBuffer.distance;
            }

            displacement = CustomUtilities.Multiply(Vector3.Normalize(displacement), distance);
            return true;
        }
    }
}

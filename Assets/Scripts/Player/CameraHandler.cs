using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;

namespace GJ
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTransform, cameraTransform, cameraPivotTransform;
        private Transform myTransform;

        private Vector3 cameraTransformPosition;
        private Vector3 cameraFollowVelocity = Vector3.zero;
        private LayerMask ignoreLayers;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;
        public float rotationSpeed = 1;

        private float targetPosition, defaultPosition, lookAngle, pivotAngle;

        public float minimumPivot = -35;
        public float maximumPivot = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;

        private void Awake()
        {
            singleton = this;
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * lookSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;

            Quaternion targetRotation = Quaternion.Lerp(myTransform.rotation, Quaternion.Euler(rotation), rotationSpeed * Time.deltaTime);
            myTransform.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Lerp(cameraPivotTransform.localRotation, Quaternion.Euler(rotation), rotationSpeed * Time.deltaTime);
            cameraPivotTransform.localRotation = targetRotation;
        }

        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if(Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
               float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
               targetPosition = -(dis - cameraCollisionOffset);
            }

            if(Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;

        }

        //[Range(1f, 100f)] public float range;

        //private void OnDrawGizmos()
        //{
        //    targetPosition = defaultPosition;
        //    Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
        //    direction.Normalize();

        //    //Debug.Log(direction);
        //    Debug.Log(targetPosition);
        //    Gizmos.DrawWireSphere(cameraPivotTransform.position, targetPosition);

        //    if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition)))
        //    {
        //        Gizmos.color = Color.green;
        //        Vector3 sphereCastMidpoint = cameraTransform.position + (cameraTransform.forward * hit.distance);
        //        Gizmos.DrawWireSphere(sphereCastMidpoint, cameraSphereRadius);
        //        Gizmos.DrawSphere(hit.point, 0.1f);
        //        Debug.DrawLine(cameraTransform.position, sphereCastMidpoint, Color.green);
        //    }
        //    else
        //    {
        //        Gizmos.color = Color.red;
        //        Vector3 sphereCastMidpoint = cameraTransform.position + (cameraTransform.forward * (range - cameraSphereRadius));
        //        Gizmos.DrawWireSphere(sphereCastMidpoint, cameraSphereRadius);
        //        Debug.DrawLine(cameraTransform.position, sphereCastMidpoint, Color.red);
        //    }
        //}
    }
}

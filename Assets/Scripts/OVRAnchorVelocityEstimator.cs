﻿namespace OculusIntegrationForVRTK.Tracking.Velocity
{
    using UnityEngine;
    using Zinnia.Tracking.Velocity;

    public class OVRAnchorVelocityEstimator : VelocityTracker
    {
        [Tooltip("The GameObject anchor from the OVRCameraRig to track velocity for.")]
        public GameObject trackedGameObject;

        [Tooltip("An optional object to consider the source relative to when retrieving velocities.")]
        public GameObject relativeTo;

        public override bool IsActive()
        {
            return trackedGameObject != null && trackedGameObject.activeInHierarchy && isActiveAndEnabled;
        }

        protected override Vector3 DoGetAngularVelocity()
        {
            switch (trackedGameObject.name)
            {
                case "CenterEyeAnchor":
                    return OVRManager.isHmdPresent
                        ? OVRPlugin.GetNodeAngularVelocity(OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render)
                            .FromFlippedZVector3f()
                        : Vector3.zero;
                case "LeftHandAnchor":
                    return OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);
                case "RightHandAnchor":
                    return OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
            }

            return Vector3.zero;
        }

        protected override Vector3 DoGetVelocity()
        {
            switch (trackedGameObject.name)
            {
                case "CenterEyeAnchor":
                    return OVRManager.isHmdPresent
                        ? OVRPlugin.GetNodeVelocity(OVRPlugin.Node.EyeCenter, OVRPlugin.Step.Render)
                            .FromFlippedZVector3f()
                        : Vector3.zero;
                case "LeftHandAnchor":
                    return OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
                case "RightHandAnchor":
                    return OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            }

            return Vector3.zero;
        }
    }
}

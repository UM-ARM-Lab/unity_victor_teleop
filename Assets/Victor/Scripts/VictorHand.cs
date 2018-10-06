using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;
using RosSharp.RosBridgeClient;

namespace Valve.VR.InteractionSystem.Sample
{
    [RequireComponent(typeof(RosSharp.RosBridgeClient.GripperPublisher))]
    [RequireComponent(typeof(RosSharp.RosBridgeClient.HandTargetPublisher))]
    public class VictorHand : MonoBehaviour
    {
        [SteamVR_DefaultActionSet("default")]
        public SteamVR_ActionSet actionSet;

        [SteamVR_DefaultAction("GrabGrip", "default")]
        public SteamVR_Action_Boolean a_grip;

        public Transform hand_real;

        public Material validIkMaterial;
        public Material invalidIkMaterial;
        public StringListener validIkListener;


        public GameObject arm;

        private Interactable interactable;

        private Quaternion trigSRot;

        private Quaternion joySRot;

        private Coroutine resettingRoutine;

        private Vector3 initialScale;

        private void Start()
        {
            //joySRot = modelJoystick.localRotation;
            //trigSRot = modelTrigger.localRotation;

            interactable = GetComponent<Interactable>();
            interactable.activateActionSetOnAttach = actionSet;

        }

        private void Update()
        {
            Vector2 steer = Vector2.zero;
            //float throttle = 0;
            //float brake = 0;

            //bool b_brake = false;
            //bool b_reset = false;
            bool gripped = false;



            if (interactable.attachedToHand)
            {
                SteamVR_Input_Sources hand = interactable.attachedToHand.handType;

                //steer = a_steering.GetAxis(hand);

                //throttle = a_trigger.GetAxis(hand);
                gripped = a_grip.GetState(hand);
                //Debug.Log("Gripped is " + gripped);
                RosSharp.RosBridgeClient.GripperPublisher gripper_pub = 
                    GetComponent<RosSharp.RosBridgeClient.GripperPublisher>();
                RosSharp.RosBridgeClient.HandTargetPublisher hand_target_pub = 
                    GetComponent<RosSharp.RosBridgeClient.HandTargetPublisher>();
                gripper_pub.Publish(gripped ? 1 : 0);
                hand_target_pub.PublishPose(this.transform);
                //b_brake = a_brake.GetState(hand);
                //b_reset = a_reset.GetState(hand);
                //brake = b_brake ? 1 : 0;
                //reset = a_reset.GetStateDown(hand);

                SetArmVisibility(true);
                if (validIkListener.HasNew())
                {
                    bool validIk = validIkListener.GetLast().data == "valid";
                    SetHandColor(validIk ? validIkMaterial : invalidIkMaterial);
                    if (!validIk)
                    {
                        interactable.attachedToHand.TriggerHapticPulse((ushort)10000);
                    }
                }
            }
            else
            {
                SetArmVisibility(false);
                this.transform.position = hand_real.transform.position;
                this.transform.rotation = hand_real.transform.rotation;
                this.transform.localScale = hand_real.transform.localScale;
                SetHandColor(validIkMaterial);
            }

        }

        private void SetHandColor(Material m)
        {

            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                r.material = m;
            }

        }

        private void SetArmVisibility(bool vis)
        {
            /*
            var links = gameObject.GetComponentsInChildren<RosSharp.Urdf.UrdfLink>();
            foreach (var link in links)
            {
                foreach (Renderer r in link.GetComponentsInChildren<Renderer>())
                {
                    r.enabled = vis;
                }
            }
            */
            foreach (Renderer r in arm.GetComponentsInChildren<Renderer>())
            {
                r.enabled = vis;
            }
        }
    }
}


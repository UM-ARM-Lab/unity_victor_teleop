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
        //[SteamVR_DefaultActionSet("default")]
        public SteamVR_ActionSet actionSet;

        //[SteamVR_DefaultAction("GrabPinch", "default")]
        public SteamVR_Action_Single a_grip;

        public Transform hand_real;

        public Material validIkMaterial;
        public Material invalidIkMaterial;
        public StringListener validIkListener;
        public WrenchStampedListener wrenchStampedListener;

        public double forceThreshold = 0;
        public double forceMax = 0;


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
            float grip = 0;

            if (wrenchStampedListener != null)
            {
                //RosSharp.RosBridgeClient.Messages.Geometry.Wrench t = wrenchListener.GetLast();
                RosSharp.RosBridgeClient.Messages.Geometry.WrenchStamped t = wrenchStampedListener.GetLast();
                
                if(t == null)
                {
                    //Debug.Log("Null twist");
                }
                else
                {
                    //Debug.Log(t.wrench.force.x);
                }
                
            }


            if (interactable.attachedToHand)
            {
                SteamVR_Input_Sources hand = interactable.attachedToHand.handType;

                //steer = a_steering.GetAxis(hand);

                //throttle = a_trigger.GetAxis(hand);
                //gripped = a_grip.GetState(hand);
                grip = a_grip.GetAxis(hand);
                //Debug.Log("Gripped is " + gripped);
                RosSharp.RosBridgeClient.GripperPublisher gripper_pub = 
                    GetComponent<RosSharp.RosBridgeClient.GripperPublisher>();
                RosSharp.RosBridgeClient.HandTargetPublisher hand_target_pub = 
                    GetComponent<RosSharp.RosBridgeClient.HandTargetPublisher>();
                gripper_pub.Publish(grip);
                hand_target_pub.PublishPose(this.transform);
                //b_brake = a_brake.GetState(hand);
                //b_reset = a_reset.GetState(hand);
                //brake = b_brake ? 1 : 0;
                //reset = a_reset.GetStateDown(hand);

                if (wrenchStampedListener != null &&
                    wrenchStampedListener.GetLast() != null)
                {
                    var f = wrenchStampedListener.GetLast().wrench.force;
                    float fsq = (f.x * f.x + f.y * f.y + f.z * f.z);

                    if(fsq > forceThreshold * forceThreshold)
                    {
                        var a = System.Math.Min(1, Mathf.Sqrt(fsq) / forceMax);
                        interactable.attachedToHand.TriggerHapticPulse((ushort)(3000*a));
                    }
                }

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


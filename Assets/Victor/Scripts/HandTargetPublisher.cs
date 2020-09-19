/*
© Siemens AG, 2017-2018
Author: Dr. Martin Bischoff (martin.bischoff@siemens.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
<http://www.apache.org/licenses/LICENSE-2.0>.
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using UnityEngine;

namespace RosSharp.RosBridgeClient
{
    public class HandTargetPublisher : Publisher<Messages.Geometry.PoseStamped>
    {

        public string FrameId = "victor_root";
        public float maxUpdateIntervalSec;

        private Messages.Geometry.PoseStamped message;
        private float nextTimeToSend = 0;

        public void PublishPose(Transform tf)
        {
            
            if (Time.time < nextTimeToSend)
            {
                return;
            }
           
            UpdateMessage(tf);
            Publish(message);
            nextTimeToSend = Time.time + maxUpdateIntervalSec;
        }

        protected override void Start()
        {
            base.Start();
            InitializeMessage();
        }


        private void InitializeMessage()
        {
            message = new Messages.Geometry.PoseStamped
            {
                header = new Messages.Standard.Header()
                {
                    frame_id = FrameId
                }
            };
        }

        private void UpdateMessage(Transform tf)
        {
            message.header.Update();
            message.pose.position = GetGeometryPoint(tf.position.Unity2Ros());
            message.pose.orientation = GetGeometryQuaternion(tf.rotation.Unity2Ros());
        }

        private Messages.Geometry.Point GetGeometryPoint(Vector3 position)
        {
            Messages.Geometry.Point geometryPoint = new Messages.Geometry.Point();
            geometryPoint.x = position.x;
            geometryPoint.y = position.y;
            geometryPoint.z = position.z;
            return geometryPoint;
        }

        private Messages.Geometry.Quaternion GetGeometryQuaternion(Quaternion quaternion)
        {
            Messages.Geometry.Quaternion geometryQuaternion = new Messages.Geometry.Quaternion();
            geometryQuaternion.x = quaternion.x;
            geometryQuaternion.y = quaternion.y;
            geometryQuaternion.z = quaternion.z;
            geometryQuaternion.w = quaternion.w;
            return geometryQuaternion;
        }
    }
}

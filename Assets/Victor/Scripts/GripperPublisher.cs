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
    public class GripperPublisher : Publisher<Messages.Sensor.Joy>
    {
        private JoyAxisReader[] JoyAxisReaders;
        private JoyButtonReader[] JoyButtonReaders;

        public string FrameId = "Unity";
        public float maxUpdateIntervalSec;

        private float nextTimeToSend = 0;

        private Messages.Sensor.Joy message;

        public void Publish(float gripper_val)
        {
            if(Time.time < nextTimeToSend)
            {
                return;
            }
            message.axes[0] = gripper_val;
            Publish(message);
            nextTimeToSend = Time.time + maxUpdateIntervalSec;
        }

        protected override void Start()
        {
            base.Start();
            InitializeGameObject();
            InitializeMessage();
        }

        private void Update()
        {
            //UpdateMessage();
        }      

        private void InitializeGameObject()
        {
            JoyAxisReaders = GetComponents<JoyAxisReader>();
            JoyButtonReaders = GetComponents<JoyButtonReader>();            
        }

        private void InitializeMessage()
        {
            message = new Messages.Sensor.Joy();
            message.header.frame_id = FrameId;
            message.axes = new float[1];
            message.buttons = new int[0];
        }

        private void UpdateMessage()
        {
            message.header.Update();

            for (int i = 0; i < JoyAxisReaders.Length; i++)
                message.axes[i] = JoyAxisReaders[i].Read();
            
            for (int i = 0; i < JoyButtonReaders.Length; i++)
                message.buttons[i] = (JoyButtonReaders[i].Read() ? 1 : 0);

            Publish(message);
        }
    }
}

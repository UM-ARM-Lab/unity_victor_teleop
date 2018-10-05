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
using OpenCvSharp;
using Unity.Collections;
using Unity.Jobs;

namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosConnector))]
    public class DepthImageListener: Subscriber<Messages.Sensor.CompressedImage>
    {

        //private Texture2D texture2D;
        private bool hasNew;
        private NativeArray<short> decompressedDepth;
        private JobHandle jobHandle;

        protected override void Start()
        {
			base.Start();
            //texture2D = new Texture2D(1, 1);
            decompressedDepth = new NativeArray<short>(960 * 540, Allocator.Persistent);
            hasNew = false;
        }


        protected override void ReceiveMessage(Messages.Sensor.CompressedImage compressedImage)
        {

            Mat mat = Mat.ImDecode(compressedImage.data, ImreadModes.AnyDepth);
            short[] data = new short[960 * 540];
            mat.GetArray(0, 0, data);
            decompressedDepth.CopyFrom(data);
            hasNew = true;
            /*
            if (hasRecievedData && !jobHandle.IsCompleted)
            {
                return;
            }
            decompressedDepth.Dispose();
            decompressedDepth = new NativeArray<short>(960 * 540, Allocator.Persistent);
            hasRecievedData = true;
            hasNew = true;


            DecompressDepthJob job = new DecompressDepthJob();
            job.compressed = new NativeArray<byte>(compressedImage.data, Allocator.TempJob);
            job.decompressed = decompressedDepth;
            JobHandle h = job.Schedule();

            h.Complete();
            job.compressed.Dispose();*/
        }

        public bool HasNew()
        {
            return hasNew;
        }

        public NativeArray<short> GetLast()
        {
            
            hasNew = false;
            return decompressedDepth;
            //texture2D.LoadImage(imageData);
            //texture2D.Apply();
            //return texture2D;
        }

        public void OnApplicationQuit()
        {
            decompressedDepth.Dispose();
        }



    }

    public struct DecompressDepthJob : IJob
    {
        public NativeArray<short> decompressed;
        public NativeArray<byte> compressed;

        public void Execute()
        {
            Mat mat = Mat.ImDecode(compressed.ToArray(), ImreadModes.AnyDepth);
            short[] data = new short[960 * 540];
            mat.GetArray(0, 0, data);
            decompressed.CopyFrom(data);
        }
    }
}


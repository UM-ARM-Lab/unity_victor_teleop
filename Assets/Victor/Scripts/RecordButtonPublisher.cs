

using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace RosSharp.RosBridgeClient
{
    public class RecordButtonPublisher : Publisher<Messages.Standard.String>
    {

        public HoverButton hoverButton;
        private Messages.Standard.String message;
        public bool recording = false;
        public GameObject indicatorOn;
        public GameObject indicatorOff;

        IEnumerator PublishPeriodically()
        {
            for (; ; )
            {
                if (recording)
                {
                    Publish(message);
                }
               
                yield return new WaitForSeconds(0.1f);
            }
        }

        IEnumerator Blink()
        {
            for(; ; )
            {
                if(recording)
                {
                    indicatorOn.SetActive(!indicatorOn.activeSelf);
                }
                yield return new WaitForSeconds(0.7f);
            }
        }

        protected override void Start()
        {
            base.Start();
            message = new Messages.Standard.String("record");
            hoverButton.onButtonDown.AddListener(OnButtonDown);
            StartCoroutine("PublishPeriodically");
            StartCoroutine("Blink");
        }

        public void OnButtonDown(Hand fromHand)
        {
            recording = !recording;

            indicatorOn.SetActive(recording);
            indicatorOff.SetActive(!recording);
        }

        private void Update()
        {

        }

    }
}

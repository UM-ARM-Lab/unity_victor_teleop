using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

namespace ArmLab.Victor
{
    public class ButtonEnabler : MonoBehaviour
    {
        public HoverButton hoverButton;
        public GameObject toggledObject;

        private void Start()
        {
            hoverButton.onButtonDown.AddListener(OnButtonDown);
        }

        public void OnButtonDown(Hand fromHand)
        {
            toggledObject.SetActive(!toggledObject.activeSelf);
        }
    }
}

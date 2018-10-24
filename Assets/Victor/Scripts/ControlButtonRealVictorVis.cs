//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

namespace ArmLab.Victor
{
    public class ControlButtonRealVictorVis : MonoBehaviour
    {
        public HoverButton hoverButton;

        private bool visible;

        private void Start()
        {
            hoverButton.onButtonDown.AddListener(OnButtonDown);
            visible = true;
        }

        public void OnButtonDown(Hand fromHand)
        {
            visible = !visible;
            SetVisibility(visible);
        }


        private void SetVisibility(bool vis)
        {
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.enabled = vis;
            }
        }
    }
}
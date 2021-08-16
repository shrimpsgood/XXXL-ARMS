using System;
using System.Collections.Generic;
using HarmonyLib;
using BepInEx;
using UnityEngine;
using System.Reflection;
using UnityEngine.XR;
using Photon.Pun;
using System.IO;
using System.Net;
using Photon.Realtime;
using UnityEngine.Rendering;

/* This mod was made by JJoe (jona939s on github) */

namespace longArms
{
    [BepInPlugin("org.J-JOE.monkeytag.LongArms", "LongArms", "0.5.0.1")]
    public class MyPatcher : BaseUnityPlugin
    {
        public void Awake()
        {
            var harmony = new Harmony("com.J-JOE.monkeytag.LongArms");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            var Default = GorillaLocomotion.Player.Instance.leftHandOffset;
            var Default2 = GorillaLocomotion.Player.Instance.rightHandOffset;
        }
    }

    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("Update", MethodType.Normal)]
    public class Class1
    {   
        private static float timeSinceLastChange = 0f;

        static float myVarY1 = 0f;
        static float myVarY2 = 0f;

        static bool gain = false;
        static bool less = false;
        static bool reset = false;
        static bool fastr = false;
        static bool speed1 = true;

        static float gainSpeed = 1f;
        static void Postfix(GorillaLocomotion.Player __instance)
        {
            bool Enabled = PhotonNetwork.CurrentRoom == null ? false : !PhotonNetwork.CurrentRoom.IsVisible;

            if (Enabled != true)
            {
                GorillaLocomotion.Player.Instance.leftHandOffset = new Vector3(0f, 0f, 0f);
                GorillaLocomotion.Player.Instance.rightHandOffset = new Vector3(0f, 0f, 0f);
            }

            if (Enabled == true)
            {
                
                List<InputDevice> list = new List<InputDevice>();
                InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller, list);
                list[0].TryGetFeatureValue(CommonUsages.gripButton, out gain);
                list[0].TryGetFeatureValue(CommonUsages.triggerButton, out less);
                list[0].TryGetFeatureValue(CommonUsages.primaryButton, out reset);
                list[0].TryGetFeatureValue(CommonUsages.secondaryButton, out fastr);

                timeSinceLastChange += Time.deltaTime;
                //max float 6.0f
                if (timeSinceLastChange <= 0.2f)
                {
                    return;
                }

                GorillaLocomotion.Player.Instance.leftHandOffset = new Vector3(0f, myVarY1, 0f);
                GorillaLocomotion.Player.Instance.rightHandOffset = new Vector3(0f, myVarY2, 0f);

                GorillaLocomotion.Player.Instance.maxArmLength = 200f;

                if (gain)
                {
                    timeSinceLastChange = 0f;

                    myVarY1 = myVarY1 + gainSpeed;
                    myVarY2 = myVarY2 + gainSpeed;

                    if (myVarY1 >= 201f)
                    {
                        myVarY1 = 200f;
                        myVarY2 = 200f;
                    }

                }

                if (less)
                {
                    timeSinceLastChange = 0f;

                    myVarY1 = myVarY1 - gainSpeed;
                    myVarY2 = myVarY2 - gainSpeed;

                    if (myVarY2 <= -6f)
                    {
                        myVarY1 = -5f;
                        myVarY2 = -5f;
                    }
                }
                if (reset)
                {
                    timeSinceLastChange = 0f;

                    myVarY1 = 0f;
                    myVarY2 = 0f;
                }
                if (fastr)
                {
                    if (myVarY1 == 5f)
                    {
                        myVarY1 = 10f;
                        myVarY2 = 10f;
                    }
                }
            }
        }
    }
}
/* This mod was made by JJoe (jona939s on github)*/
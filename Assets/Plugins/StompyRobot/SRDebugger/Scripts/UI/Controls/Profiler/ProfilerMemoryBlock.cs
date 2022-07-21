﻿namespace SRDebugger.UI.Controls
{
    using System;
    using System.Collections;
    using SRF;
    using UnityEngine;
    using UnityEngine.UI;
#if UNITY_5_5_OR_NEWER
    using UnityEngine.Profiling;
#endif

    public class ProfilerMemoryBlock : SRMonoBehaviourEx
    {
        private float _lastRefresh;

        [RequiredField] public Text CurrentUsedText;

        [RequiredField] public Slider Slider;

        [RequiredField] public Text TotalAllocatedText;

        protected override void OnEnable()
        {
            base.OnEnable();
            TriggerRefresh();
        }

        protected override void Update()
        {
            base.Update();

            if (SRDebug.Instance.IsDebugPanelVisible && (Time.realtimeSinceStartup - _lastRefresh > 1f))
            {
                TriggerRefresh();
                _lastRefresh = Time.realtimeSinceStartup;
            }
        }

        public void TriggerRefresh()
        {
            var max = Profiler.GetTotalReservedMemory();
            var current = Profiler.GetTotalAllocatedMemory();

            Slider.maxValue = max;
            Slider.value = current;

            var maxMb = (max >> 10);
            maxMb /= 1024; // On new line to fix il2cpp

            var currentMb = (current >> 10);
            currentMb /= 1024;

            TotalAllocatedText.text = "Reserved: <color=#FFFFFF>{0}</color>MB".Fmt(maxMb);
            CurrentUsedText.text = "<color=#FFFFFF>{0}</color>MB".Fmt(currentMb);
        }

        public void TriggerCleanup()
        {
            StartCoroutine(CleanUp());
        }

        private IEnumerator CleanUp()
        {
            GC.Collect();
            yield return Resources.UnloadUnusedAssets();
            GC.Collect();

            TriggerRefresh();
        }
    }
}

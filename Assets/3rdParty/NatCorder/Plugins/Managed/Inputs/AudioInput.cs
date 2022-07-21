/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Inputs {

    using UnityEngine;
    using System;
    using Clocks;
    using Docs;

    /// <summary>
    /// Recorder input for recording game audio from an AudioListener or AudioSource
    /// </summary>
    [Doc(@"AudioInput"), AddComponentMenu(""), DisallowMultipleComponent]
    public sealed class AudioInput : MonoBehaviour, IDisposable {

        #region --Client API--
        /// <summary>
        /// Create an audio recording input from a scene's AudioListener
        /// </summary>
        /// <param name="mediaRecorder">Media recorder to receive committed frames</param>
        /// <param name="sceneAudio">Audio listener for the current scene</param>
        /// <param name="clock">Optional. Clock for generating timestamps</param>
        [Doc(@"AudioInputCreateListener")]
        public static AudioInput Create (IMediaRecorder mediaRecorder, AudioListener sceneAudio, IClock clock = null) {
            // Null checking
            if (!sceneAudio) {
                Debug.LogError("NatCorder Error: Cannot create audio recorder for null AudioListener");
                return null;
            }
            var recorder = sceneAudio.gameObject.AddComponent<AudioInput>();
            recorder.mediaRecorder = mediaRecorder;
            recorder.clock = clock ?? new RealtimeClock();
            return recorder;
        }

        /// <summary>
        /// Create an audio recording input from an AudioSource
        /// </summary>
        /// <param name="audioSource">Audio source to record</param>
        /// <param name="mute">Optional. Mute audio source after recording so that it is not heard in scene</param>
        /// <param name="clock">Optional. Clock for generating timestamps</param>
        [Doc(@"AudioInputCreateSource")]
        public static AudioInput Create (IMediaRecorder mediaRecorder, AudioSource audioSource, IClock clock = null, bool mute = false) {
            // Null checking
            if (!audioSource) {
                Debug.LogError("NatCorder Error: Cannot create audio recorder for null AudioSource");
                return null;
            }
            var recorder = audioSource.gameObject.AddComponent<AudioInput>();
            recorder.mediaRecorder = mediaRecorder;
            recorder.clock = clock ?? new RealtimeClock();
            recorder.mute = mute;
            return recorder;
        }

        /// <summary>
        /// Stop recording and teardown resources
        /// </summary>
        [Doc(@"AudioInputDispose")]
        public void Dispose () {
            AudioInput.Destroy(this);
        }
        #endregion


        #region --Operations--

        private IMediaRecorder mediaRecorder;
        private IClock clock;
        private bool mute = false;

        private AudioInput () {}

        private void OnAudioFilterRead (float[] data, int channels) {
            mediaRecorder.CommitSamples(data, clock.Timestamp);
            if (mute)
                Array.Clear(data, 0, data.Length);
        }
        #endregion
    }
}
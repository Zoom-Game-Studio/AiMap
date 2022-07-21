/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Inputs {

    using UnityEngine;
    using System;
    using Clocks;
    using Docs;
    using Dispatch;

    /// <summary>
    /// Recorder input for recording a game camera
    /// </summary>
    [Doc(@"CameraInput")]
    public sealed class CameraInput : IDisposable {
        
        #region --Client API--
        /// <summary>
        /// Control number of successive camera frames to skip while recording.
        /// This is very useful for GIF recording, which typically has a lower framerate appearance
        /// </summary>
        [Doc(@"CameraInputNthFrame", @"CameraInputNthFrameDiscussion"), Code(@"RecordGIF")]
        public int recordEveryNthFrame = 1;

        /// <summary>
        /// Create a video recording input from a game camera
        /// </summary>
        /// <param name="mediaRecorder">Media recorder to receive committed frames</param>
        /// <param name="camera">Game camera to record</param>
        /// <param name="clock">Optional. Clock for generating timestamps</param>
        [Doc(@"CameraInputCreate"), Code(@"RecordGIF")]
        public static CameraInput Create (IMediaRecorder mediaRecorder, Camera camera, IClock clock = null) {
            if (mediaRecorder == null) {
                Debug.LogError("NatCorder Error: Cannot create video recorder with null media recorder");
                return null;
            }
            if (!camera) {
                Debug.LogError("NatCorder Error: Cannot create video recorder with null camera");
                return null;
            }
            return new CameraInput(mediaRecorder, camera, clock ?? new RealtimeClock());
        }

        /// <summary>
        /// Stop recorder input and teardown resources
        /// </summary>
        [Doc(@"CameraInputDispose")]
        public void Dispose () {
            DispatchUtility.onFrame -= OnFrame;
        }
        #endregion


        #region --Operations--

        private readonly IMediaRecorder mediaRecorder;
        private readonly Camera camera;
        private readonly IClock clock;
        private int frameCount;

        private CameraInput (IMediaRecorder mediaRecorder, Camera camera, IClock clock) {
            this.mediaRecorder = mediaRecorder;
            this.camera = camera;
            this.clock = clock;
            DispatchUtility.onFrame += OnFrame;
        }

        private void OnFrame () {
            if (frameCount++ % recordEveryNthFrame == 0)
                RecordFrame();
        }

        private void RecordFrame () {
            // Acquire frame, save camera state
            var encoderFrame = mediaRecorder.AcquireFrame();
            var prevTarget = camera.targetTexture;
            camera.targetTexture = encoderFrame;               
            // Render
            camera.Render();
            // Restore camera state, commit frame
            camera.targetTexture = prevTarget;
            mediaRecorder.CommitFrame(encoderFrame, clock.Timestamp);
        }
        #endregion
    }
}
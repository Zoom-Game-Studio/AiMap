/* 
*   NatCorder
*   Copyright (c) 2019 Yusuf Olokoba
*/

namespace NatCorder.Platforms {

    using UnityEngine;
    using UnityEngine.Scripting;
    using System;
    using System.Runtime.InteropServices;
    using Dispatch;
    using Readback;

    public sealed class MediaRecorderAndroid : AndroidJavaProxy, IMediaRecorder {

        #region --Op vars--
        private readonly AndroidJavaObject recorder;
        private readonly int width, height;
        private readonly Action<string> callback;
        private readonly ReadbackContext readbackContext;
        private readonly MainDispatcher dispatcher;
        #endregion

        
        #region --IMediaRecorder--

        public MediaRecorderAndroid (AndroidJavaObject recorder, int width, int height, string recordingDirectory, Action<string> callback) : base(@"com.yusufolokoba.natcorder.RecordingDelegate") {
            this.recorder = recorder;
            this.width = width;
            this.height = height;
            this.callback = callback;
            this.readbackContext = new GLESReadback(true);
            this.dispatcher = new MainDispatcher();
            // Start recording
            using (var dispatcher = new RenderDispatcher())
                dispatcher.Dispatch(() => recorder.Call(@"startRecording", recordingDirectory, this));
        }

        public void Dispose () {
            readbackContext.Readback(null, ptr => recorder.Call(@"stopRecording"));
            readbackContext.Dispose();
        }
        
        public RenderTexture AcquireFrame () {
            return readbackContext.Allocate(width, height);
        }

        public void CommitFrame (RenderTexture frame, long timestamp) {
            var textureHandle = (IntPtr)GCHandle.Alloc(frame, GCHandleType.Normal);
            readbackContext.Readback(frame, framePtr => {
                recorder.Call(@"encodeFrame", framePtr.ToInt64(), textureHandle.ToInt64(), timestamp);
            });
        }

        public void CommitSamples (float[] sampleBuffer, long timestamp) {
            AndroidJNI.AttachCurrentThread();
            recorder.Call(@"encodeSamples", sampleBuffer, timestamp);
        }
        #endregion


        #region --Callbacks--

        [Preserve]
        private void onEncode (long textureHandle) {
            var frameHandle = (GCHandle)(IntPtr)textureHandle;
            var frame = frameHandle.Target as RenderTexture;
            frameHandle.Free();
            dispatcher.Dispatch(() => RenderTexture.ReleaseTemporary(frame));
        }

        [Preserve]
        private void onRecording (string path) {
            dispatcher.Dispatch(() => {
                recorder.Dispose();
                callback(path);
            });
            dispatcher.Dispose();
        }
        #endregion
    }
}
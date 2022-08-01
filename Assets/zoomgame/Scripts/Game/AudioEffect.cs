using Architecture;
using Architecture.Command;
using QFramework;
using UnityEngine;

namespace zoomgame.Scripts.Game
{
    public class AudioEffect : AbstractMonoController
    {
        
        [SerializeField] private AudioClip success, fail,hit;

        private void Start()
        {
            this.RegisterEvent<PlayAudioEvent>(OnFinish);
        }
        
        private void OnFinish(PlayAudioEvent obj)
        {
            if (obj.success)
            {
                AudioSource.PlayClipAtPoint(success,Camera.main.transform.position);
            }
            else
            {
                AudioSource.PlayClipAtPoint(hit, Camera.main.transform.position);
            }
        }
    }
}
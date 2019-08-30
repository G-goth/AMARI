using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace AMARI.Assets.Scripts.Sounds
{
    public class SoundBehaviour : MonoBehaviour
    {
        [SerializeField] private AudioClip[] audioClips;
        [SerializeField] private AudioClip[] finishAudioClips;
        private AudioSource hitAudioSource;
        private AudioSource finishAudioSource;
        private AudioSource[] allocationAudioSources;

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            hitAudioSource = gameObject.GetComponent<AudioSource>();
            allocationAudioSources = gameObject.GetComponents<AudioSource>();

            var hitSound = this.UpdateAsObservable()
                .Select(_ => SelectingSoundEffectCountProp)
                .Where(_ => SelectingSoundEffectCountProp > 0)
                .DistinctUntilChanged()
                .Subscribe(_ => {
                    FiringSoundEffect();
                });

            var finishSound = this.UpdateAsObservable()
                .Select(_ => FinishSoundEffectProp)
                .DistinctUntilChanged()
                .Subscribe(_ => {
                    FinishSOundEffect();
                });
        }

        // 階段状にSEを鳴らす
        public int SelectingSoundEffectCountProp{ get; set; }
        private void FiringSoundEffect()
        {
            hitAudioSource.clip = audioClips[SelectingSoundEffectCountProp - 1];
            hitAudioSource.Play();
        }

        // フィニッシュ音
        public int FinishSoundEffectProp{ get; set; }
        private void FinishSOundEffect()
        {
            if(FinishSoundEffectProp > 2)
            {
                allocationAudioSources[1].clip = finishAudioClips[0];
                allocationAudioSources[1].Play();
            }
        }
    }
}
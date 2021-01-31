using UnityEngine;

namespace Blowing
{
    public class BlowingBreathingEffect : MonoBehaviour
    {
        [SerializeField] 
        private ParticleSystem particleSystem = default;

        [SerializeField] 
        private float maxSpeed = 1f;

        private void Update()
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            if (ExhaleInput.strength <= 0f)
            {
                particleSystem.Stop();
                return;
            }

            if (particleSystem.isPlaying == false)
            {
                particleSystem.Play();
            }
            
            mainModule.startSpeed = ExhaleInput.strength*maxSpeed;
        }
    }
}

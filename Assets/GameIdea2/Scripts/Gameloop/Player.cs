using GameIdea2.Audio;
using UnityEngine;

namespace GameIdea2.Gameloop
{
    public class Player : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            var target = other.collider.GetComponent<Target>();
            if (target)
            {
                GameManager.Instance.ReachedTarget = true;
                AudioManager.instance?.PlaySoundOfType(SoundTyes.Success);
                Destroy(this.gameObject);
            }
        }
    }
}
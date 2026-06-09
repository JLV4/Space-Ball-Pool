using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BallCollisionAudio : MonoBehaviour
{
    public AudioClip collisionSound;
    public float minVolume = 0.1f;
    public float maxVolume = 0.7f;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 0f; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        //check if we collided with another ball OR the player ball
        if (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("PlayerBall"))
        {
            //get the unique ID of this ball and the ball it hit
            //by only playing a sound if our ID is lower guarantee
            //only one of the two balls will make noise
            if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
            {
                //calc volume based on impact velocity
                float impactVelocity = collision.relativeVelocity.magnitude;
                float volume = Mathf.InverseLerp(0, 20, impactVelocity); 
                float finalVolume = Mathf.Lerp(minVolume, maxVolume, volume);

                //randomize pitch for more natural sound
                audioSource.pitch = Random.Range(minPitch, maxPitch);

                //play the sound
                if (collisionSound != null)
                {
                    audioSource.PlayOneShot(collisionSound, finalVolume);
                }
            }
        }
    }
}
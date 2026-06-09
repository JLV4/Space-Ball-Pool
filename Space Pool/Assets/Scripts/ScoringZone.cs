using TMPro;
using UnityEngine;
using System.Collections;

public class ScoringZone : MonoBehaviour
{
    public GameSettings gameSettings;
    public GameObject scoreParticleEffect;

    public Material stripes;
    public Material solids;
    public Material black;

    public TextMeshProUGUI p1Text;
    public TextMeshProUGUI p2Text;
    public TextMeshProUGUI alertText;

    private Color stripesColor;
    private Color solidsColor;

    public Material stripesMaterial;
    public Material solidsMaterial;


    public AudioClip ballSound;
    public AudioSource audioSource;
    public Renderer zoneRenderer;
    public float colorDuration = 4f;
    private Coroutine colorCoroutine;
 

    public void Start()
    {
        // ensure material colors are safe to read
        if (stripesMaterial != null) stripesColor = stripesMaterial.color;
        if (solidsMaterial != null) solidsColor = solidsMaterial.color;

        // initialize AudioSource if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // initialize zoneRenderer if not assigned (fall back to this GameObject's renderer)
        if (zoneRenderer == null)
        {
            zoneRenderer = GetComponent<Renderer>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            // determine if this is the cueball (tag or name heuristic)
            bool isCue = other.gameObject.CompareTag("Cueball") || other.gameObject.name.ToLower().Contains("cue");

            // If NOT a cueball: play sound and flash the scoring zone
            if (!isCue)
            {
                if (audioSource != null && ballSound != null)
                {
                    audioSource.PlayOneShot(ballSound);
                }

                // start flashing: toggle red/green for colorDuration
                if (zoneRenderer != null)
                {
                    if (colorCoroutine != null) StopCoroutine(colorCoroutine);
                    colorCoroutine = StartCoroutine(FlashZoneColor());
                }
            }

            if (scoreParticleEffect != null)
            {
                Instantiate(scoreParticleEffect, other.transform.position, Quaternion.identity);
            }

            var ballName = other.gameObject.name;

            if (gameSettings.p1Stripes == null)
            {
                var ballRenderer = other.gameObject.GetComponent<Renderer>();
                if (ballRenderer != null && ballRenderer.sharedMaterial == stripes)
                {
                    gameSettings.p1Stripes = (gameSettings.currPlayer == 1);
                    alertText.gameObject.SetActive(true);
                    alertText.text = $"Player {((bool)gameSettings.p1Stripes ? 1 : 2)} is Stripes!";

                    gameSettings.p1Score = 0;
                    gameSettings.p2Score = 0;
                    
                    p1Text.color = gameSettings.p1Stripes == true ? stripesColor : solidsColor;
                    p2Text.color = gameSettings.p1Stripes == false ? stripesColor : solidsColor;
                }
                else if (ballRenderer != null && ballRenderer.sharedMaterial == solids)
                {
                    gameSettings.p1Stripes = (gameSettings.currPlayer == 1);
                    alertText.gameObject.SetActive(true);
                    alertText.text = $"Player {((bool)gameSettings.p1Stripes ? 2 : 1)} is Solids!";

                    gameSettings.p1Score = 0;
                    gameSettings.p2Score = 0;

                    p1Text.color = gameSettings.p1Stripes == true ? stripesColor : solidsColor;
                    p2Text.color = gameSettings.p1Stripes == false ? stripesColor : solidsColor;
                }
                else if (ballRenderer != null && ballRenderer.sharedMaterial == black)
                {
                    EndGame();
                }
            }

            var renderer = other.gameObject.GetComponent<Renderer>();
            if (renderer != null && renderer.sharedMaterial == stripes)
            {
                if ((bool)gameSettings.p1Stripes)
                {
                    gameSettings.p1Score += 1;
                    p1Text.text = $"P1 (Stripes): {gameSettings.p1Score}";
                    p2Text.text = $"P2 (Solids): {gameSettings.p2Score}";
                }
                else
                {
                    gameSettings.p2Score += 1;
                    p2Text.text = $"P2 (Stripes): {gameSettings.p2Score}";
                    p1Text.text = $"P1 (Solids): {gameSettings.p1Score}";
                }
            }
            else if (renderer != null && renderer.sharedMaterial == solids)
            {
                if ((bool)gameSettings.p1Stripes)
                {
                    gameSettings.p2Score += 1;
                    p2Text.text = $"P2 (Solids): {gameSettings.p2Score}";
                    p1Text.text = $"P1 (Stripes): {gameSettings.p1Score}";
                }
                else
                {
                    gameSettings.p1Score += 1;
                    p1Text.text = $"P1 (Solids): {gameSettings.p1Score}";
                    p2Text.text = $"P2 (Stripes): {gameSettings.p2Score}";
                }
            }
            else if (renderer != null && renderer.sharedMaterial == black)
            {
                EndGame();
            }
            Destroy(other.gameObject);

            Debug.Log("Ball scored! Add 1 point.");
        }
    }

    // Flash the scoring zone between red and green for colorDuration seconds, then restore original color
    private IEnumerator FlashZoneColor()
    {
        if (zoneRenderer == null) yield break;
        Material mat = zoneRenderer.material;
        Color original = mat.color;
        float elapsed = 0f;
        float flashInterval = 0.5f;
        bool useRed = true;

        while (elapsed < colorDuration)
        {
            mat.color = useRed ? Color.red : Color.green;
            useRed = !useRed;
            float wait = Mathf.Min(flashInterval, colorDuration - elapsed);
            elapsed += wait;
            yield return new WaitForSeconds(wait);
        }

        mat.color = original;
        colorCoroutine = null;
    }

    public void EndGame()
    {
        gameSettings.gameOver = true;
        alertText.gameObject.SetActive(true);
        alertText.text = $"Black Ball Scored! Player {(gameSettings.currPlayer == 0 ? 1 : 0)} Wins!";

        gameSettings.gameOver = false;
        gameSettings.p1Score = 0;
        gameSettings.p2Score = 0;
        gameSettings.currPlayer = 1;
        gameSettings.p1Stripes = null;
    }
}
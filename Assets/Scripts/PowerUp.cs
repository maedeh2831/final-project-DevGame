using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
    public float duration = 5f;

    [Header("Optional Effects")]
    public AudioClip collectSound;

    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        if (other.CompareTag("Ball"))
        {
            isCollected = true;

            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            GameManager.Instance.ActivatePowerUp(powerUpType, duration);

            Destroy(gameObject);
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BallController : MonoBehaviour, IPointerDownHandler
{
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private bool gameStarted = false;

    public GameManager gameManager;
    public AudioSource jumpAudio;

    private HealthManager healthManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        healthManager = FindObjectOfType<HealthManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HandleTap();
    }

    void HandleTap()
    {
        if (!gameStarted)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            gameStarted = true;
        }

        float direction = Random.value > 0.5f ? 1f : -1f;
        float randomX = direction * Random.Range(3f, 6f);

        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(randomX, jumpForce), ForceMode2D.Impulse);

        if (jumpAudio != null)
            jumpAudio.Play();

        if (gameManager != null)
            gameManager.AddScore(1);
    }




    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && healthManager != null)
        {
            healthManager.TakeDamage();
        }
    }
}

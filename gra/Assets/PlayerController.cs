using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    [Header("Health Settings")]
    public int health = 3;
    public float invincibleTimeAfterHurt = 1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("UI Settings")]
    public TextMeshProUGUI healthText;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private bool isInvincible = false;
    private float invincibleTimer = 0f;
    public bool facingRight = true;
    public Animator animator;

    void Start()
    {
        // Pobranie komponentu Rigidbody2D
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Odczyt wej�cia gracza
        moveInput = Input.GetAxis("Horizontal");

        // Sprawdzenie czy gracz stoi na ziemi
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Skakanie
        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        //}
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
        }
        else
        {
            animator.SetBool("IsJumping", true);
        }

        // Obsługa nieśmiertelności
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0f)
            {
                isInvincible = false;
            }
        }
        // Aktualizacja tekstu HP
        UpdateHealthText();

        //Animator
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        float h = Input.GetAxis("Horizontal");
        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        //if (Input.GetButtonDown("Jump"))
        //{
        //    isGrounded = false;
        //    animator.SetBool("IsJumping", true);
        //}
    }

    void FixedUpdate()
    {
        // Ruch w poziomie
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Hurt()
    {
        if (isInvincible) return;

        health--;
        isInvincible = true;
        invincibleTimer = invincibleTimeAfterHurt;

        if (health <= 0)
        {
            // Restart poziomu po śmierci
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        else
        {
        }

        GetComponent<SpriteRenderer>().color = new Color(1f, 0.30196078f, 0.30196078f);

        GetComponent<SpriteRenderer>().color = new Color(0.388235229f, 0.3372549f, 1f);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Patrol patrol = collision.collider.GetComponent<Patrol>();
        if (patrol != null)
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                Debug.Log(point.normal);
                Debug.DrawLine(point.point, point.point + point.normal, Color.red, 10);

                if (point.normal.y >= 0.9f) // Jeśli gracz spada na wroga od góry
                {
                    Vector2 velocity = rb.linearVelocity;
                    velocity.y = jumpForce; // Skok po zniszczeniu wroga
                    rb.linearVelocity = velocity;
                    patrol.Hurt(); // Zranienie wroga
                }
                else // Jeśli gracz zderza się z wrogiem od boku
                {
                    Hurt();
                }
            }
        }

        if (collision.collider.CompareTag("Spike"))
        {
            Hurt(); // Zadanie obrażeń graczowi
            return;
        }


    }

    // Funkcja do aktualizacji liczby żyć na UI
    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + health.ToString();  // Zaktualizuj tekst na ekranie
        }
    }

    void OnDrawGizmosSelected()
    {
        // Wizualizacja obszaru sprawdzania gruntu w edytorze
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

   
}
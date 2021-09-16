using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0414 // Quitar miembros privados no leídos

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private CameraController camController;
    [SerializeField] private Transform weaponTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerStats ps;
    [SerializeField] private BoxCollider2D playerCollider;

    [Header("Fields")]
    [SerializeField] private float dashTime;
    [SerializeField] private float dashSpeedUpperLimit;
    [SerializeField] private float dashDropRate;

    private float dashTimeCounter;
    private float horizontal;
    private float vertical;
    private float fireRateCounter;
    private Vector3 mousePos;
    private bool moving;
    private bool shooting;
    private Vector2 dir;
    private float dashSpeedSmoothed;

    private void Start()
    {
        fireRateCounter = ps.fireRate;
    }

    private void Update()
    {
        fireRateCounter += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashTimeCounter <= 0f)
                Dash();
        }

        if (dashTimeCounter > 0f)
        {
            transform.position += new Vector3(horizontal, vertical) * dashSpeedSmoothed * Time.deltaTime;
            dashTimeCounter -= Time.deltaTime;
            dashSpeedSmoothed -= ps.moveSpeed/dashDropRate;
            dashSpeedSmoothed = Mathf.Clamp(dashSpeedSmoothed, 0f, dashSpeedUpperLimit);
            return;
        }

        Move();

        Rotate();

        if (Input.GetMouseButton(0))
        {
            if (fireRateCounter < ps.fireRate)
                return;

            Shoot();
        }

        if (fireRateCounter >= ps.fireRate)
        {
            shooting = false;
        }
    }

    /// <summary>
    /// Moves the player in horizontal and vertical axis according to input
    /// </summary>
    private void Move()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        
        if (horizontal != 0f || vertical != 0f)
            moving = true;
        else
            moving = false;

        transform.position += new Vector3(horizontal, vertical) * ps.moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Rotates the player so that it faces the mouse position
    /// </summary>
    private void Rotate()
    {
        // Code for constant rotation
        // transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // Code for rotating towards the mouse position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir = mousePos - transform.position;
        weaponTransform.up = dir;

        Animate(dir);
    }

    /// <summary>
    /// Shoots a bullet in the shootPoint direction
    /// </summary>
    private void Shoot()
    {
        shooting = true;
        fireRateCounter = 0f;

        GameObject go = BulletPoolManager.Instance.RequestPlayerBullet();
        go.transform.position = shootPoint.position;
        go.transform.rotation = Quaternion.Euler(shootPoint.rotation.eulerAngles.x, shootPoint.rotation.eulerAngles.y, shootPoint.rotation.eulerAngles.z/* + Random.Range(-10f, 5f)*/);

        // Shooting bullets without Unity Physics System
        Bullet b = go.GetComponent<Bullet>();
        b.SetDir(dir);

        // Shooting bullets with Unity Physics System
        // Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        // rb.AddForce(go.transform.up * bulletSpeed, ForceMode2D.Impulse);

        StartCoroutine(camController.ScreenShake());

        ParticlesManager.Instance.CreateParticle(ParticleType.PlayerShoot, shootPoint.position, 0.5f, shootPoint.rotation);
        SoundManager.Instance.PlaySound(SoundType.PlayerShoot, 1f);
    }

    private void Dash()
    {
        dashTimeCounter = dashTime;
        animator.SetTrigger("Dashing");
        dashSpeedSmoothed = dashSpeedUpperLimit;
    }

    /// <summary>
    /// Animates the player according to where the mouse is pointing at and whether he is moving or not
    /// </summary>
    /// <param name="dir"> Direction where player is looking at </param>
    private void Animate(Vector2 dir)
    {
        animator.SetBool("Moving", moving);
        animator.SetFloat("Horizontal", dir.x);
        animator.SetFloat("Vertical", dir.y);
        //animator.SetBool("Shooting", shooting);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If collided with a powerup, then take it
        if (collision.gameObject.TryGetComponent(out Powerup p))
        {
            p.ApplyPowerup();
        }
    }
}

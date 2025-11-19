using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishMovement : FishBase
{
    [Header("Movement")]
    public float speed = 2f;
    [SerializeField] private float rotationSpeed = 90f;

    [Header("Random Direction Timer")]
    [SerializeField] private float timerUntilNewDirection = 3f;

    private float timer;
    private Rigidbody rb;
    private bool sceneReady = true;

    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Eerste willekeurige richting
        SetNewRandomDirection();

        // Begin met huidige rotatie
        targetRotation = rb.rotation;
    }

    void FixedUpdate()
    {
        if (!sceneReady) return;

        // Smooth rotatie naar targetRotation
        rb.MoveRotation(
            Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            )
        );

        // Vooruit bewegen
        rb.MovePosition(rb.position + rb.transform.forward * speed * Time.fixedDeltaTime);
    }

    void Update()
    {
        UpdateDirectionTimer();
    }

    private void SetNewRandomDirection()
    {
        float yaw = Random.Range(0, 360f);
        float pitch = Random.Range(-45f, 45f);

        targetRotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void UpdateDirectionTimer()
    {
        timer += Time.deltaTime;
        if (timer >= timerUntilNewDirection)
        {
            SetNewRandomDirection();
            timer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Draai weg van de muur
        Vector3 away = (transform.position - collision.contacts[0].point).normalized;
        
        targetRotation = Quaternion.LookRotation(away, Vector3.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        SetNewRandomDirection();
    }
}


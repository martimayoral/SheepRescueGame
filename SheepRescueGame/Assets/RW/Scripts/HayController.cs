using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HayController : MonoBehaviour
{
    public float movementSpeed;
    public float horizontalBoundary = 22;

    public GameObject hayBalePrefab; // 1
    public Transform haySpawnpoint; // 2
    public float shootInterval; // 3
    private float shootTimer; // 4

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UpdateMovement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // 1

        Vector3 translation = transform.right * movementSpeed * Time.deltaTime * horizontalInput;

        if((transform.position + translation).x > -horizontalBoundary &&
            (transform.position + translation).x < horizontalBoundary)
            transform.Translate(translation);

    }
    private void UpdateShooting()
    {
        shootTimer -= Time.deltaTime; // 1

        if (shootTimer <= 0 && Input.GetKey(KeyCode.Space)) // 2
        {
            shootTimer = shootInterval; // 3
            ShootHay(); // 4
        }
    }

    private void ShootHay()
    {
        Instantiate(hayBalePrefab, haySpawnpoint.position, Quaternion.AngleAxis(90,new Vector3(0,1,0)));
    }


    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateShooting();
    }
}

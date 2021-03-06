using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public SheepMode sheepMode;
    public float normalRunSpeed; // 1
    public float angryRunSpeed; // 1
    public float friendlyRunSpeed; // 1
    public MeshRenderer model; // 1
    public Color angryColor; // 2
    public Color friendlyColor; // 3

    private float speed;

    public float gotHayDestroyDelay; // 2
    private bool hitByHay; // 3

    public float dropDestroyDelay; // 1
    private Collider myCollider; // 2
    private Rigidbody myRigidbody; // 3
    private SheepSpawner sheepSpawner;

    public float heartOffset; // 1
    public GameObject heartPrefab; // 2

    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }

    private void OnTriggerEnter(Collider other) // 1
    {
        if (other.CompareTag("Hay") && !hitByHay) // 2
        {
            Destroy(other.gameObject); // 3
            HitByHay(); // 4
        }
        else if (other.CompareTag("DropSheep") && myCollider.isTrigger)
        {
            Drop();
        }
    }
    private void Drop()
    {

        GameStateManager.Instance.DroppedSheep();
        sheepSpawner.RemoveSheepFromList(gameObject);
        myRigidbody.isKinematic = false; // 1
        myCollider.isTrigger = false; // 2


        SoundManager.Instance.PlaySheepDroppedClip();

        Destroy(gameObject, dropDestroyDelay); // 3
    }

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();

        switch (sheepMode)
        {
            case SheepMode.Angry:
                model.material.color = angryColor;
                speed = angryRunSpeed;
                break;
            case SheepMode.Friendly:
                model.material.color = friendlyColor;
                speed = friendlyRunSpeed;
                break;
            default:
                speed = normalRunSpeed;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void HitByHay()
    {
        sheepSpawner.RemoveSheepFromList(gameObject);
        hitByHay = true; // 1
        speed = 0; // 2
        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
       
        TweenScale tweenScale = gameObject.AddComponent<TweenScale>(); ; // 1
        tweenScale.targetScale = 0; // 2
        tweenScale.timeToReachTarget = gotHayDestroyDelay; // 3

        GameStateManager.Instance.SavedSheep();

        SoundManager.Instance.PlaySheepHitClip();

        Destroy(gameObject, gotHayDestroyDelay); // 3
    }

}

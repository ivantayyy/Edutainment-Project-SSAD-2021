using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject Shoot;
    public GameObject ShootingPoint;
    private float bulletSpeed = 10.0f;
    private Vector3 target;
    private Camera cam;
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        target = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                  Input.mousePosition.y, transform.position.z));


        Vector3 difference = target - player.transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        float distance = difference.magnitude;
        Vector2 direction = difference / distance;
        moveAim(direction, rotationZ);
        direction.Normalize();

        if (Input.GetMouseButtonDown(0))
        {
           
            fireBullet(direction, rotationZ - 90f);
        }
    }
    void fireBullet(Vector2 direction, float rotationZ)
    {
        
        GameObject b = Instantiate(bulletPrefab) as GameObject;

        b.transform.position = ShootingPoint.transform.position;
        b.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        b.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

    }
    void moveAim(Vector2 direction, float rotationZ)
    {
        Shoot.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }
}

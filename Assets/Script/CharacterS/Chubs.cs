using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets
{
    public class Chubs : Player
    {
        public GameObject player;
        public GameObject bulletPrefab;
        public GameObject Shoot;
        public GameObject ShootingPoint;
        private float bulletSpeed = 10.0f;
        public Camera camera;
        private Vector3 target;





        public override void Awake()
        {
            base.Awake();

        }

        public override void Start()
        {
            base.Start();
            camera = playerCamera.GetComponent<Camera>();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            target = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                     Input.mousePosition.y, transform.position.z));


            Vector3 difference = target - player.transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            float distance = difference.magnitude;
            Vector2 direction = difference / distance;
            moveAim(direction, rotationZ);
            direction.Normalize();

            if (Input.GetMouseButtonDown(0) && photonView.isMine)
            {

                fireBullet(direction, rotationZ - 90f);
            }

        }
        void fireBullet(Vector2 direction, float rotationZ)
        {



            GameObject b = PhotonNetwork.Instantiate(bulletPrefab.name, ShootingPoint.transform.position, Quaternion.Euler(0.0f, 0.0f, rotationZ), 0);

            b.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        }
        void moveAim(Vector2 direction, float rotationZ)
        {
            Shoot.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        }
    }

}





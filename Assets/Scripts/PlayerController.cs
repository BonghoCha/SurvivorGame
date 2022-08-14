using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody _rigidbody;

    [SerializeField] GameObject _missilePrefab;
    [SerializeField] Transform _aimObject;
    [SerializeField] Transform _shootPosition;

    [SerializeField] float speed = 10f;
    [SerializeField] float missileSpeed = 1000f;

    [SerializeField] Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Physics.gravity = new Vector3(0, 0, -9.81f);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal != 0 || vertical != 0)
        {
            direction = new Vector3(horizontal, vertical, 0).normalized;
        }

        if (direction.magnitude >= 0.1f)
        {
            var dir = (direction * Time.deltaTime * speed);
            _rigidbody.velocity = dir;

            // 방향에 맞춰서 돌아보게끔
            var rot = Quaternion.LookRotation(Vector3.forward, dir);
            _aimObject.rotation = rot;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject goMissile = Instantiate(_missilePrefab, _shootPosition.position, Quaternion.identity);

        goMissile.GetComponent<Rigidbody>().AddForce(direction * missileSpeed);
    }
}

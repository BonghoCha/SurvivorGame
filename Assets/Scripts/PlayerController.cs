using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool _init = false;

    Rigidbody _rigidbody;

    [SerializeField] Joystick _joystick;

    [SerializeField] GameObject _missilePrefab;
    [SerializeField] Transform _aimObject;
    [SerializeField] ParticleSystem _aimParticle;
    [SerializeField] Transform _shootPosition;

    [SerializeField] float speed = 10f;
    [SerializeField] float missileSpeed = 1000f;

    [SerializeField] Vector3 direction;

    public void SetSpeed(float num)
    {
        if (speed >= 2000f) return;

        speed += num;
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Physics.gravity = new Vector3(0, 0, -9.81f);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = _joystick.Horizontal;//Input.GetAxisRaw("Horizontal");
        float vertical = _joystick.Vertical;//Input.GetAxisRaw("Vertical");

        if (!horizontal.Equals(0) && !vertical.Equals(0))
        {
            direction = new Vector3(horizontal, vertical, 0).normalized;

            if (direction.magnitude >= 0.1f)
            {
                if (!_init)
                {
                    _aimParticle.Play();
                    _init = true;
                }
                var dir = (direction * Time.deltaTime * speed);
                _rigidbody.velocity = dir;

                // 방향에 맞춰서 돌아보게끔
                var rot = Quaternion.LookRotation(Vector3.forward, dir);
                _aimObject.rotation = rot;
            }
        } else
        {
            _rigidbody.velocity = Vector3.zero;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (!_init) return;

        GameObject goMissile = Instantiate(_missilePrefab, _shootPosition.position, Quaternion.identity);

        goMissile.GetComponent<Rigidbody>().AddForce(direction * missileSpeed);
    }
}

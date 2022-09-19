using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region ### Player Info ###
    [Header("Player Info")]
    [SerializeField] float _power = 10f;
    [SerializeField] float _speed = 10f;
    [SerializeField] float _critical = 10f;
    [SerializeField] float _firerate = 10f;
    #endregion

    [Header("Public Preperties")]
    Rigidbody _rigidbody;
    Collider _collider;

    #region Joystick
    [SerializeField] Joystick _joystick;
    #endregion

    #region ### Missile Info ###
    [Header("Missile Info")]
    [SerializeField] MissileType _missileType;
    MissileType missileType {
        get
        {
            return _missileType;
        }
        set
        {
            missileType = value;

            missileSpeed = MissileManager.instance.missileInfo[(int)missileType].speed;
        }
    }
    
    [SerializeField] GameObject _missilePrefab;
    [SerializeField] Transform _aimObject;
    [SerializeField] ParticleSystem _aimParticle;
    [SerializeField] Transform _shootPosition;

    [SerializeField] float missileSpeed = 1000f;
    #endregion

    #region ### Game Preperties ###
    bool _init = false;                  // 캐릭터를 움직였는지 여부
    Vector3 _direction;                  // 플레이어가 향한 방향
    float _initialSpeed;                 // 캐릭터 스피드 (백업용)
    #endregion

    #region ### Actions ####
    public Action onShot;
    public Action onDash;
    #endregion

    #region ### UI Buttons ###
    [SerializeField] ButtonController[] buttons;
    #endregion

    public void SetSpeed(float num)
    {
        if (_speed >= 2000f) return;

        _speed += num;
    }

    void Init()
    {
        _power = PlayerInfo.Power;
        _speed = PlayerInfo.Speed;
        _critical = PlayerInfo.Critical;
        _firerate = PlayerInfo.Firerate;

        _initialSpeed = _speed;

        // 버튼 액션 동기화
        StartCoroutine( SynchronizedOnAction() );
    }

    void Save()
    {
        PlayerInfo.Power = _power;
        PlayerInfo.Speed = _speed;
        PlayerInfo.Critical = _critical;
        PlayerInfo.Firerate = _firerate;
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();

        Physics.gravity = new Vector3(0, 0, -9.81f);
    }

    private void OnEnable()
    {
        Init();
    }

    float timeLimit = 5f;
    IEnumerator SynchronizedOnAction()
    {
        float time = 0f;
        while (time < timeLimit)
        {
            for (int i = 0; i < buttons.Length; i++)
            {                
                switch (buttons[i].type)
                {
                    case ButtonController.ButtonType.Shot:
                        {
                            if (onShot == null) {
                                onShot += buttons[i].onClick;
                            }
                            break;
                        }
                    case ButtonController.ButtonType.Dash:
                        {
                            if (onDash == null)
                            {
                                onDash += buttons[i].onClick;
                            }
                            break;
                        }
                }
            }
            if (onShot != null && onDash != null) break;

            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        onShot += Shot;
        onDash += Dash;
    }

    private void OnDisable()
    {
        Save();

        for (int i = 0; i < buttons.Length; i++)
        {
            switch (buttons[i].type)
            {
                case ButtonController.ButtonType.Shot:
                    {
                        onShot -= buttons[i].onClick;
                        break;
                    }
                case ButtonController.ButtonType.Dash:
                    {
                        onDash -= buttons[i].onClick;
                        break;
                    }
            }
        }
        if (onShot != null)
        {
            onShot -= Shot;
        }

        if (onDash != null)
        {
            onDash -= Dash;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnShot();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            OnDash();
        }
    }

    void FixedUpdate()
    {
        float horizontal = _joystick.Horizontal;//Input.GetAxisRaw("Horizontal");
        float vertical = _joystick.Vertical;//Input.GetAxisRaw("Vertical");

        if (!horizontal.Equals(0) && !vertical.Equals(0))
        {
            _direction = new Vector3(horizontal, vertical, 0).normalized;

            if (_direction.magnitude >= 0.1f)
            {
                if (!_init)
                {
                    _aimParticle.Play();
                    _init = true;
                }
                var dir = (_direction * Time.deltaTime * _speed);
                _rigidbody.velocity = dir;

                // 방향에 맞춰서 돌아보게끔
                var rot = Quaternion.LookRotation(Vector3.forward, dir);
                _aimObject.rotation = rot;
            }
        } else
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    public void OnShot()
    {
        if (onShot != null)
        {
            onShot();            
        }
    }

    public void OnDash()
    {
        if (onShot != null)
        {
            onDash();
        }
    }

    void Shot()
    {
        if (!_init) return;
        if (buttons[(int)ButtonController.ButtonType.Shot].isCooltime) return;

        GameObject goMissile = MissileManager.instance.GetMissile(missileType);
        goMissile.transform.SetPositionAndRotation(_shootPosition.position, Quaternion.identity);

        goMissile.GetComponent<Rigidbody>().AddForce(_direction * missileSpeed);
    }

    void Dash()
    {
        if (!_init) return;
        if (buttons[(int)ButtonController.ButtonType.Dash].isCooltime) return;

        Sequence sequence = DOTween.Sequence();
        sequence.OnStart(() =>
        {
            _collider.enabled = false;
        });
        sequence.Append(DOTween.To(() => _speed, _ => _speed = _, 1000f, 0.125f));
        sequence.Append(DOTween.To(() => _speed, _ => _speed = _, _initialSpeed, 0.125f));
        sequence.OnComplete(() =>
        {
            _collider.enabled = true;
        });

        sequence.Play();
    }
}

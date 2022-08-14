using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform Player;

    Vector3 _spawnPosition;
    [SerializeField] float speed = 10f;

    Collider _collider;

    bool _isAlive = true;

    SpriteRenderer _sprite;
    [SerializeField] ParticleSystem _dieParticle;

    int heart = 5;

    public void Damage()
    {
        if (heart > 0)
        {
            heart--;
            if (heart <= 0) { 
                heart = -1;
                _isAlive = false;
                _collider.enabled = false;

                Die();
            }
        }
    }

    public void Die()
    {
        GameManager.instance.GetScore();        

        _dieParticle.Play();
        _sprite.DOFade(0f, 1f).OnComplete(Initialize);
    }

    void Initialize()
    {
        Debug.Log("이니셜라이즈");
        _isAlive = true;

        heart = 5;
        transform.position = _spawnPosition;

        _sprite.color = new Color(255, 255, 255, 1);

        _collider.enabled = true;

        speed = Random.Range(1f, 2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_sprite == null) _sprite = GetComponent<SpriteRenderer>();
        if (_collider == null) _collider = GetComponent<Collider>();

        _spawnPosition = transform.position;

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAlive)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.position, Time.deltaTime*speed);
        }
    }
}

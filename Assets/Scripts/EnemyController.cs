using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : ObjectManager
{
    [SerializeField] Transform Player;

    Vector3 _spawnPosition;
    [SerializeField] float speed = 10f;

    bool _isAlive = true;

    public override void OnDestroyObject()
    {

    }

    public override void Damage(float damage, bool isCritical = false)
    {
        GameManager.instance.SetDamage(damage, transform, isCritical);

        if (hp > 0)
        {
            hp -= damage;
            if (hp <= 0) {
                hp = -1;
                _isAlive = false;
                _collider.enabled = false;

                Die();
            }
        }
    }

    public void Die()
    {
        GameManager.instance.GetEXP();

        PlayParticle();
        _sprite.DOFade(0f, 1f).OnComplete(Initialize);
    }


    void Initialize()
    {
        _isAlive = true;

        hp = 50;
        transform.position = _spawnPosition;

        _sprite.color = new Color(255, 255, 255, 1);

        _collider.enabled = true;

        speed = Random.Range(1f, 2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        _spawnPosition = transform.position;

        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canMove) return;

        if (_isAlive)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.position, Time.deltaTime*speed);
        }
    }
}

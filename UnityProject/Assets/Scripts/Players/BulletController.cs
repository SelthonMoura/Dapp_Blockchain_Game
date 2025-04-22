using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    private int _damage;
    private Rigidbody2D _rb;
    private Animator _anim;
    private Collider2D _col;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _col = GetComponent<Collider2D>();

        StartCoroutine(BulletLifetime());
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public int GetBulletDamage()
    {
        return _damage;
    }

    public void PlayPopAnimation()
    {
        _rb.velocity = Vector3.zero;
        _col.enabled = false;
        _anim.SetTrigger("Pop");
    }
    private IEnumerator BulletLifetime()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }
}

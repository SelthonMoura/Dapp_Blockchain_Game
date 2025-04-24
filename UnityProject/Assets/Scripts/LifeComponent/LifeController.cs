using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Unity.Netcode;
using UnityEngine.UI;
using static EventManager;
public class LifeController : NetworkBehaviour
{
    [SerializeField] private int _maxLife;
    [SerializeField] private Image _healthBar;
    private bool _isDead;
    public event Action OnDeath;

    private NetworkVariable<int> _currentLife = new NetworkVariable<int>(
     0,
     NetworkVariableReadPermission.Everyone,
     NetworkVariableWritePermission.Server);

    private void Start()
    {
        _currentLife.OnValueChanged += UpdateHealth;

    }

    public override void OnDestroy()
    {
        _currentLife.OnValueChanged -= UpdateHealth;

    }

    public override void OnNetworkSpawn()
    {
        _isDead = false;
        _currentLife.Value = _maxLife;
    }
   
    public void GetDamage(int damage)
    {
        if (_isDead) return;

        if ((_currentLife.Value - damage) > 0)
        {
            _currentLife.Value -= damage;
        }
        else
        {
            _isDead = true;
            OnDeath?.Invoke();
        }
    }

    private void UpdateHealth(int previousValue, int newValue)
    {
        _healthBar.fillAmount = _currentLife.Value / (float)_maxLife;
    }
}

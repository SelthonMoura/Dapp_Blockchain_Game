using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private GameOverController _gameOverController;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private int _maxLife;
    private int _currentLife;
    private bool _isPlayerOne;

    private void Start()
    {
        if(_playerController.GetControlScheme() == PlayerController.ControlScheme.WASD)
            _isPlayerOne = true;
        else
            _isPlayerOne = false;
        
        _currentLife = _maxLife;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet_2" && _isPlayerOne ||
            collision.gameObject.tag == "Bullet_1" && !_isPlayerOne)
        {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            bullet.PlayPopAnimation();

            _currentLife -= bullet.GetBulletDamage();

            if ( _currentLife <= 0)
            {
                // game over
            }

            if (_isPlayerOne)
                _gameOverController.playerTwoScore += 1;
            else
                _gameOverController.playerOneScore += 1;

            EventManager.OnUpdateUITrigger();
        }
    }
}

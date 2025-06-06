using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private PlayerDataSO _playerData;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Animator _anim;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private LifeController _lifeController;
    [SerializeField] private SpriteRenderer _spriteRenderer;
     
    private NetworkVariable<Vector2> networkInputVector = new NetworkVariable<Vector2>(
        Vector2.zero,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner);

    private void Awake()
    {
        _lifeController.OnDeath += PerformDeathSequence;
    }

    private void OnDestroy()
    {
        _lifeController.OnDeath -= PerformDeathSequence;
    }

    private void Update()
    {
        if (IsOwner)
        {
            Vector2 inputVector = _playerInput.GetMovementVector();

            if (networkInputVector.Value != inputVector)
            {
                networkInputVector.Value = inputVector;
            }
        }

        InterpretMovement();
        _anim.SetBool("IsMoving", networkInputVector.Value != Vector2.zero);
    }

    private void InterpretMovement()
    {
        Vector2 inputVector = networkInputVector.Value;
        Vector3 movement = inputVector * Time.deltaTime * _moveSpeed;
        _spriteRenderer.flipX = inputVector.x > 0;
        transform.Translate(movement);
    }

    public void PerformDeathSequence()
    {
        EventManager.OnPlayerDeathTrigger();

        if (IsServer)
        {
            ulong clientId = OwnerClientId;
            PlayerRespawner.Instance.RespawnPlayer(clientId);
        }

        Destroy(gameObject);
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private BlockchainClient _blockchainClient;
    [SerializeField] private List<GunDetail> _gunDetailList;
    [SerializeField] private Transform _shopItemParent;
    [SerializeField] private Transform _inventoryParent;
    [SerializeField] private ButtonItem _shopItemPrefab;
    [SerializeField] private InventoryButton _inventoryPrefab;
    public GameObject shopPanel;

    private void Awake()
    {
        EventManager.OnSendTransactionEvent += SendTransaction;
        EventManager.OnReceiveBlockchainEvent += UpdateStore;
        StartCoroutine(_blockchainClient.GetBlockchain());
    }

    private void OnDestroy()
    {
        EventManager.OnSendTransactionEvent -= SendTransaction;
        EventManager.OnReceiveBlockchainEvent -= UpdateStore;
    }

    private void SendTransaction(string item, float amount)
    {
        if(DataHolder.Instance.runtimePlayerDataSO.playerData >= (int)amount)
        {
            if (_blockchainClient.SendTransaction(item, amount))
                DataHolder.Instance.runtimePlayerDataSO.playerData -= (int)amount;
        }
    }

    private void UpdateStore(string blockchainJson)
    {
        // Limpa os itens atuais da loja e inventário
        foreach (Transform child in _shopItemParent)
            Destroy(child.gameObject);
        foreach (Transform child in _inventoryParent)
            Destroy(child.gameObject);

        HashSet<string> ownedItems = new HashSet<string>();

        // Pega a chave do jogador
        string playerKey = _blockchainClient.apiUrl;

        // Desserializa a blockchain
        BlockchainBlock[] blocks = JsonHelper.FromJson<BlockchainBlock>(blockchainJson);

        // Coleta todos os itens que o jogador comprou
        foreach (var block in blocks)
        {
            foreach (var tx in block.transactions)
            {
                if (tx.player == playerKey)
                {
                    ownedItems.Add(tx.item);
                }
            }
        }

        // Agora instancia os itens em cada painel
        foreach (var gun in _gunDetailList)
        {
            if (ownedItems.Contains(gun.gunName))
            {
                // Adiciona ao inventário
                var btn = Instantiate(_inventoryPrefab, _inventoryParent);
                btn.Setup(gun, _gunDetailList.FindIndex(o=>o.Equals(gun))); // Assumindo que você tenha um método Setup(GunDetail)
            }
            else
            {
                // Adiciona à loja
                var btn = Instantiate(_shopItemPrefab, _shopItemParent);
                btn.Setup(gun); // idem
            }
        }
    }

    public void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CloseShop()
    {
        Time.timeScale = 1.0f;
        shopPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        _settingsPanel.SetActive(true);
    }
}

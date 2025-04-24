using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUIController : MonoBehaviour
{

    [SerializeField]
    private Button _hostbutton;


    [SerializeField]
    private Button _clientbutton;


    [SerializeField]
    private Button _serverbutton;

    [SerializeField] private GameObject _gameplayPanel;

    private void Awake()
    {
        _hostbutton.onClick.AddListener(StartHost);
        _clientbutton.onClick.AddListener(StartClient);
        _serverbutton.onClick.AddListener(StartServer);
    }

    private void StartServer()
    {
        NetworkManager.Singleton.StartServer();
        DeactivateButtons();
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        DeactivateButtons();
    }

    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        DeactivateButtons();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        _hostbutton.onClick.RemoveListener(StartHost);
        _clientbutton.onClick.RemoveListener(StartClient);
        _serverbutton.onClick.RemoveListener(StartServer);
    }

    private void DeactivateButtons()
    {
        _hostbutton.gameObject.SetActive(false);
        _clientbutton.gameObject.SetActive(false);
        _serverbutton.gameObject.SetActive(false);

        _gameplayPanel.SetActive(true);
    }
}

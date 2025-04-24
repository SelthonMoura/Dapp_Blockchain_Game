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


    private void Awake()
    {
        _hostbutton.onClick.AddListener(StartHost);
        _clientbutton.onClick.AddListener(StartClient);
        _serverbutton.onClick.AddListener(StartServer);
    }

    private void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    private void StartHost()
    {
        NetworkManager.Singleton.StartHost(); 
    }

    private void StartClient()
    {
        NetworkManager.Singleton.StartClient();
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
}

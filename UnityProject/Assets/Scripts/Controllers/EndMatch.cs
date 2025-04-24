using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMatch : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "MainMenu";

    public void OnReturnToMenuPressed()
    {
        StartCoroutine(ShutdownAndReturn());
    }

    private IEnumerator ShutdownAndReturn()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(menuSceneName);
        yield return new WaitForEndOfFrame();

        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
        {
            NetworkManager.Singleton.Shutdown();
            AuthenticationService.Instance.SignOut();
            /*
            if (NetworkManager.Singleton != null)
            {
                Destroy(NetworkManager.Singleton.gameObject);
            }
            */
        }

        yield return null;

        if (DataHolder.Instance != null)
        {
            Destroy(DataHolder.Instance.gameObject);
        }
    }

}

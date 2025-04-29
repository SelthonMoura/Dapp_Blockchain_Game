using System.Collections;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Authentication;
using UnityEngine.SceneManagement;

public class EndMatch : MonoBehaviour
{
    [SerializeField] private string menuSceneName = "MenuScene";

    public void OnReturnToMenuPressed()
    {
        StartCoroutine(ResetGame());
    }

    private IEnumerator ResetGame()
    {
        Time.timeScale = 1.0f;

        if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("1");
        }

        // Step 1: Sign out
        if (AuthenticationService.Instance.IsSignedIn)
        {
            AuthenticationService.Instance.SignOut();
        }


        if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("2");
        }


        // Step 2: Shutdown NetworkManager immediately
        if (NetworkManager.Singleton != null)
            {
                if (NetworkManager.Singleton.IsListening || NetworkManager.Singleton.IsClient)
                {
                    NetworkManager.Singleton.Shutdown();
                }

                //Destroy(NetworkManager.Singleton.gameObject);
            }


        if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("3");
        }



        /*
        // Step 3: Destroy custom singletons (like DataHolder)
        if (DataHolder.Instance != null)
        {
            Destroy(DataHolder.Instance.gameObject);
        }
        */

        // Step 4: Small delay to allow Unity to finalize object destruction
        SceneManager.LoadScene(menuSceneName);
        yield return new WaitForSecondsRealtime(0.2f);

        // Step 5: Load Menu Scene
        
    }
}

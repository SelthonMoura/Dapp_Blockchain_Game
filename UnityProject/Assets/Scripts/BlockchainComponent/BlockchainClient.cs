using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static UnityEditor.Progress;
using static Cinemachine.CinemachineOrbitalTransposer;

public class BlockchainClient : MonoBehaviour
{
    public int port = 8000;
    public string apiUrl = "http://localhost:8000"; // ou 8001, 8002...
    private KeyPair keyPair;
    private bool sending = false;

    [System.Serializable] public class KeyPair { public string private_key; public string public_key; }
    [System.Serializable] public class SignatureRequest { public string tx_data; public string private_key; }
    [System.Serializable] public class SignatureResponse { public string signature; }
    [System.Serializable]
    public class TransactionData
    {
        public string player;
        public string item;
        public float amount;
        public string signature;
        public string public_key;
    }

    public void Start()
    {
        apiUrl = "http://localhost:" + port.ToString();
    }

    public bool SendTransaction(string item, float amount)
    {
        if (!sending)
        {
            sending = true;
            StartCoroutine(ExecuteTransactionAndMine(item, amount));
            return true;
        }
        return false;
    }

    public IEnumerator GetBlockchain()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl + "/chain"))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erro ao buscar blockchain: " + request.error);
            }
            else
            {
                Debug.Log("Blockchain: " + request.downloadHandler.text);
                EventManager.OnReceiveBlockchainTrigger(request.downloadHandler.text);
            }
        }
    }


    private IEnumerator ExecuteTransactionAndMine(string item, float amount)
    {
        // 1. Verifica se já temos chave privada

        if (keyPair==null)
        {
            keyPair = new KeyPair();
            Debug.Log("Gerando novas chaves...");
            UnityWebRequest keyReq = UnityWebRequest.Get(apiUrl + "/generate_keys");
            yield return keyReq.SendWebRequest();

            if (keyReq.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erro ao gerar chaves: " + keyReq.error);
                yield break;
            }

            var keys = JsonUtility.FromJson<KeyPair>(keyReq.downloadHandler.text);
            keyPair.private_key = keys.private_key;
            keyPair.public_key = keys.public_key;
        }

        // 2. Monta a transação (sem assinatura ainda)
        string formattedAmount = amount.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
        var txJson = "{\"amount\":\"" + formattedAmount + "\",\"item\":\"" + item + "\",\"player\":\"" + apiUrl + "\"}";

        // 3. Solicita assinatura
        SignatureRequest signRequest = new SignatureRequest { tx_data = txJson, private_key = keyPair.private_key };
        string signJson = JsonUtility.ToJson(signRequest);

        UnityWebRequest signReq = new UnityWebRequest(apiUrl + "/sign", "POST");
        byte[] signBody = Encoding.UTF8.GetBytes(signJson);
        signReq.uploadHandler = new UploadHandlerRaw(signBody);
        signReq.downloadHandler = new DownloadHandlerBuffer();
        signReq.SetRequestHeader("Content-Type", "application/json");

        yield return signReq.SendWebRequest();

        if (signReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao assinar transação: " + signReq.error);
            yield break;
        }

        string signature = JsonUtility.FromJson<SignatureResponse>(signReq.downloadHandler.text).signature;

        // 4. Envia a transação
        TransactionData tx = new TransactionData
        {
            player = apiUrl,
            item = item,
            amount = amount,
            signature = signature,
            public_key = keyPair.public_key
        };

        string txJsonFinal = JsonUtility.ToJson(tx);
        UnityWebRequest txReq = new UnityWebRequest(apiUrl + "/transaction", "POST");
        txReq.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(txJsonFinal));
        txReq.downloadHandler = new DownloadHandlerBuffer();
        txReq.SetRequestHeader("Content-Type", "application/json");

        yield return txReq.SendWebRequest();

        if (txReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao enviar transação: " + txReq.error);
            yield break;
        }

        Debug.Log("Transação enviada!");
        Debug.Log(txReq.downloadHandler.text);

        // 5. Minerar
        UnityWebRequest mineReq = UnityWebRequest.PostWwwForm(apiUrl + "/mine", "");
        yield return mineReq.SendWebRequest();

        if (mineReq.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro ao minerar: " + mineReq.error);
        }
        else
        {
            Debug.Log("Bloco minerado com sucesso!");
            Debug.Log(mineReq.downloadHandler.text);
        }
        sending = false;

        StartCoroutine(GetBlockchain());
    }
}

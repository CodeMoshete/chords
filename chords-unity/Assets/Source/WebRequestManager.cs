using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestManager : MonoBehaviour
{
    private const string BASE_URI = "http://codemoshete.com/chords/web";
    private const string LOAD_ENDPOINT = "/loadjson.php";
    private const string SAVE_ENDPOINT = "/savejson.php";

    public static WebRequestManager Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        Debug.Log("Web Requests ready");
    }

    public void GetData<T>(Action<T> OnFinished)
    {
        StartCoroutine(GetDataRequest<T>(OnFinished));
    }

    private IEnumerator GetDataRequest<T>(Action<T> OnFinished)
    {
        string uri = BASE_URI + LOAD_ENDPOINT;

        // Debug.Log("GETting layouts from: " + uri);
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
#if UNITY_EDITOR
            www.SetRequestHeader("Accept", "*/*");
            www.SetRequestHeader("Accept-Encoding", "gzip, deflate");
            www.SetRequestHeader("User-Agent", "runscope/0.1");
#endif
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.LogError(www.error);
            }

            if (OnFinished != null)
            {
                string responseText = www.downloadHandler.text;
                // Debug.Log("RESPONSE: " + responseText);
                T response = JsonUtility.FromJson<T>(responseText);
                OnFinished.Invoke(response);
            }
        }
    }

    public void PostData(string venuesData, Action OnFinished)
    {
        StartCoroutine(PostNewData(venuesData, OnFinished));
    }

    private IEnumerator PostNewData(string data, Action OnFinished)
    {
        string uri = BASE_URI + SAVE_ENDPOINT;

        // Debug.Log("POSTing data update to: " + uri);
        using (UnityWebRequest www = new UnityWebRequest(uri))
        {
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(data));
            www.downloadHandler = new DownloadHandlerBuffer();
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");
#if UNITY_EDITOR
            www.SetRequestHeader("Accept", "*/*");
            www.SetRequestHeader("Accept-Encoding", "gzip, deflate");
            www.SetRequestHeader("User-Agent", "runscope/0.1");
#endif

            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.LogError(www.error);
            }

            if (OnFinished != null)
            {
                OnFinished();
            }
        }
    }
}
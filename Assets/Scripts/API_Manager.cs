using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class API_Manager : MonoBehaviour
{
    private string JsonString;
    private string APILink;
    public MonitorManager monitorManager;
    // public VideoLoader videoLoader;
    private bool isRenderAwake;

    //public ImagePrompt imagePrompt;
    // Start is called before the first frame update
    void Start()
    {
        APILink = "https://backend-server-tqhm.onrender.com";
        // APILink = "https://pyflask-re8t.onrender.com";

        StartCoroutine(WakeUpRender());

    }

    IEnumerator WakeUpRender()
    {
        Debug.Log("Checking if Render is awake..");
        using UnityWebRequest webRequest = UnityWebRequest.Get(APILink);

        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log(webRequest.error);
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log(webRequest.error);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log(webRequest.error);
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log("Server Awakened");
                isRenderAwake = true;
                break;
        }
        if (isRenderAwake) GetAllImages();
    }

    public void GetAllImages()
    {
        string uri = APILink + "/readall"; //building url
        StartCoroutine(GetAllImagesRequest(uri));
    }
   
    public void RequestImage(string prompt)
    {
      
        string uri = APILink + "/robin/" + prompt; //building url
        StartCoroutine(GetRobinAPIRequest(uri));
    }

    public void RequestVideo(string prompt, Monitor monitor) {
        string uri2 = APILink + "/video/" + prompt;
        StartCoroutine(GetVideoAPIRequest(uri2, monitor));
    }


    IEnumerator GetVideoAPIRequest(string uri, Monitor monitor)
    {
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("Connection Error with Video API Request");
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Data Processing Error with  Video API Request");
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Protocol Error with Video API Request, check openai and pexels key");
                monitor.HideVideo();
                break;
            case UnityWebRequest.Result.Success:
               // { "urls": "https: pexels.com/."}
                string videoJsonString = webRequest.downloadHandler.text; //json string
                //Imagejson json = Imagejson.CreateFromJSON(JsonString); //json object
                //json.printInfo();

                var results = JsonConvert.DeserializeObject<UrlsJson>(videoJsonString);
                string videoLink = results.urls[0];
                monitor.loader.SetNewVideoURL(videoLink);
                monitor.ShowVideo();

                Debug.Log("Video Request JSON" + videoJsonString);
                break;
        }
    }
    IEnumerator GetRobinAPIRequest(string uri)
    {
        //imagePrompt.StartLoadingText();
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();
      
        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("Connection Error");
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Data Processing Error with Robin API Request");
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Protocol Error with Robin API Request");
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(":\nDatabase Data Received: " + webRequest.downloadHandler.text);

                JsonString = webRequest.downloadHandler.text; //json string
                Artifact newImage = JsonConvert.DeserializeObject<Artifact>(JsonString);

                monitorManager.AddArtifact(newImage);
                break;
        }
    }

    IEnumerator GetAllImagesRequest(string uri)
    {
        //imagePrompt.StartLoadingText();
        using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
        // Request and wait for the desired page.
        yield return webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.ConnectionError:
            case UnityWebRequest.Result.DataProcessingError:
                break;
            case UnityWebRequest.Result.ProtocolError:
                break;
            case UnityWebRequest.Result.Success:
                Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
                JsonString = webRequest.downloadHandler.text; //json string

                var results = JsonConvert.DeserializeObject<Root>(JsonString);
                //var result = JsonConvert.DeserializeObject<Artifact[]>(JsonString);

                monitorManager.artifacts = results.artifacts;

                int lastIndex = monitorManager.artifacts.Count - 1;

                monitorManager.left.SetArtifact(monitorManager.artifacts[lastIndex - 2], lastIndex-2);
                monitorManager.middle.SetArtifact(monitorManager.artifacts[lastIndex - 1], lastIndex-1);
                monitorManager.right.SetArtifact(monitorManager.artifacts[lastIndex], lastIndex);
                break;
        }

    }

}

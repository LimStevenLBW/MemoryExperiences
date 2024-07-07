using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class Monitor : MonoBehaviour
{
    private float speed = 300;
    private MeshRenderer meshRenderer;
    public int index { get; set; }

    public MonitorCover cover;
    //public ImagePrompt imagePrompt;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void MoveTo(Vector3 position)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(position));
    }

    public void MoveTo(Vector3 position1, Vector3 position2)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPositions(position1, position2));
    }
    
    IEnumerator MoveToPosition(Vector3 position)
    {
        while (Vector3.Distance(transform.position, position) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoveToPositions(Vector3 position1, Vector3 position2)
    {
        while (Vector3.Distance(transform.position, position1) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position1, speed*2.1f * Time.deltaTime);
            yield return null;
        }

        // Check if the position of the cube and sphere are approximately equal.
        while (Vector3.Distance(transform.position, position2) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position2, speed*2.1f * Time.deltaTime);
            yield return null;
        }
    }



    public void setImage(string url)
    {
        StartCoroutine(DownloadImage(url));
    }
    public void setImage(string url, int index)
    {
        this.index = index;
        StartCoroutine(DownloadImage(url));
    }

    IEnumerator DownloadImage(string MediaUrl) //download image and make it to the material
    {
        cover.StartLoadingText();
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        //imagePrompt.StopLoadingText();

        if (request.isNetworkError || request.isHttpError)
        { 
            Debug.Log(request.error);
            cover.StopLoadingText();
        }
        else
        {
            //Texture.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            //texture=SetTexture("Image", ((DownloadHandlerTexture)request.downloadHandler).texture);

            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            var mat = new Material(Shader.Find("UI/Default"));
            mat.mainTexture = myTexture;
            meshRenderer.material.mainTexture = myTexture;
            cover.StopLoadingText();
        }
    }   
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class Monitor : MonoBehaviour
{
    private float speed = 300;
    private MeshRenderer meshRenderer;
    public int index { get; set; }

    public MonitorCover cover;

    public TextMeshPro text;
    //public ImagePrompt imagePrompt;

    private bool moving;
    private bool downloadingImage;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void MoveTo(Vector3 position, Vector3 rotation)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(position, rotation));
    }

    public void MoveTo(Vector3 position1, Vector3 rotation1, Vector3 position2, Vector3 rotation2)
    {
        StopAllCoroutines();
        StartCoroutine(MoveToPositions(position1, rotation1, position2, rotation2));
    }
    
    IEnumerator MoveToPosition(Vector3 position, Vector3 rotation)
    {
        moving = true;
        while (Vector3.Distance(transform.position, position) > 0.001f || Vector3.Distance(transform.rotation.eulerAngles, rotation) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation.eulerAngles, rotation, speed * Time.deltaTime));
            yield return null;
        }
        moving = false;
    }

    IEnumerator MoveToPositions(Vector3 position1, Vector3 rotation1, Vector3 position2, Vector3 rotation2)
    {
        moving = true;
        while (Vector3.Distance(transform.position, position1) > 0.001f || Vector3.Distance(transform.rotation.eulerAngles, rotation1) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position1, speed*2.1f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation.eulerAngles, rotation1, speed * Time.deltaTime));
            yield return null;
        }

        // Check if the position of the cube and sphere are approximately equal.
        while (Vector3.Distance(transform.position, position2) > 0.001f || Vector3.Distance(transform.rotation.eulerAngles, rotation2) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position2, speed*2.1f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.MoveTowards(transform.rotation.eulerAngles, rotation2, speed * Time.deltaTime));
            yield return null;
        }
        moving = false;
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
        downloadingImage = true;
        cover.StartLoadingText();
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        //imagePrompt.StopLoadingText();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
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
            text.SetText("#" + index);
        }
        downloadingImage = false;
    }

    public bool Busy() {
        return downloadingImage && moving;
    }
}

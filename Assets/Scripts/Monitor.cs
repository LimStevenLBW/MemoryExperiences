using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Monitor : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    //public ImagePrompt imagePrompt;
    public InputFieldPrompt inputFieldPrompt;

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
            transform.position = Vector3.MoveTowards(transform.position, position, 75 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator MoveToPositions(Vector3 position1, Vector3 position2)
    {
        while (Vector3.Distance(transform.position, position1) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position1, 150 * Time.deltaTime);
            yield return null;
        }

        // Check if the position of the cube and sphere are approximately equal.
        while (Vector3.Distance(transform.position, position2) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position2, 150 * Time.deltaTime);
            yield return null;
        }
    }



    public void setImage(string url)
    {
        StartCoroutine(DownloadImage(url));

    }
    IEnumerator DownloadImage(string MediaUrl) //downdload image and make it to the material
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        //imagePrompt.StopLoadingText();

        inputFieldPrompt.activated = false;
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            //Texture.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            //texture=SetTexture("Image", ((DownloadHandlerTexture)request.downloadHandler).texture);

            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            var mat = new Material(Shader.Find("UI/Default"));
            mat.mainTexture = myTexture;
            meshRenderer.material.mainTexture = myTexture;
        }
    }   
}

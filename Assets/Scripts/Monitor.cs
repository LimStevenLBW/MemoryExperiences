using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class Monitor : MonoBehaviour
{
    public API_Manager APIManager;
    private float speed = 300;
    private MeshRenderer meshRenderer;
    public int index { get; set; }
    public MonitorCover cover;
    public TextMeshPro text;
    public VideoLoader loader;
    public Material videoMat;

    private bool moving;
    private bool downloadingImage;
    private Artifact artifact;

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

    public void SetArtifact(Artifact artifact, int index) {
        this.artifact = artifact;
        this.index = index;
        StartCoroutine(DownloadImage(artifact.imageURL));
        if (MonitorManager.videoMode) {
            DownloadVideo();
        }
    }

    public void DownloadVideo() {
        APIManager.RequestVideo(artifact.gptPrompt, loader);
        meshRenderer.SetMaterials(new List<Material>{videoMat});
    }

    public void HideVideo() {
        loader.videoPlayer.Stop(); 
        StartCoroutine(DownloadImage(artifact.imageURL));
    }

    IEnumerator DownloadImage(string MediaUrl) //download image and make it to the material
    {
        downloadingImage = true;
        cover.StartLoadingText();
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        { 
            Debug.Log(request.error);
            cover.StopLoadingText();
        }
        else
        {
            print("Downloaded " + index.ToString());

            Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

            // Use Unlit/Texture shader which respects scaling
            var mat = new Material(Shader.Find("Unlit/Texture"));
            mat.SetTexture("_MainTex", myTexture);
            mat.SetTextureScale("_MainTex", new Vector2(-1, 1)); // Flip the texture horizontally

            // Apply the material to the mesh
            meshRenderer.SetMaterials(new List<Material>{mat});

            cover.StopLoadingText();
            text.SetText("#" + index);
        }
        downloadingImage = false;
    }

    public bool Busy() {
        return downloadingImage && moving;
    }
}

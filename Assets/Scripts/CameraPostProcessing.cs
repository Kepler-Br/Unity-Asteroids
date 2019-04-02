using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [ExecuteInEditMode]
public class CameraPostProcessing : MonoBehaviour
{
    [Range (1, 24)]
    public int DownRez = 1;
    RenderTexture renderTexture;
    public Material shaderMaterial;
    public bool applyVsh = true;

    void CreateLowRezCamera()
    {
        var camObj = new GameObject();
        camObj.transform.position = this.transform.position;
        camObj.transform.rotation = this.transform.rotation;
        Camera cam = camObj.AddComponent<Camera>();
        cam.backgroundColor = Color.black;
        renderTexture = new RenderTexture(Screen.width/DownRez, Screen.height/DownRez, 16);
        cam.targetTexture = renderTexture;
        renderTexture.filterMode = FilterMode.Point;
        cam.projectionMatrix = this.GetComponent<Camera>().projectionMatrix;
    }

    void Start()
    {
        // Camera mainCam = this.GetComponent<Camera>();
        CreateLowRezCamera();
        // int width = mainCam.targetTexture.width;
        // int height = mainCam.targetTexture.height;




    }
    // Start is called before the first frame update
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        

        // lowRezCamera.Render();

        if (applyVsh)
            Graphics.Blit(renderTexture, destination, shaderMaterial);
        else
            Graphics.Blit(renderTexture, destination);
        
    }
}

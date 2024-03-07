using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraController : MonoBehaviour
{
    private PixelPerfectCamera pixelPerfectCamera;
    private int startPPU = 100;
    private int targetPPU = 150;
    private float zoomInDuration = 1f;
    private float zoomOutDuration = 0.5f;

    private Coroutine zoomCoroutine;

    void Start()
    {
        pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        pixelPerfectCamera.assetsPPU = startPPU;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Start zooming in coroutine
            zoomCoroutine = StartCoroutine(ZoomIn());
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            // Stop zooming in coroutine if it's running
            if (zoomCoroutine != null)
                StopCoroutine(zoomCoroutine);

            // Start zooming out coroutine
            zoomCoroutine = StartCoroutine(ZoomOut());
        }
    }

    IEnumerator ZoomIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < zoomInDuration)
        {
            float t = elapsedTime / zoomInDuration;
            pixelPerfectCamera.assetsPPU = (int)Mathf.Lerp(startPPU, targetPPU, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pixelPerfectCamera.assetsPPU = targetPPU;
    }

    IEnumerator ZoomOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < zoomOutDuration)
        {
            float t = elapsedTime / zoomOutDuration;
            pixelPerfectCamera.assetsPPU = (int)Mathf.Lerp(targetPPU, startPPU, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pixelPerfectCamera.assetsPPU = startPPU;
    }
}
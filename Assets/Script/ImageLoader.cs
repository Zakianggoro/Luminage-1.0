using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLoader : MonoBehaviour
{
    private static ImageLoader _instance;

    public static ImageLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject loaderObject = new GameObject("ImageLoader");
                _instance = loaderObject.AddComponent<ImageLoader>();
                DontDestroyOnLoad(loaderObject); // Keep this GameObject between scenes
            }
            return _instance;
        }
    }

    public void LoadImage(string path, Image targetImage, System.Action onComplete = null)
    {
        StartCoroutine(LoadImageCoroutine(path, targetImage, onComplete));
    }

    private IEnumerator LoadImageCoroutine(string path, Image targetImage, System.Action onComplete)
    {
        ResourceRequest loadRequest = Resources.LoadAsync<Sprite>(path);
        yield return loadRequest;

        if (loadRequest.asset != null)
        {
            targetImage.sprite = loadRequest.asset as Sprite;
        }
        else
        {
            Debug.LogError("Failed to load image from path: " + path);
        }

        // Invoke the callback if there's any
        onComplete?.Invoke();
    }
}


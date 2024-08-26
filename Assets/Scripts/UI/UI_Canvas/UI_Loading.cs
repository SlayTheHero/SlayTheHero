using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SceneController;

public class UI_Loading : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image progressBar;
    int SceneId;
    private SceneDataLoader sceneDataLoader = new SceneDataLoader();
    public void StartLoading(SceneType sceneId)
    {
        gameObject.SetActive(true);
        SceneId = (int)sceneId;
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(LoadSceneProcess());
    }
    float destProgress;
    private IEnumerator LoadSceneProcess()
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(SceneId);
        op.allowSceneActivation = false;
        StartCoroutine(StartProgressBar());
        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;
            if(op.progress < 0.9f)
            {
                destProgress = op.progress / 2;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.45f, 0.5f, timer);
                if(progressBar.fillAmount >= 0.5f)
                {
                    StopCoroutine(StartProgressBar());
                    destProgress = 0.5f;
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
    private IEnumerator LoadDataProcess()
    {
        progressBar.fillAmount = 0.5f;
        sceneDataLoader.StartDataLoad((SceneController.SceneType)SceneId);


        StartCoroutine(StartProgressBar());
        float timer = 0f;

        while (true)
        {
            yield return null;
            if (sceneDataLoader.progress < 1)
            {
                destProgress = 0.5f + sceneDataLoader.progress / 2;
            }
            else if (sceneDataLoader.isDone || sceneDataLoader.progress == 1)
            {
                timer += Time.fixedDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    StartCoroutine(Fade(false));
                    SceneManager.sceneLoaded -= OnSceneLoaded;
                    yield break;
                }
            }
        }
    }
    private IEnumerator StartProgressBar()
    {
        while(true)
        {
            progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, destProgress, 0.3f);
            yield return null;
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        StartCoroutine(LoadDataProcess());
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        float timer = 0f;
        while(timer <= 1f)
        {
            yield return null;
            timer += Time.fixedDeltaTime * 2f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f,0f,timer);
        }
        if(!isFadeIn)
        {
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using UnityEngine;

public class LoadingCurtain : MonoBehaviour
{
    private const float STEP_TIME = 0.03f;
    private const float FULL_ALPHA = 1.0f;

    [SerializeField] private CanvasGroup _canvasCurtain;

    private void Awake() => DontDestroyOnLoad(this);

    public void Show()
    {
        gameObject.SetActive(true);
        _canvasCurtain.alpha = FULL_ALPHA;
    }

    public void Hide() =>
        StartCoroutine(FadeIn());

    private IEnumerator FadeIn()
    {
        while(_canvasCurtain.alpha > 0)
        {
            _canvasCurtain.alpha -= STEP_TIME;
            yield return new WaitForSeconds(STEP_TIME);
        }

        gameObject.SetActive(false);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ObjectiveLightTarget : MonoBehaviour
{
    [Header("Light")]
    [SerializeField] private Light2D targetLight;
    [SerializeField] private bool toggleGameObjectInstead = false;

    [Header("Blink")]
    [SerializeField] private bool blink = true;
    [SerializeField] private float blinkInterval = 0.35f;

    [Header("State")]
    [SerializeField] private bool startEnabled = false;

    private Coroutine blinkRoutine;
    private bool highlighted;

    private void Awake()
    {
        if (targetLight == null)
            targetLight = GetComponentInChildren<Light2D>(true);

        if (!startEnabled)
            SetHighlighted(false);
    }

    private void OnDisable()
    {
        StopBlinking();
    }

    public void SetHighlighted(bool shouldHighlight)
    {
        highlighted = shouldHighlight;

        if (toggleGameObjectInstead)
        {
            gameObject.SetActive(shouldHighlight);
            return;
        }

        if (targetLight == null)
            return;

        if (!shouldHighlight)
        {
            StopBlinking();
            targetLight.enabled = false;
            return;
        }

        if (!blink)
        {
            targetLight.enabled = true;
            return;
        }

        StartBlinking();
    }

    private void StartBlinking()
    {
        StopBlinking();
        blinkRoutine = StartCoroutine(BlinkRoutine());
    }

    private void StopBlinking()
    {
        if (blinkRoutine != null)
        {
            StopCoroutine(blinkRoutine);
            blinkRoutine = null;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        while (highlighted)
        {
            if (targetLight != null)
                targetLight.enabled = !targetLight.enabled;

            yield return new WaitForSeconds(blinkInterval);
        }

        if (targetLight != null)
            targetLight.enabled = false;
    }
}

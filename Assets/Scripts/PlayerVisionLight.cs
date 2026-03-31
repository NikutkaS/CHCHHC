using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerVisionLight : MonoBehaviour
{
    [Header("Свет вокруг игрока")]
    [SerializeField] private float pointLightOuterRadius = 8f;
    [SerializeField] private float pointLightInnerRadius = 1f;
    [SerializeField] private float lightIntensity = 1.5f;
    [SerializeField] private Color lightColor = Color.white;

    [Header("Смещение")]
    [SerializeField] private Vector3 lightOffset = Vector3.zero;

    private Light2D visionLight;

    private void Awake()
    {
        visionLight = GetComponentInChildren<Light2D>();

        if (visionLight == null)
        {
            GameObject lightObject = new GameObject("Player Vision Light");
            lightObject.transform.SetParent(transform);
            lightObject.transform.localPosition = lightOffset;
            lightObject.transform.localRotation = Quaternion.identity;

            visionLight = lightObject.AddComponent<Light2D>();
            visionLight.lightType = Light2D.LightType.Point;
        }
    }

    private void Start()
    {
        ApplySettings();
    }

    private void LateUpdate()
    {
        if (visionLight == null)
            return;

        visionLight.transform.localPosition = lightOffset;
    }

    private void OnValidate()
    {
        ApplySettings();
    }

    private void ApplySettings()
    {
        if (visionLight == null)
            return;

        visionLight.lightType = Light2D.LightType.Point;
        visionLight.intensity = lightIntensity;
        visionLight.pointLightOuterRadius = pointLightOuterRadius;
        visionLight.pointLightInnerRadius = pointLightInnerRadius;
        visionLight.color = lightColor;
    }
}

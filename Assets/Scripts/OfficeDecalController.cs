using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OfficeDecalController : MonoBehaviour
{
    public GameObject officeDirtParent; 
    public float nightDurationInSeconds = 360f;
    private float timeElapsed = 0f;

    private DecalProjector[] childDecals;

    void Start()
    {
        if (officeDirtParent != null)
        {
            childDecals = officeDirtParent.GetComponentsInChildren<DecalProjector>();
        }
        else
        {
            childDecals = GetComponentsInChildren<DecalProjector>();
        }

        SetDecalOpacity(0f);
    }

    void Update()
    {
        if (timeElapsed < nightDurationInSeconds)
        {
            timeElapsed += Time.deltaTime;
            float currentOpacity = Mathf.Clamp01(timeElapsed / nightDurationInSeconds);
            SetDecalOpacity(currentOpacity);
        }
    }

    private void SetDecalOpacity(float alpha)
    {
        if (childDecals == null) return;

        foreach (DecalProjector decal in childDecals)
        {
            if (decal != null)
            {
                // Decal Projectors use fadeFactor
                decal.fadeFactor = alpha; 
            }
        }
    }
}
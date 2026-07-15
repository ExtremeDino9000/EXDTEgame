using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerClick : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float maxInteractionDistance = 3.5f; 

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            // NEW: Added 'QueryTriggerInteraction.Collide' to force close-up detection if it's set to trigger
            if (Physics.Raycast(ray, out hit, maxInteractionDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide))
            {
                BreakerSwitch switchScript = hit.transform.GetComponent<BreakerSwitch>();
                if (switchScript != null)
                {
                    switchScript.ToggleLight();
                }
            }
        }
    }
}
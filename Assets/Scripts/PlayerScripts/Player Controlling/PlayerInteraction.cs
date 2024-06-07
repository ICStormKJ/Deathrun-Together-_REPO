using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float reach = 3f;
    Interactable currentInteractable;
    private Camera camera;

    //----------Trapmaster things----------
    public float trapCooldown;
    private float timer;
    private bool canUseTrap;

    private void Start()
    {
        canUseTrap = false;
        camera = GetComponentInChildren<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            canUseTrap = true;
        }
        else
        {
            timer -= Time.deltaTime;
            canUseTrap = false;
        }
        CheckInteraction();
        if (canUseTrap && Input.GetKeyDown(KeyCode.E) && currentInteractable != null){
            currentInteractable.Interact();
            timer = trapCooldown;
        }
    }

    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        if (Physics.Raycast(ray, out hit, reach))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable newInteract = hit.collider.GetComponent<Interactable>();

                if (currentInteractable && newInteract != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }

                if (newInteract.enabled)
                {
                    SetNewCurrentInteractable(newInteract);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        HUDController.instance.EnableText(currentInteractable.message);
    }

    void DisableCurrentInteractable()
    {
        HUDController.instance.DisableText();
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable=null;
        }
    }
}

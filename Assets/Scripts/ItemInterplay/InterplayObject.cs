using UnityEngine;

[RequireComponent(typeof(Outline))]
public class InterplayObject : MonoBehaviour
{
    public bool Interactable = true;

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    public void SetItemOutline(bool isOutline)
    {
        outline.enabled = isOutline;
    }

    public virtual void IterplayObject(CharacterInvetory characterInvetory)
    {
    }
}
using UnityEngine;

[RequireComponent(typeof(Outline), typeof(Rigidbody))]
public class PlayerHandItem : MonoPooled
{
    [field: SerializeField] public HandItemType HandItemType { get; private set; }
    [field: SerializeField] public CharacterAnimationType CharacterAnimation { get; private set; }
    [field: SerializeField] public Vector3 AttachPosition { get; private set; }
    [field: SerializeField] public Vector3 AttachRotation { get; private set; }
    [SerializeField] private int selectableMaskIndex;
    [SerializeField] private int deSelectableMaskIndex;

    public bool IsSelectable = true;

    private Rigidbody rigidbody;
    private Outline outline;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        outline = GetComponent<Outline>();
    }

    protected virtual void OnAwake(){}
    public void SetItemOutline(bool isOutline)
    {
        outline.enabled = isOutline;
    }

    public void ItemSelected()
    {
        rigidbody.isKinematic = true;
        gameObject.layer = selectableMaskIndex;
    }

    public virtual void ItemDeSelected()
    {
        rigidbody.isKinematic = false;
        gameObject.layer = deSelectableMaskIndex;
    }
}
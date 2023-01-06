using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RopeBreaker : Interactable<JointConnector>
{
    public override void OnInteract(JointConnector interact)
    {
        interact.Rope.Break();
    }
}
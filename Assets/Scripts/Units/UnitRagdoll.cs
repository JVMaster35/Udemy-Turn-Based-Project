using System;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    private void Update()
    {
        RemoveRagdollBodies();
    }

    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, ragdollRootBone);

        Vector3 randomDir = new Vector3(UnityEngine.Random.Range(-1f, +1f), 0, UnityEngine.Random.Range(-1f, +1f));
        ApplyExplosionToRagdoll(ragdollRootBone, 250f, transform.position + randomDir, 10f);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach(Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if(cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 exploisionPos, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(explosionForce, exploisionPos, explosionRange);
            }

            ApplyExplosionToRagdoll(child, explosionForce, exploisionPos, explosionRange);
        }

    }

    private  void RemoveRagdollBodies()
    {
        TurnSystem.Instance.OnTurnChanged += OnTurnChganged_RemoveRagdolls;
    }

    private void OnTurnChganged_RemoveRagdolls(object sender, EventArgs e)
    {
        Destroy(gameObject);
    }
}

using UnityEngine;
using UnityEngine.Rendering;

public class EffectDestroy : MonoBehaviour
{
    [SerializeField] private float Time;
    private void Start()
    {
        Destroy(this.gameObject, Time);
    }
}

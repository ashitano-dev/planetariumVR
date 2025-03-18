using UnityEngine;
using UnityEngine.Animations;

public class StarManager : MonoBehaviour
{
    public int hip_id;

    public bool is_active = false;
    public void Activate()
    {
        is_active = true;
    }
}

using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CustomeButtonBehaviour : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    private bool _IsOpen = false;
    private void Open()
    {
        if (_animator != null)
        {
            _animator.Play("Anim_EditOptions_Show");
        }
    }
    private void Close()
    {
        if (_animator != null)
        {
            _animator.Play("Anim_EditOptions_Close");
        }
    }
    public void OnInteraction()
    {
        if (_IsOpen)
        {
            Close();
        }
        else Open();
        _IsOpen = !_IsOpen;
    }
}

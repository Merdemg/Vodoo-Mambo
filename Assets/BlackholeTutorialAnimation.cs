#define USE_LOGS
using UnityEngine;

public class BlackholeTutorialAnimation : MonoBehaviour
{
    public Transform[] ballPositions;
    public Animator[] _animators;

    private void Start()
    {
        // Disable all animations except first
        for (var i = 1; i < (_animators.Length); i++)
        {
            var animator = _animators[i];
            animator.gameObject.SetActive(false);
        }
    }

    public void KillAnimation(int index)
    {
        Trace.Msg(string.Format("Killing animation index {0}", index));
        _animators[index].gameObject.SetActive(false);
        
        // Enable next animation
        if(index < _animators.Length - 1)
            _animators[index + 1].gameObject.SetActive(true);
    }
}

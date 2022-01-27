using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTransi : MonoBehaviour
{
    public Animator anim;
    public bool menu;
    public bool last;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        if (!menu)
            anim.Play("FadeOut");
        player = FindObjectOfType<Player>().GetComponent<Player>();
    }

    private void Update()
    {
        if (player.flagHit) anim.Play("FadeIn");
    }
    void Next()
    {
        player.NextLevel();
    }
}

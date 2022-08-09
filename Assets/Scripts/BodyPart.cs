using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPart : MonoBehaviour
{
    [SerializeField] private Sprite _detachedSprite;
    [SerializeField] private Sprite _burnedSprite;
    [SerializeField] private Transform _bloodFountainOrigin;

    private bool _detached = false;

    public void Detach()
    {
        _detached = true;
        this.tag = "Untagged";
        transform.SetParent(null, true);
    }

    private void Update()
    {
        if (_detached == false)
            return;

        var rigidbody = GetComponent<Rigidbody2D>();

        if(rigidbody.IsSleeping())
        {
            foreach (Joint2D joint in GetComponentsInChildren<Joint2D>())
                Destroy(joint);

            foreach (Rigidbody2D rigidbody2D in GetComponentsInChildren<Rigidbody2D>())
                Destroy(rigidbody2D);

            foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
                Destroy(collider);

            Destroy(this);
        }
    }

    public void ApplyDamageSprite(Gnome.DamageType damageType)
    {
        Sprite spriteToUse = null;

        switch (damageType)
        {
            case Gnome.DamageType.Burning:
                spriteToUse = _burnedSprite;
                break;
            case Gnome.DamageType.Slicing:
                spriteToUse = _detachedSprite;
                break;
        }

        if (spriteToUse != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteToUse;
        }
    }
}

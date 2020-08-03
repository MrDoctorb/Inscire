using UnityEngine;
using System;
using System.Collections;

public interface IDamageable
{
    void TakeDamage(int dmg);
}

public interface IKnockbackable
{
    GameObject gameObject
    {
        get;
    }

    void Knockback(Vector2 direction, float knockbackForce);//10 is a good starting knockback force w linear drag of 5
}

public interface IKillable
{
    IEnumerator Die();
}

public interface IInteractable
{
    void Interact();
}

public interface ITextEvent
{
    void TextFinished();//Will be called most by Display_Text
}

[System.Serializable]
public class ITextEventContainer : IUnifiedContainer<ITextEvent> { }

public interface ISaveable
{
    void SaveData(Scene_Data data);
    void LoadData(Scene_Data data);
}
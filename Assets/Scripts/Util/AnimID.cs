using UnityEngine;

public static class PlayerAnimID
{ 
    public static readonly int Up = Animator.StringToHash("Up");
    public static readonly int Down = Animator.StringToHash("Down");
    public static readonly int Left = Animator.StringToHash("Left");
    public static readonly int Right = Animator.StringToHash("Right");

    public static readonly int UseSkill = Animator.StringToHash("UseSkill");
    public static readonly int InvincibilityUp = Animator.StringToHash("InvincibilityUp");
    public static readonly int InvincibilityDown = Animator.StringToHash("InvincibilityDown");
    public static readonly int InvincibilityLeft = Animator.StringToHash("InvincibilityLeft");
    public static readonly int InvincibilityRight = Animator.StringToHash("InvincibilityRight");
}

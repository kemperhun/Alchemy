using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;
    Vector2 velocity;
    public Animator Animator;

    void FixedUpdate()
    {
        velocity.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        transform.Translate(velocity.x, 0, velocity.y);
        //Animator.SetFloat("forward speed", Mathf.Clamp(Input.GetAxis("Vertical") + Input.GetAxis("Horizontal"), -1, 1));
        //if (velocity.y != 0 || velocity.x != 0) Animator.SetBool("walk", true);
        //else Animator.SetBool("walk", false);
    }
}

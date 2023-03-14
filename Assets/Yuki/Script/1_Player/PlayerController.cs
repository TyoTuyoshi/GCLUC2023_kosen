using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float speed;
    
    //public readonly 
    private InputAction move, jump;
    private Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = gameObject.GetComponent<PlayerInput>();
        move = playerInput.actions["Move"];
        rbody = this.gameObject.GetComponent<Rigidbody2D>();
        //Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 m = move.ReadValue<Vector2>();
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _animator.SetTrigger("Attack");
        }

        _animator.SetFloat("run", Mathf.Abs(m.magnitude));
        rbody.MovePosition(transform.position + new Vector3(m.x, m.y, 0) * Time.deltaTime * speed);
    }
}

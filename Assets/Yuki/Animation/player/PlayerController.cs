using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private InputAction move, jump;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput playerInput = gameObject.GetComponent<PlayerInput>();
        move = playerInput.actions["Move"];
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 vec2 = new Vector2(Keyboard.current.)
        Vector2 m = move.ReadValue<Vector2>();
        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _animator.SetTrigger("Attack");
        }

        _animator.SetFloat("run", Mathf.Abs(m.magnitude));
        
    }
}

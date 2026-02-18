using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Runtime.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerMovement playerMovement;
        
        public void OnMove(InputValue value)
        {
            playerMovement.movementInput = value.Get<Vector2>();
        }
    }
}
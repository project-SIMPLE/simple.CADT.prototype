using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class InteractableClimber : MonoBehaviour
{
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] float distanceToClimb = 1f;
    [SerializeField] float maxHeigth = 7f;

    [SerializeField] CharacterController characterController;
    [SerializeField] InputActionReference moveAction;
    [SerializeField] ContinuousMoveProvider locomotionProvider;
    [SerializeField] Transform head;

    bool isClimbing = false;
    float verticalVelocity = 0f;

    public void StartClimb(SelectEnterEventArgs args)
    {
        if (!args.interactableObject.transform.CompareTag("climbable"))
        {
            return;
        }

        Vector3 handPosition = args.interactableObject.transform.position;
        float verticalDistance = Vector3.Distance(head.position, handPosition);
        if (verticalDistance > distanceToClimb)
        {
            return;
        }

        isClimbing = true;
        Debug.Log("Started Climbing");

        if (locomotionProvider != null)
        {
            locomotionProvider.enabled = false;
        }
    }

    public void StopClimb(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("climbable"))
        {
            isClimbing = false;

            if (locomotionProvider != null)
            {
                locomotionProvider.enabled = true;
                locomotionProvider.useGravity = true;
            }

            Debug.Log("Stopped Climbing");
        }
    }

    void Update()
    {
        if (isClimbing)
        {
            HandleClimbingMovement();
        }
        else 
        {
            HandleGravity();
        }
    }

    private void HandleClimbingMovement()
    {
        float joystickVerticalInput = moveAction.action.ReadValue<Vector2>().y;

        if (Mathf.Abs(joystickVerticalInput) > 0.1f)
        {
            if(head.position.y >= maxHeigth)
            {
                return;
            }
            
            Vector3 climbMovement = Vector3.up * joystickVerticalInput * climbSpeed * Time.deltaTime;
            characterController.Move(climbMovement);
        }
    }
    
     private void HandleGravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        Vector3 gravityMovement = new Vector3(0, verticalVelocity, 0) * Time.deltaTime;
        characterController.Move(gravityMovement);
    }
}
using UnityEngine;

public class AICollector : MonoBehaviour
{
    public Transform collectibleArea;
    public Transform dropArea;
    public float moveSpeed = 3f;
    private Animator animator;

    private enum AIState { MovingToCollectible, MovingToDrop }
    private AIState currentState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentState = AIState.MovingToCollectible;
        SetRunState(); // Ýlk hareket baþlarken koþma animasyonu baþlasýn
    }

    private void Update()
    {
        if (currentState == AIState.MovingToCollectible)
        {
            MoveToCollectibleArea();
        }
        else if (currentState == AIState.MovingToDrop)
        {
            MoveToDropArea();
        }
    }

    private void MoveToCollectibleArea()
    {
        if (Vector3.Distance(transform.position, collectibleArea.position) > 0.1f)
        {
            SetRunState(); // Hareket ediyorsa koþma animasyonunu aç
            transform.position = Vector3.MoveTowards(transform.position, collectibleArea.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            SetIdleState(); // Hedefe ulaþtýðýnda dur
            currentState = AIState.MovingToDrop;
        }
    }

    private void MoveToDropArea()
    {
        if (Vector3.Distance(transform.position, dropArea.position) > 0.1f)
        {
            SetRunState(); // Hareket ediyorsa koþma animasyonunu aç
            transform.position = Vector3.MoveTowards(transform.position, dropArea.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            SetIdleState(); // Hedefe ulaþtýðýnda dur
            currentState = AIState.MovingToCollectible;
        }
    }

    private void SetIdleState()
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", false); // Duraðan animasyona geç
        }
    }

    private void SetRunState()
    {
        if (animator != null)
        {
            animator.SetBool("isRunning", true); // Koþma animasyonuna geç
        }
    }
}

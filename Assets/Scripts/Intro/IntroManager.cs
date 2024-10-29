using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    //Intro Level Light Variables
    [SerializeField] private GameObject pointLight;

    // Door Animation and End Point variables
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private Transform doorEntrancePosition;
    private bool isDoorOpen = false;

    //Player Movement and Animation variables
    [SerializeField] private GameObject player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float moveSpeed = 1.75f;
    [SerializeField] private float stoppingDistance = 0.1f;

    //Screen Fadeout variable
    [SerializeField] private CanvasGroup fadeOutCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnablePointLight", 2f);

        Invoke("OpenDoor", 17f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EnablePointLight()
    {
        pointLight.SetActive(true);
    }

    void OpenDoor()
    {
        doorAnimator.SetTrigger("isOpen");
        isDoorOpen = true;


        StartCoroutine(MovePlayerToDoor());
    }

    private IEnumerator MovePlayerToDoor()
    {
        if (isDoorOpen)
        {
            yield return new WaitForSeconds(3.5f);
            Vector3 _direction = (doorEntrancePosition.position - player.transform.position).normalized;

            playerAnimator.SetBool("isWalking", true);

            // Move the player toward the door
            while (Vector3.Distance(player.transform.position, doorEntrancePosition.position) > stoppingDistance)
            {
                characterController.Move(_direction * moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Stop the player at the door entrance position
            player.transform.position = doorEntrancePosition.position;
            playerAnimator.SetBool("isWalking", false);

            // Immediately start fading out the screen
            StartCoroutine(FadeToBlack());
        }

    }

    private IEnumerator FadeToBlack()
    {
        Debug.Log("Fading started");

        // Ensure the fade out starts from the current alpha value
        float initialAlpha = fadeOutCanvasGroup.alpha;
        float _duration = 2f;
        float _elapsedTime = 0f;

        while (_elapsedTime < _duration)
        {
            fadeOutCanvasGroup.alpha = Mathf.Lerp(initialAlpha, 1, _elapsedTime / _duration);
            _elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeOutCanvasGroup.alpha = 1; // Ensure it's fully opaque
        Debug.Log("Fading completed");
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    bool shooting;
    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.started) 
        {
            shooting = true;
            StartCoroutine(Shoot());
        }
        else if (context.canceled)
        {
            shooting = false;
        }
    }

    IEnumerator Shoot()
    {
        while(shooting)
        {
            Debug.Log("Disparé.");
            yield return new WaitForSeconds(0.2f);
        }
    }
}

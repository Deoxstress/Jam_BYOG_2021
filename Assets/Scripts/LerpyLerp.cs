using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpyLerp : MonoBehaviour
{
    // Si tu lis ça c'est bien. Ce script sert à tester le lerp en déplacement (puisque hier t'étais fatigué) après avoir lu ta dinguerie de 150 lignes :D du coup met toi bien

    public float lerpTime; // t ==> temps de trajet

    public float currentLerpTime;//Dividende pour normalizé le temps de trajet

    public float moveDistance;//Distance à parcourir en t

    public Vector3 startPos;
    public Vector3 endPos;
    public bool rayHit;

    protected void Update()
    {
        //Reset le timer de déplacement et set la position du perso
        if (currentLerpTime == lerpTime)
        {
            if (transform.position.x > -5.0f && transform.position.x < 5.0f && rayHit == false)
            {
                if (Input.GetKeyDown(KeyCode.D))
                {
                    RaycastHit hit;
                    currentLerpTime = 0f;
                    startPos = transform.position;
                    endPos = transform.position + transform.forward * moveDistance;
                    Debug.DrawRay(startPos, endPos, Color.red, 1.1f);
                    if (Physics.Raycast(startPos, endPos, out hit, 1.1f))
                    {
                        if (hit.collider.tag == "Interactable") rayHit = true;
                        else rayHit = false;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Q))
                {
                    RaycastHit hit;
                    currentLerpTime = 0f;
                    startPos = transform.position;
                    endPos = transform.position - transform.forward * moveDistance;
                    if (Physics.Raycast(transform.position, -Vector3.forward, out hit, 1.1f))
                    {
                        if (hit.collider.tag == "Interactable") rayHit = true;
                        else rayHit = false;
                    }
                }
            }
        }
        //déplace sur deltaTime
        currentLerpTime += Time.deltaTime;
        //Pas overlap le timer max
        if (currentLerpTime > lerpTime)
        {
            currentLerpTime = lerpTime;
        }
        //Faire le déplacement
        if (currentLerpTime < lerpTime)
        {
            float normal = currentLerpTime / lerpTime;
            transform.position = Vector3.Lerp(startPos, new Vector3(0, endPos.y, endPos.z), normal);
        }
    }
}

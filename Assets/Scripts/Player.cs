using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private Rigidbody PlayerRb;
    private Vector3 movementDir, targetMoveAmount, moveAmount, smoothMoveVelocity;
    public float lerpTime; // t ==> temps de trajet
    public float currentLerpTime;//Dividende pour normalizé le temps de trajet
    public float moveDistance;//Distance à parcourir en t
    public Vector3 startPos;
    public Vector3 endPos;
    [SerializeField] private GameObject StoredObjToReset, StoredObjMouseOver;
    [SerializeField] private GameObject StoredObjOutlineReset, StoredObjMouseOverReset;
    public bool gameStart, parented, blocked, flagHit;
    private Animator playerAnim;
    [SerializeField] private AudioClip walkSound, spellSound;
    private AudioSource playerAS;
    [SerializeField] private float knockbackStrength;

    void Awake()
    {
        PlayerRb = gameObject.GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        playerAS = GetComponent<AudioSource>();
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            gameStart = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            if (currentLerpTime == lerpTime)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    playerAnim.Play("Move");
                    playerAS.pitch = Random.Range(0.9f, 1.1f);
                    playerAS.PlayOneShot(walkSound);
                    currentLerpTime = 0f;
                    startPos = transform.position;
                    RaycastHit hit;
                    endPos = transform.position + transform.forward * moveDistance;
                    if (Physics.Raycast(startPos, Vector3.forward, out hit, 4.0f))
                    {
                        if (hit.collider.tag == "SidePush" || hit.collider.tag == "Interactable")
                        {
                            endPos = hit.point - Vector3.forward;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    playerAnim.Play("Move");
                    playerAS.pitch = Random.Range(0.9f, 1.1f);
                    playerAS.PlayOneShot(walkSound);
                    currentLerpTime = 0f;
                    startPos = PlayerRb.position;
                    RaycastHit hit;
                    endPos = PlayerRb.position - PlayerRb.transform.forward * moveDistance;
                    if (Physics.Raycast(startPos, Vector3.back, out hit, 4.0f))
                    {
                        if (hit.collider.tag == "SidePush" || hit.collider.tag == "Interactable")
                        {
                            endPos = hit.point - Vector3.forward;
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
            if (currentLerpTime < lerpTime && (parented || transform.position.x > -0.1f && transform.position.x < 0.1f))
            {
                float normal = currentLerpTime / lerpTime;
                transform.position = Vector3.Lerp(startPos, endPos, normal);
                Debug.Log("Normal");
            }
            else if (currentLerpTime < lerpTime && (transform.position.x > 0.1f || transform.position.x < -0.1f || transform.parent == null))
            {
                if (transform.position.x > -5.0f && transform.position.x < 5.0f)
                {
                    float normal = currentLerpTime / lerpTime;
                    transform.position = Vector3.Lerp(startPos, new Vector3(0, endPos.y, endPos.z), normal);
                    Debug.Log("Lerping");
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                TryInteract();
            }

            if (StoredObjToReset != null && Input.GetKeyDown(KeyCode.Space))
            {
                ResetStoredObjPosition(StoredObjToReset);
                playerAnim.Play("PlayerSpell");
                playerAS.pitch = Random.Range(0.9f, 1.1f);
                playerAS.PlayOneShot(spellSound);
            }
        }

        MouseOverInteractables();
    }
    void FixedUpdate()
    {
        if (gameStart)
        {

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WinFlag")
        {
            flagHit = true;
        }
        if (other.tag == "DeathFloor")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Interactable")
        {
            transform.parent = other.transform;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Interactable")
        {
            transform.parent = null;
        }
    }

    void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f))
        {
            if (hit.collider.tag == "Interactable")
            {
                StoredObjToReset = hit.collider.gameObject;
                StoredObjToReset.GetComponent<Animator>().enabled = true;
                StoredObjToReset.GetComponent<OutlineBeat>().ChangeFillColor(new Color(1, 0.5293866f, 0f, 1));
                if (StoredObjOutlineReset != null && StoredObjOutlineReset != StoredObjToReset)
                {
                    if (StoredObjOutlineReset.GetComponent<Outline>() != null)
                    {
                        StoredObjOutlineReset.GetComponent<Animator>().enabled = false;
                        StoredObjOutlineReset.GetComponent<OutlineBeat>().outlineFillValue = 0f;
                        StoredObjOutlineReset.GetComponent<OutlineBeat>().ChangeFillColor(new Color(0.6f, 0.6f, 0.6f, 1));
                        StoredObjOutlineReset = StoredObjToReset;
                    }
                }
                else if (StoredObjOutlineReset == null)
                {
                    StoredObjOutlineReset = StoredObjToReset;
                }
            }
        }
    }

    void MouseOverInteractables()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 200.0f))
        {
            if (hit.collider.tag == "Interactable")
            {
                StoredObjMouseOver = hit.collider.gameObject;
                StoredObjMouseOver.GetComponent<OutlineBeat>().outlineFillValue = 3f;
                if (StoredObjMouseOverReset != null && StoredObjMouseOverReset != StoredObjMouseOver)
                {
                    if (StoredObjMouseOverReset.GetComponent<Outline>() != null)
                    {
                        StoredObjMouseOverReset.GetComponent<OutlineBeat>().outlineFillValue = 0f;
                        StoredObjMouseOverReset = StoredObjMouseOver;
                    }
                }
                else if (StoredObjMouseOverReset == null)
                {
                    StoredObjMouseOverReset = StoredObjMouseOver;
                }
            }
            else if (hit.collider.tag != "Interactable")
            {
                if (StoredObjMouseOver != null && StoredObjMouseOver != StoredObjToReset)
                {
                    StoredObjMouseOver.GetComponent<OutlineBeat>().outlineFillValue = 0f;
                }
            }
        }
    }

    void ResetStoredObjPosition(GameObject StoredObj)
    {
        //Do Stuff here
        if (StoredObj.GetComponent<Waypoints>() == null)
        {
            StoredObj.transform.parent.position = StoredObj.GetComponentInParent<Waypoints>().firstWP.transform.position;
        }
        else
        {
            StoredObj.transform.position = StoredObj.GetComponent<Waypoints>().firstWP.transform.position;

            StoredObj.GetComponent<Waypoints>().waitTimer = StoredObj.GetComponent<Waypoints>().maxTimer;
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            blocked = true;
        }
        if (collision.gameObject.tag == "SidePush")
        {
            Vector3 direction = transform.position - collision.transform.position;
            PlayerRb.AddForce(direction.normalized * knockbackStrength, ForceMode.Impulse);
        }
    }

    public void SetGameStart(bool value)
    {
        gameStart = value;
    }

    public void NextLevel()
    {
        flagHit = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

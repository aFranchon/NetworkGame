using UnityEngine;
using UnityEngine.Networking;

public class Movements : NetworkBehaviour
{
    public GameObject player;
    public GameObject zone;
    public GameObject ShootPrefab;
    public Camera camera;

    public Score scoreScript;

    public int playerId;
    private int NumberOfJumps = 1;
    private int JumpCount = 0;

    private bool AbleToJump = true;
    private bool isAbleToDash = true;
    public bool isAbleToTp = true;
    [SyncVar]
    private bool isAbleToShoot = true;

    private float DashTime = 0.5f;
    [SyncVar]
    private float ActualDashTime = 0.5f;

    private float ShotTime = 0.5f;
    [SyncVar]
    private float ActualShotTime = 0.5f;
    [SyncVar]
    private Vector3 ShotDirectionF;
    [SyncVar]
    private Vector3 ShotDirectionR;

    public GameObject respawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        if (!isLocalPlayer)
        {
            scoreScript.cmdAddPlayer();
            return;
        }

        NetworkManagerHUD hud = FindObjectOfType<NetworkManagerHUD>();
        if (hud != null)
            hud.showGUI = false;

        scoreScript = GameObject.Find("Scores").GetComponent<Score>();

        respawnPoints = GameObject.Find("SpawningPos");
        Debug.Log(respawnPoints);

        camera.gameObject.SetActive(true);

        Debug.Log("Local player creation");
        Cursor.visible = false;

        playerId = scoreScript.cmdAddPlayer();
        Debug.Log(playerId);
        Debug.Log("playerID = " + playerId);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "DeathWall" && isServer)
        {
            RpcRespawn();
        }
        if (!isLocalPlayer)
        {
            // exit from update if this is not the local player
            return;
        }

        if (collision.gameObject.tag == "DeathWall")
        {
            player.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
            ActualDashTime = DashTime;
            ActualShotTime = ShotTime;
        }

        if (collision.gameObject.name == "Ground")
        {
            JumpCount = 0;
        }

        if (collision.gameObject.tag == "LevelUp")
        {
            NumberOfJumps++;
            Debug.Log("Score");
            collision.transform.position = new Vector3(Random.Range(10, 90), zone.transform.position.y + 7f, Random.Range(10, 90));
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        Debug.Log("Salop");
        if (isLocalPlayer)
        {
            transform.position = respawnPoints.GetComponent<GetChildren>().getChildren();
        }
    }


    void AbleSecondJump()
    {
        AbleToJump = true;
    }

    public void Tped()
    {
        isAbleToTp = false;
        Invoke("ResetTp", 3f);
    }

    private void ResetTp()
    {
        isAbleToTp = true;
    }

    private void ResetDash()
    {
        isAbleToDash = true;
    }

    private void ResetShoot()
    {
        isAbleToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            // exit from update if this is not the local player
            return;
        }

        var x = Input.GetAxis("Horizontal") * 50f * Time.deltaTime;
        var z = Input.GetAxis("Vertical") * 50f * Time.deltaTime;

        transform.Translate(z, 0, -x);

        if (Input.GetButton("Jump") && JumpCount < NumberOfJumps && AbleToJump)
        {
            JumpCount++;
            AbleToJump = false;

            player.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
            player.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * 300000 * Time.deltaTime);

            Invoke("AbleSecondJump", 0.5f);
        }

        if (Input.GetButton("Fire1") && isAbleToShoot)
        {
            CmdPlayerShoot();
            isAbleToShoot = false;
            Invoke("ResetShoot", 0.5f);
        }

        if (Input.GetButton("Dash") && isAbleToDash)
        {
            ActualDashTime = 0f;
            isAbleToDash = false;
            Invoke("ResetDash", 0.8f);
        }

        if (ActualDashTime <= DashTime)
        {
            ActualDashTime += 0.09f;
            transform.position -= 300 * transform.forward.normalized * Time.deltaTime * Input.GetAxis("Horizontal");
            transform.position += 300 * transform.right.normalized * Time.deltaTime * Input.GetAxis("Vertical");
        }

        if (ActualShotTime <= ShotTime)
        {
            ActualShotTime += 0.10f;
            transform.position += 700 * ShotDirectionR.normalized * Time.deltaTime;
        }

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * 500);
    }

    [Command]
    void CmdPlayerShoot()
    {
        var newShoot = Instantiate( 
            ShootPrefab,
            transform.position + transform.right * 10,
            Quaternion.identity
        );
        newShoot.GetComponent<Rigidbody>().velocity = transform.right * 100;
        newShoot.GetComponent<Shot>().DirectionF = transform.forward.normalized;
        newShoot.GetComponent<Shot>().DirectionR = transform.right.normalized;
        NetworkServer.Spawn(newShoot);
        Destroy(newShoot, 10f);
    }

    public void PlayerHit(Vector3 DirectionF, Vector3 DirectionR)
    {
        Debug.Log("test -> " + isServer);
        if (!isLocalPlayer && !isServer)
        {
            return;
        }
        Debug.Log("Player hit");
        ActualShotTime = 0f;
        ShotDirectionF = DirectionF;
        ShotDirectionR = DirectionR;
    }
}

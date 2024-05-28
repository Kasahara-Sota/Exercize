using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ガンマンのキャラクターを操作するコンポーネント
/// </summary>

public class GunmanController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    /// <summary>左右移動する力</summary>
    [SerializeField] float m_movePower = 5f;
    /// <summary>ジャンプする力</summary>
    [SerializeField] float m_jumpPower = 15f;
    /// <summary>色の配列</summary>
    [SerializeField] Color[] m_colors = default;
    /// <summary>弾丸のプレハブ</summary>
    [SerializeField] GameObject m_bulletPrefab = default;
    /// <summary>銃口の位置を設定するオブジェクト</summary>
    [SerializeField] Transform m_muzzle = default;
    /// <summary>入力に応じて左右を反転させるかどうかのフラグ</summary>
    [SerializeField] bool m_flipX = false;
    Rigidbody2D m_rb = default;
    SpriteRenderer m_sprite = default;
    /// <summary>m_colors に使う添字</summary>
    int m_colorIndex;
    /// <summary>水平方向の入力値</summary>
    float m_h;
    float m_scaleX;
    /// <summary>最初に出現した座標</summary>
    Vector3 m_initialPosition;
    Vector2 spawnleft;
    Vector2 spawnright;

    public string groundTag = "Ground";
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_sprite = GetComponent<SpriteRenderer>();
        // 初期位置を覚えておく
        m_initialPosition = this.transform.position;
    }
    public bool checkGround = true;

    private bool canJump = true;

    int jumpCount = 2;

    public float shootSpeed = 5f;
    public GameObject prefabToSpawn;
    public Vector2 shootDirection = new Vector2(1f, 1f);
    public bool relativeToRotation = true;

    void Update()
    {
        spawnleft = new Vector2(-10f, this.transform.position.y);
        spawnright = new Vector2(10f, this.transform.position.y);
        // 設定に応じて左右を反転させる
        if (m_flipX)
        {
            FlipX(m_h);
        }
        // 入力を受け取る
        m_h = Input.GetAxisRaw("Horizontal");
        
        // 各種入力を受け取る
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("ここにジャンプする処理を書く。");
            if(jumpCount > 0)
            {
                m_rb.AddForce(Vector2.up * m_jumpPower, ForceMode2D.Impulse);
                jumpCount -= 1;
                Debug.Log(jumpCount);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            
            {
                Debug.Log("ここに弾を発射する処理を書く。");
                    GameObject newObject = Instantiate<GameObject>(prefabToSpawn);
                    newObject.transform.position = m_muzzle.transform.position;
                    //Vector2 actualBulletDirection = (relativeToRotation) ? (Vector2)(Quaternion.Euler(1, 0, transform.eulerAngles.z) * shootDirection) : shootDirection;
                    //rigidbody2D.AddForce(actualBulletDirection * shootSpeed, ForceMode2D.Impulse);
            }         
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("ここに色を切り替える処理を書く。");
            m_sprite.color = m_colors[m_colorIndex];
            m_colorIndex++;
            if(m_colorIndex == 3)
            {
                m_colorIndex = 0;
            }
        }

        // 下に行きすぎたら初期位置に戻す
        if (this.transform.position.y < -10f)
        {
            this.transform.position = m_initialPosition;
        }
        if (this.transform.position.x < -11f)
        {
            this.transform.position = spawnright;
        }
        else if (this.transform.position.x > 11f)
        {
            this.transform.position = spawnleft;
        }



    }
    void OnCollisionEnter2D(Collision2D collisionData)
    {
        if (collisionData.gameObject.CompareTag(groundTag))
        {
            jumpCount = 2;
        }
    }
    private void FixedUpdate()
    {
        // 力を加えるのは FixedUpdate で行う
        m_rb.AddForce(Vector2.right * m_h * m_movePower, ForceMode2D.Force);
    }

    /// <summary>
    /// 左右を反転させる
    /// </summary>
    /// <param name="horizontal">水平方向の入力値</param>
    void FlipX(float horizontal)
    {
        /*
         * 左を入力されたらキャラクターを左に向ける。
         * 左右を反転させるには、Transform:Scale:X に -1 を掛ける。
         * Sprite Renderer の Flip:X を操作しても反転する。
         * */
        m_scaleX = this.transform.localScale.x;

        if (horizontal > 0)
        {
            this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (horizontal < 0)
        {
            this.transform.localScale = new Vector3(-1 * Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
            shootDirection.x *= -1;
        }
    }
}

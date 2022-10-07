using System;
using MyBox;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

[Serializable]
public class VerticalEnemy
{
    public float enemyHeight;

    public VerticalEnemy(float enemyHeight)
    {
        this.enemyHeight = enemyHeight;
    }
}

[Serializable]
public class HorizontalEnemy
{
    public HorizontalEnemy(float gapBetweenEnemy, float horizontalMoveDist, float speed)
    {
        this.gapBetweenEnemy = gapBetweenEnemy;
        this.horizontalMoveDist = horizontalMoveDist;
        this.speed = speed;
    }

    public float gapBetweenEnemy;
    public float horizontalMoveDist;
    public float speed;
}


public class EnemyController : MonoBehaviour
{
    public enum EnemyType
    {
        Horizontal = 1,
        Vertical = 2
    }

    public EnemyType enemyType = EnemyType.Horizontal;

    [ConditionalField(nameof(enemyType), false, EnemyType.Horizontal)]
    public HorizontalEnemy horizontalEnemy;

    [ConditionalField(nameof(enemyType), false, EnemyType.Vertical)]
    public VerticalEnemy verticalEnemy;


    public bool enemyReverse = false;
    private GameObject _whiteEnemy;
    private GameObject _blackEnemy;
    private int _isReverse;
    private bool _direction = true;

    // Start is called before the first frame update
    public EnemyController()
    {
        horizontalEnemy = new HorizontalEnemy(1f, 2f, 2f);
        verticalEnemy = new VerticalEnemy(3f);
    }

    void Start()
    {
        _isReverse = enemyReverse == false ? 1 : -1;
        _whiteEnemy = transform.GetChild(0).gameObject;
        _blackEnemy = transform.GetChild(1).gameObject;

        if (enemyType == EnemyType.Horizontal)
        {
            _whiteEnemy.transform.localPosition = new Vector3(0, -horizontalEnemy.gapBetweenEnemy * _isReverse, 0);
            _blackEnemy.transform.localPosition = new Vector3(0, +horizontalEnemy.gapBetweenEnemy * _isReverse, 0);
            _whiteEnemy.GetComponent<Rigidbody2D>().gravityScale = -1 * _isReverse;
            _blackEnemy.GetComponent<Rigidbody2D>().gravityScale = 1 * _isReverse;
        }

        if (enemyType == EnemyType.Vertical)
        {
            _whiteEnemy.transform.localPosition = new Vector3(0, -(verticalEnemy.enemyHeight * _isReverse), 0);
            _blackEnemy.transform.localPosition = new Vector3(0, +(verticalEnemy.enemyHeight * _isReverse), 0);
            _whiteEnemy.GetComponent<Rigidbody2D>().gravityScale = -1 * _isReverse;
            _blackEnemy.GetComponent<Rigidbody2D>().gravityScale = 1 * _isReverse;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyType == EnemyType.Horizontal)
        {
            HorizontalEnemyMove();
        }
    }

    void HorizontalEnemyMove()
    {
        if (_direction == true && _whiteEnemy.transform.position.x - transform.position.x >=
            horizontalEnemy.horizontalMoveDist)
        {
            _direction = false;
        }

        if (_direction == false && transform.position.x - _whiteEnemy.transform.position.x >=
            horizontalEnemy.horizontalMoveDist)
        {
            _direction = true;
        }

        if (_direction == true)
        {
            _whiteEnemy.transform.position =
                new Vector3(_whiteEnemy.transform.position.x + Time.deltaTime * horizontalEnemy.speed,
                    _isReverse * _whiteEnemy.transform.position.y * _isReverse, 0);
            _blackEnemy.transform.position =
                new Vector3(_blackEnemy.transform.position.x + Time.deltaTime * horizontalEnemy.speed,
                    _isReverse * _blackEnemy.transform.position.y * _isReverse, 0);
        }
        else
        {
            _whiteEnemy.transform.position =
                new Vector3(_whiteEnemy.transform.position.x - Time.deltaTime * horizontalEnemy.speed,
                    _isReverse * _whiteEnemy.transform.position.y * _isReverse, 0);
            _blackEnemy.transform.position =
                new Vector3(_blackEnemy.transform.position.x - Time.deltaTime * horizontalEnemy.speed,
                    _isReverse * _blackEnemy.transform.position.y * _isReverse, 0);
        }
    }
}

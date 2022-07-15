using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    [SerializeField]
    protected int _level;
    [SerializeField]
    protected float _hp;
    [SerializeField]
    protected float _maxhp;

    [SerializeField]
    protected int _attack;
    [SerializeField]
    protected float _moveSpeed;

    public int Level { get { return _level; } set { _level = value; } }
    public float Hp { get { return _hp; } set { _hp = value; } }
    public float MaxHp { get { return _maxhp; } set { _maxhp = value; } }
    public int Attack { get { return _attack; } set { _attack = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    private void Start()
    {
        _maxhp = 100;
        _hp = 100;
        _moveSpeed = 2;
    }


}

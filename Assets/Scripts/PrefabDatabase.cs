using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Custom Assets/PrefabDatabase")]
public class PrefabDatabase : ScriptableObject
{
    private static PrefabDatabase _instance;

    private static PrefabDatabase Instance
    {
        get
        {
            if (_instance) return _instance;

            _instance = Resources.Load<PrefabDatabase>("PrefabDatabase");
            if (!_instance) throw new InvalidOperationException("PrefabDatabase was not found");

            return _instance;
        }
    }

    
    public static GameObject DestroyedAsteroidParticle => Instance._destroyedAsteroidParticle;
    public static GameObject WeaponFireParticle => Instance._weaponFireParticle;

    // public static GameObject PlayerIcon => Instance._playerIcon;

    // public static GameObject Asteroid => Instance._asteroid;

    public static GameObject LazerBullet => Instance._lazerBullet;
    public static GameObject RapidFireBullet => Instance._rapidFireBullet;
    public static GameObject RocketBullet => Instance._rocketBullet;
    public static GameObject RocketSplinter => Instance._rocketSplinter;
    public static GameObject SquareBullet => Instance._squareBullet;
    public static GameObject StingerBullet => Instance._stingerBullet;

    public static GameObject ChaingunSound => Instance._chaingunSound;
    public static GameObject LazerSound => Instance._lazerSound;
    public static GameObject RapidFireSound => Instance._rapidFireSound;
    public static GameObject RocketSound => Instance._rocketSound;
    public static GameObject ShotgunSound => Instance._shotgunSound;
    public static GameObject SquareSound => Instance._squareSound;
    public static GameObject StingerSound => Instance._stingerSound;

    [Header("Particles")]
    [SerializeField] private GameObject _destroyedAsteroidParticle = null;
    [SerializeField] private GameObject _weaponFireParticle = null;
    // [Header("Player")]
    // [SerializeField] private GameObject _playerIcon;
    // [Header("Asteroid")]
    // [SerializeField] private GameObject _asteroid;
    [Header("Bullets")]
    [SerializeField] private GameObject _lazerBullet = null;
    [SerializeField] private GameObject _rapidFireBullet = null;
    [SerializeField] private GameObject _rocketBullet = null;
    [SerializeField] private GameObject _rocketSplinter = null;
    [SerializeField] private GameObject _squareBullet = null;
    [SerializeField] private GameObject _stingerBullet = null;
    [Header("Bullet sounds")]
    [SerializeField] private GameObject _chaingunSound = null;
    [SerializeField] private GameObject _lazerSound = null;
    [SerializeField] private GameObject _rapidFireSound = null;
    [SerializeField] private GameObject _rocketSound = null;
    [SerializeField] private GameObject _shotgunSound = null;
    [SerializeField] private GameObject _squareSound = null;
    [SerializeField] private GameObject _stingerSound = null;
}
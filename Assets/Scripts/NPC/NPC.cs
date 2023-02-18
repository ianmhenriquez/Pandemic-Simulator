using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    [Space]
    [Header("Health")]

    [SerializeField]
    [Tooltip("Current Asset Type")]
    private AssetType _assetType;

    [SerializeField]
    [Tooltip("Character Infection Status")]
    private bool _isInfected;

    [SerializeField]
    [Tooltip("Character Health")]
    private float _health = 100f;

    [SerializeField]
    [Tooltip("The Virus that the NPC carries when infected")]
    private Virus _virus;

    [Space]
    [Header("Stamina")]

    [SerializeField]
    [Tooltip("Character Stamina")]
    private float _stamina = 100f;

    [SerializeField]
    [Tooltip("Stamina base decay rate")]
    private float _staminaDecayBase = 1f;

    [Space]
    [Header("Happiness")]

    [SerializeField]
    [Tooltip("Character Happiness")]
    private float _happiness = 100f;

    [SerializeField]
    [Tooltip("Character Happiness Decay Rate")]
    private float _happinessDecayRate = 0f;

    [SerializeField]
    [Tooltip("Base rate for happiness decay rate")]
    private float _happinessDecayBase = 1f;

    private bool _isHappinessDecayActive;

    private NavMeshAgent _agent;
    private GameManager _gameManager;

    public enum AssetType
    {
        Healthy,
        Infected
    }

    public bool IsInfected { get => _isInfected; set => _isInfected = value; }
    public float Health { get => _health; set => _health = value; }
    public float Stamina { get => _stamina; set => _stamina = value; }
    public float Happiness { get => _happiness; set => _happiness = value; }
    public float HappinessDecayRate { get => _happinessDecayRate; set => _happinessDecayRate = value; }
    public float HappinessDecayBase { get => _happinessDecayBase; set => _happinessDecayBase = value; }
    public float StaminaDecayBase { get => _staminaDecayBase; set => _staminaDecayBase = value; }
    public AssetType Asset { get => _assetType; set => _assetType = value; }
    public Virus Virus { get => _virus; set => _virus = value; }

    private void CheckInfection()
    {
        if ((_isInfected && _assetType == AssetType.Healthy) || (!_isInfected && _assetType == AssetType.Infected))
        {
            _gameManager.UpdateAsset(gameObject);
        }
    }

    public void Copy(GameObject source)
    {
        NPC sourceNpc = source.GetComponent<NPC>();
        _isInfected = sourceNpc._isInfected;
        _health = sourceNpc._health;
        _stamina = sourceNpc._stamina;
        _happiness = sourceNpc._happiness;
        _virus = sourceNpc._virus;
        var currentDestination = source.GetComponent<Navigation>().Destination;
        var home = source.GetComponent<Navigation>().Home;
        GetComponent<Navigation>().Home = home;

        var waypoints = _gameManager.ResidentialDestinations.ToList();
        waypoints.Concat(_gameManager.CommercialDestinations.ToList());
        waypoints.Concat(_gameManager.MedicalDestinations.ToList());

        Building.BuildingType buildingType;
        foreach (GameObject waypoint in waypoints)
        {
            if (waypoint.transform.position == currentDestination.transform.position)
            {
                string tag = waypoint.tag;
                if (tag == "Commercial")
                    buildingType = Building.BuildingType.Commercial;
                else if (tag == "Residential")
                    buildingType = Building.BuildingType.Residential;
                else
                    buildingType = Building.BuildingType.Medical;
                GetComponent<Navigation>().UpdateDestination(currentDestination, buildingType);
                break;
            }
        }
    }

    public void UpdateStamina()
    {
        if (!_gameManager.GodMode)
        {
            float decayRate = _virus ? _virus.StaminaDecayRate + _staminaDecayBase : _staminaDecayBase;
            if (_stamina > 0)
            {
                _stamina -= decayRate * Time.deltaTime;
                return;
            }
            if (_stamina < 0)
            {
                _stamina = 0;
            }
        }
    }

    private void UpdateHealth()
    {
        if (!_gameManager.GodMode)
        {
            if (_isInfected)
            {
                _health -= _virus.HealthDecayRate * Time.deltaTime;
            }

            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void UpdateHappiness()
    {
        if (!_gameManager.GodMode)
        {
            if (_isInfected)
            {
                UpdateHappinessDecayRate();
                float decayRate = _virus ? _happinessDecayBase + _happinessDecayRate : _happinessDecayBase;
                _happiness -= decayRate * Time.deltaTime;
                if (_happiness < 0)
                {
                    _happiness = 0;
                }
            }
        }
    }

    private void UpdateHappinessDecayRate()
    {
        if (_health < _gameManager.HealthThreshold && !_isHappinessDecayActive)
        {
            _happinessDecayRate += 1f;
            _isHappinessDecayActive = true;
        }
        else if (_health >= _gameManager.HealthThreshold && _isHappinessDecayActive)
        {
            _happinessDecayRate -= 1f;
            _isHappinessDecayActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC") && !_isInfected)
        {
            NPC otherNPC = other.gameObject.GetComponent<NPC>();
            if (otherNPC.IsInfected && otherNPC._virus)
            {
                otherNPC._virus.TransmitVirus(this);
            }
        }
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _assetType = IsInfected ? AssetType.Infected : AssetType.Healthy;
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (_agent.velocity.magnitude > 0 && !_gameManager.GodMode)
        {
            UpdateStamina();
        }
        UpdateHealth();
        CheckInfection();
        UpdateHappiness();
    }
}

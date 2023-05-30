using UnityEngine;

public class Level1 : ILevel
{
    public GameObject player;

    [Header("Monster Car")]

    // Using parent of monster car as the original monster car GameObject is being destroyed
    public GameObject monsterCarParent; 
    public GameObject finalMonsterCar; 
    private GameObject monsterCar; 
    private CarController monsterCarController;
    private CarDamage monsterCarDamage;


    [Header("Triggers")]
    [SerializeField] private Collider bossFightInitiationTrigger;
    [SerializeField] private Collider bossReviveTrigger;

    [Header("Environment")]
    [SerializeField] private Light[] lightSources;
    [SerializeField] private GameObject metalGrate;
    [SerializeField] private AudioSource metalGrateAudioSource;



    private void Start() {
        monsterCar = monsterCarParent.transform.GetChild(0).gameObject; 
        monsterCarController = monsterCar.GetComponent<CarController>();
        monsterCarDamage = monsterCar.GetComponent<CarDamage>();

        monsterCarDamage.ResetCurrentHitPoints();

        monsterCarController.enabled = false;
        monsterCarDamage.enabled = false;

        foreach (Light lightSource in lightSources)
            lightSource.enabled = true;
        
        finalMonsterCar.SetActive(false);
    }

    private void Update() {
        monsterCar = monsterCarParent.transform.GetChild(0).gameObject; 
        monsterCarController = monsterCar.GetComponent<CarController>();
        monsterCarDamage = monsterCar.GetComponent<CarDamage>();
        
        if (monsterCarDamage != null)
        {
            if (monsterCarDamage.GetCurrentHitPoints() < 3)
                TriggerSecondPhaseOfBossFight();
        } 
        else 
        {
            OpenExit();
        }
    }

    // BossFight
    public void TriggerBossFight()
    {
        Debug.Log("Been triggered!");
        monsterCarController.enabled = true;
        monsterCarDamage.enabled = true;
        monsterCarController.SetTarget(player.transform);
    }

    public void TriggerSecondPhaseOfBossFight()
    {
        foreach (Light lightSource in lightSources)
            lightSource.enabled = false;
    }

    public void TriggerFinalScene()
    {
        finalMonsterCar.SetActive(true);
        CarController finalMonsterCarController = finalMonsterCar.GetComponent<CarController>();
        finalMonsterCarController.SetTarget(player.transform);
    }

    private void OpenExit()
    {
        Destroy(metalGrate);
        metalGrateAudioSource.Play();
    }
}

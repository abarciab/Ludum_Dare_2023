using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class Resident : MonoBehaviour
{
    [System.Serializable]
    public class DialogueTriggers
    {
        [HideInInspector] public string name;
        public ItemObject trigger;
        public List<string> dialogue;
        public bool keepItem;
        public bool repeatable;
        public bool final;
    }
    [System.Serializable]
    public class Conversation
    {
        public List<string> dialogue;
    }
    [SerializeField] List<DialogueTriggers> conversations;
    [SerializeField] List<Conversation> randomConversations;    //these are displayed (in order) when this resident is given a meal that they don't have a trigger for
    [SerializeField] Conversation defaultConvo;
    public bool hungry = true;

    [Header("Pathfinding")]
    [SerializeField] List<Transform> randomPlaces;
    [SerializeField] Transform diningPlace;
    [SerializeField] Vector2 timeRangeAtPlaces;
    [SerializeField] float dinnerEatTime;
    float currentPlaceCooldown;
    AIDestinationSetter destInterface;
    AIPath pathfinder;
    bool walking;


    private void Start()
    {
        GameManager.instance.residents.Add(this);
        destInterface = GetComponent<AIDestinationSetter>();
        pathfinder = GetComponent<AIPath>();
        GameManager.DiningEvent += GoToDiningRoom;
        PickNewDest();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, GameManager.instance.player.transform.position) <= GameManager.instance.playerReach/3) pathfinder.canMove = false;
        else pathfinder.canMove = true;

        if (pathfinder.reachedEndOfPath && walking) {
            currentPlaceCooldown = destInterface.target == diningPlace ? dinnerEatTime : Random.Range(timeRangeAtPlaces.x, timeRangeAtPlaces.y);
            walking = false;
        }
        if (pathfinder.reachedEndOfPath && !walking && currentPlaceCooldown <= 0) PickNewDest();
        if (pathfinder.reachedEndOfPath) currentPlaceCooldown -= Time.deltaTime;
    }

    void PickNewDest()
    {
        destInterface.target = randomPlaces[Random.Range(0, randomPlaces.Count)];
        walking = true;
    }

    void GoToDiningRoom()
    {
        destInterface.target = diningPlace;
        walking = true;
    }

    private void OnValidate()
    {
        for (int i = 0; i < conversations.Count; i++) {
            if (conversations[i].trigger != null)
                conversations[i].name = conversations[i].trigger.name;
        }
    }

    private void OnMouseDown()
    {
        if (Vector2.Distance(transform.position, GameManager.instance.player.transform.position) > GameManager.instance.playerReach) return;
        StartConvo(GameManager.instance.inventory.selectedItem);
    }

    void StartConvo(ItemObject _item)
    {
        if (_item == null) return;
        if (_item.IsEdible() && hungry) Eat(_item);

        for (int i = 0; i < conversations.Count; i++) {
            if (conversations[i].trigger == _item) {
                GameManager.instance.StartConvo(conversations[i].dialogue, conversations[i].final ? this : null);
                if (!conversations[i].keepItem) GameManager.instance.inventory.RemoveItem(_item);
                if (!conversations[i].repeatable) conversations.RemoveAt(i);
                return;
            }
        }
        if (_item.IsEdible() && hungry) {
            if (randomConversations.Count > 0) {
                GameManager.instance.StartConvo(randomConversations[0].dialogue);
                randomConversations.RemoveAt(0);
            }
            GameManager.instance.inventory.RemoveItem(_item);
        }
        else {
            GameManager.instance.StartConvo(defaultConvo.dialogue);
        }
    }

    public void Die()
    {
        print("done dying");
        DoneDying();   
    }

    public void DoneDying()
    {
        GameManager.instance.residents.Remove(this);
        GameManager.instance.AdvanceIfReadty();
        gameObject.SetActive(false);
    }

    void Eat(ItemObject meal)
    {
        hungry = false;
        GameManager.instance.GainMoney(meal.mealValue);
    }
}

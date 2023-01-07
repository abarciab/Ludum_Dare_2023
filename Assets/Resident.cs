using System.Collections;
using System.Collections.Generic;
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
    }
    [System.Serializable]
    public class Conversation
    {
        public List<string> dialogue;
    }
    [SerializeField] List<DialogueTriggers> conversations;
    [SerializeField] List<Conversation> randomConversations;    //these are displayed (in order) when this resident is given a meal that they don't have a trigger for
    [SerializeField] Conversation defaultConvo;

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
        print("starting convo: " + _item.name);

        for (int i = 0; i < conversations.Count; i++) {
            if (conversations[i].trigger == _item) {
                GameManager.instance.StartConvo(conversations[i].dialogue);
                if (!conversations[i].keepItem) GameManager.instance.inventory.RemoveItem(_item);
                if (!conversations[i].repeatable) conversations.RemoveAt(i);
                return;
            }
        }
        if (_item.IsEdible()) {
            print("edibleeee");
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
}

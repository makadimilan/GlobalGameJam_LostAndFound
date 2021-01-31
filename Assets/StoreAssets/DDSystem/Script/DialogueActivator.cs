using System.Collections;
using System.Collections.Generic;
using Doublsb.Dialog;
using UnityEngine;
using UnityEngine.Events;

public class DialogueActivator : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        [SerializeField] string text = string.Empty;
        [SerializeField] UnityEvent callback = null;
        [SerializeField] bool isSkipable = true;

        public DialogData GetDialogData()
        {
            return new DialogData(text, callback, isSkipable);
        }
    }

    [SerializeField] string tagFilter = "Player";
    [SerializeField] DialogManager DialogManager;
    [SerializeField] List<DialogueEntry> dialogues;
    [SerializeField] int rewiredPlayerId = 0;
    [SerializeField] string skipButtonName = "Skip";

    bool isPlayerNearby = false;
    bool isShowed = false;
    Rewired.Player rewiredPlayer = null;

    void Start()
    {
        rewiredPlayer = Rewired.ReInput.players.GetPlayer(rewiredPlayerId);
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (string.IsNullOrWhiteSpace(tagFilter) || other.tag.Equals(tagFilter))
        {
            if (!isPlayerNearby && !isShowed)
            {
                var dialogTexts = new List<DialogData>();
                for(int i = 0; i < dialogues.Count; i++)
                {
                    dialogTexts.Add(dialogues[i].GetDialogData());
                }

                DialogManager.Show(dialogTexts);
                isShowed = true;
            }

            isPlayerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (string.IsNullOrWhiteSpace(tagFilter) || other.tag.Equals(tagFilter))
        {
            isPlayerNearby = false;
        }
    }

    void Update() 
    {
        if (isPlayerNearby && rewiredPlayer.GetButtonDown(skipButtonName))
        {
            DialogManager.Click_Window();
        }
    }
}

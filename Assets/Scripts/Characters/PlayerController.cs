using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DD.UI;
using DD.WebGl;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Lightbug.CharacterControllerPro.Implementation;
using Sirenix.Utilities;
using UnityEngine;

namespace DD
{
    public class PlayerController : Singleton<PlayerController>
    {
        [SerializeField] private InputHandlerSettings inputHandlerSettings;
        
        private const float TalkDistance = 2f;
        private const float ChatCheckInterval = 0.2f;

        private List<Character> npcs = new();
        private CharacterBody body;
        private NPC currentNPC;
        private bool isTalking;
        private float lastChatCheckTime;
        private NormalMovement normalMovement;
        
        /// <summary>
        /// Override singleton awake to not add to DontDestroyOnLoad
        /// </summary>
        protected override void Awake()
        {
            if (instance == null)
            {
                instance = this as PlayerController;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            npcs = Object.FindObjectsByType<Character>(FindObjectsSortMode.None).Where(c => c is NPC).ToList();
            normalMovement = GetComponentInChildren<NormalMovement>();
            SetMovementEnabled(false);
            
            GameUI.Instance.btnTalk.clicked += HandleTalk;
        }
        
        public void SetMovementEnabled(bool enabled)
        {
            if (normalMovement != null)
            {
                normalMovement.enabled = enabled;
            }
            else
            {
                StartCoroutine(SetMovementWhenReady(enabled));
            }
        }

        private IEnumerator SetMovementWhenReady(bool enabled)
        {
            Debug.Log("Waiting for movement");
            yield return new WaitUntil(() => normalMovement != null);
            SetMovementEnabled(enabled);
            Debug.Log("Set movement enabled to " + enabled);
        }

        private void HandleTalk()
        {
            if (currentNPC == null) return;

            isTalking = true;
            SetMovementEnabled(false);
            currentNPC.StartDialogue(this);
            GameUI.Instance.btnTalk.clicked -= HandleTalk;
            
            InterOp.AddChatMessage("Hola senorita 😀", true);
        }

        private void StopTalking()
        {
            Debug.Log("Stopping conversation");
            DialogueManager.Instance.EndDialogue();
            isTalking = false;
        }

        private void Update()
        {
            // If the player is talking, check if they want to exit the conversation
            if (isTalking)
            {
                var didCancel = inputHandlerSettings.InputHandler.GetBool("Cancel");
                if (didCancel)
                {
                    GameUI.Instance.SetChatHistoryActive(false);
                    StopTalking();
                    SetMovementEnabled(true);
                }
                else
                {
                    return;
                }
            }

            // Check if the player is close enough to an NPC every chatCheckInterval seconds
            if (Time.time - lastChatCheckTime > ChatCheckInterval)
            {
                CheckForNPC();
                lastChatCheckTime = Time.time;
            }
        }

        private void CheckForNPC()
        {
            // Find closest NPC
            var closest = npcs.OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).FirstOrDefault();
            if (closest.SafeIsUnityNull())
            {
                Debug.LogWarning($"No closest NPC found");
                return;
            }

            // If the player is close enough to the NPC, enable dialogue btn, else disable it
            var inRange = Vector3.Distance(transform.position, closest.transform.position) < TalkDistance;
            GameUI.Instance.SetTalkBtnActive(inRange);
            if (inRange)
            {
                var npc = closest as NPC;
                currentNPC = npc;
            }
            else
            {
                currentNPC = null;
            }
        }
    }
}
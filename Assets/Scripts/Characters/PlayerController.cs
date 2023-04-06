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
            npcs = Object.FindObjectsOfType<Character>(false).Where(c => c is NPC).ToList();
            normalMovement = GetComponentInChildren<NormalMovement>();
            
#if UNITY_WEBGL && !UNITY_EDITOR
            SetMovementEnabled(false);
#else 
            SetMovementEnabled(true);
#endif
            
            GameUI.Instance.btnTalk.clicked += HandleTalk;
        }
        
        public void SetMovementEnabled(bool canMove)
        {
            if (normalMovement != null)
            {
                normalMovement.enabled = canMove;
            }
            else
            {
                StartCoroutine(SetMovementWhenReady(canMove));
            }
        }

        private IEnumerator SetMovementWhenReady(bool canMove)
        {
            Debug.Log("Waiting for movement");
            yield return new WaitUntil(() => normalMovement != null);
            SetMovementEnabled(canMove);
            Debug.Log("Set movement enabled to " + canMove);
        }

        private void HandleTalk()
        {
            if (currentNPC == null) return;

            isTalking = true;
            SetMovementEnabled(false);
            currentNPC.StartDialogue(this);
            GameUI.Instance.btnTalk.clicked -= HandleTalk;
        }

        private void StopTalking()
        {
            Debug.Log("Stopping conversation");
            DialogueManager.Instance.EndDialogue();
            isTalking = false;
            
#if UNITY_WEBGL && !UNITY_EDITOR
            InterOp.SetChatActive(false);
#else   
            GameUI.Instance.SetChatHistoryActive(false);
            GameUI.Instance.SetChatInputActive(false);
#endif 
        }

        private void Update()
        {
            // If the player is talking, check if they want to exit the conversation
            if (isTalking)
            {
                var didCancel = inputHandlerSettings.InputHandler.GetBool("Cancel");
                if (didCancel)
                {
                    StopTalking();
                    SetMovementEnabled(true);
                }
                else
                {
                    return;
                }
            }
            
            // If the player is not talking, check if they want to talk to an NPC
            if (currentNPC != null)
            {
                var didTalk = inputHandlerSettings.InputHandler.GetBool("Interact");
                if (didTalk)
                {
                    HandleTalk();
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
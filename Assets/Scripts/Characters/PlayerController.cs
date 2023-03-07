using System.Collections.Generic;
using System.Linq;
using DD.UI;
using Lightbug.CharacterControllerPro.Core;
using Lightbug.CharacterControllerPro.Demo;
using Sirenix.Utilities;
using UnityEngine;

namespace DD
{
    public class PlayerController : Singleton<PlayerController>
    {
        private const float TalkDistance = 2f;
        private const float ChatCheckInterval = 0.2f;

        private List<Character> npcs = new();
        private CharacterBody body;
        private NPC currentNPC;
        private bool isTalking;
        private float lastChatCheckTime;
        private NormalMovement normalMovement;

        private void Start()
        {
            npcs = Object.FindObjectsByType<Character>(FindObjectsSortMode.None).Where(c => c is NPC).ToList();
            GameUI.Instance.btnTalk.clicked += HandleTalk;
            normalMovement = GetComponentInChildren<NormalMovement>();
            SetMovementEnabled(false);
        }
        
        public void SetMovementEnabled(bool enabled)
        {
            normalMovement.enabled = enabled;
        }

        private void HandleTalk()
        {
            if (currentNPC == null) return;

            // Disable CharacterControllerPro's movement
            SetMovementEnabled(false);

            GameUI.Instance.btnTalk.clicked -= HandleTalk;
            currentNPC.StartDialogue(this);
            isTalking = true;
        }

        public void StopTalking()
        {
            GameUI.Instance.btnTalk.clicked += HandleTalk;
            isTalking = false;
        }

        private void Update()
        {
            if (isTalking) return;

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
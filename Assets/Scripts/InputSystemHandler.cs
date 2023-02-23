using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Lightbug.CharacterControllerPro.Implementation;

namespace DD
{
    public sealed class InputSystemHandler : InputHandler
    {
        [SerializeField] private InputActionAsset inputActionsAsset = null;
        [SerializeField] private bool filterByActionMap = false;
        [SerializeField] private string gameplayActionMap = "Gameplay";
        [SerializeField] private bool filterByControlScheme = false;
        [SerializeField] private string controlSchemeName = "Keyboard Mouse";

        private readonly Dictionary<string, InputAction> inputActionsDictionary = new Dictionary<string, InputAction>();

        private void Awake()
        {
            if (inputActionsAsset == null)
            {
                Debug.Log("No input actions asset found!");
                return;
            }

            inputActionsAsset.Enable();

            if (filterByControlScheme)
            {
                var bindingGroup = inputActionsAsset.controlSchemes.First(x => x.name == controlSchemeName).bindingGroup;
                inputActionsAsset.bindingMask = InputBinding.MaskByGroup(bindingGroup);
            }

            var rawInputActions = new ReadOnlyArray<InputAction>();

            if (filterByActionMap)
            {
                rawInputActions = inputActionsAsset.FindActionMap(gameplayActionMap).actions;
                foreach (var t in rawInputActions) inputActionsDictionary.Add(t.name, t);
            }
            else
            {
                foreach (var action in inputActionsAsset.actionMaps.SelectMany(actionMap => actionMap.actions))
                {
                    inputActionsDictionary.Add(action.name, action);
                }
            }

            foreach (var t in rawInputActions)
            {
                inputActionsDictionary.Add(t.name, t);
            }
        }

        public override bool GetBool(string actionName)
        {
            if (!inputActionsDictionary.TryGetValue(actionName, out var inputAction))
                return false;

            return inputActionsDictionary[actionName].ReadValue<float>() >= InputSystem.settings.defaultButtonPressPoint;
        }

        public override float GetFloat(string actionName)
        {
            return !inputActionsDictionary.TryGetValue(actionName, out var inputAction) ? 0f : inputAction.ReadValue<float>();
        }

        public override Vector2 GetVector2(string actionName)
        {
            return !inputActionsDictionary.TryGetValue(actionName, out var inputAction) ? Vector2.zero : inputActionsDictionary[actionName].ReadValue<Vector2>();
        }
    }
}
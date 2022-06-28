// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""a703c810-a169-49ec-94a4-c2809b3f8e78"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""806acad3-807c-405e-91b1-485c6cf591f2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""8f21dcb9-3836-43a4-930a-fe4def8a75bd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GoLeft"",
                    ""type"": ""Button"",
                    ""id"": ""0ed6cbc6-680a-42ab-8bee-a09ee59983a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GoRight"",
                    ""type"": ""Button"",
                    ""id"": ""64307edc-e6f8-447c-a8b0-663d7b502fee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""998b6506-bbeb-44b3-87d9-e1fbd95a8933"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Shield"",
                    ""type"": ""Button"",
                    ""id"": ""8ae2236b-4011-4ccd-9f3f-76ef6a73613a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Start"",
                    ""type"": ""Button"",
                    ""id"": ""fd899b5d-72a4-4d37-954d-b5bdae4f5c2f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GoDown"",
                    ""type"": ""Button"",
                    ""id"": ""f5ba77cc-ec5d-4b9f-8f91-0eabc4c23d64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3dad8482-7295-408b-a8be-d5f561959c57"",
                    ""path"": ""<HID::USB Gamepad >/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9933d986-4bc5-447d-b427-c5580fdbe3af"",
                    ""path"": ""<HID::USB Gamepad >/trigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90e93499-7794-4c77-9635-db14acefdfb2"",
                    ""path"": ""<HID::USB Gamepad >/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GoLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ad546b43-382a-4c8a-9b06-b2ca88d063b9"",
                    ""path"": ""<HID::USB Gamepad >/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GoRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7f17d52d-1fbf-495a-902d-91c11d35ff62"",
                    ""path"": ""<HID::USB Gamepad >/button6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""60699f65-258c-456d-8276-043c974db367"",
                    ""path"": ""<HID::USB Gamepad >/button5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shield"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83d29cc9-f227-4a45-b3e6-d3d4d9236ccf"",
                    ""path"": ""<HID::USB Gamepad >/button10"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Start"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""142ae148-2217-4722-a1f0-c85c55f4ab6f"",
                    ""path"": ""<HID::USB Gamepad >/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GoDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Shoot = m_Gameplay.FindAction("Shoot", throwIfNotFound: true);
        m_Gameplay_GoLeft = m_Gameplay.FindAction("GoLeft", throwIfNotFound: true);
        m_Gameplay_GoRight = m_Gameplay.FindAction("GoRight", throwIfNotFound: true);
        m_Gameplay_Dash = m_Gameplay.FindAction("Dash", throwIfNotFound: true);
        m_Gameplay_Shield = m_Gameplay.FindAction("Shield", throwIfNotFound: true);
        m_Gameplay_Start = m_Gameplay.FindAction("Start", throwIfNotFound: true);
        m_Gameplay_GoDown = m_Gameplay.FindAction("GoDown", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Shoot;
    private readonly InputAction m_Gameplay_GoLeft;
    private readonly InputAction m_Gameplay_GoRight;
    private readonly InputAction m_Gameplay_Dash;
    private readonly InputAction m_Gameplay_Shield;
    private readonly InputAction m_Gameplay_Start;
    private readonly InputAction m_Gameplay_GoDown;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Shoot => m_Wrapper.m_Gameplay_Shoot;
        public InputAction @GoLeft => m_Wrapper.m_Gameplay_GoLeft;
        public InputAction @GoRight => m_Wrapper.m_Gameplay_GoRight;
        public InputAction @Dash => m_Wrapper.m_Gameplay_Dash;
        public InputAction @Shield => m_Wrapper.m_Gameplay_Shield;
        public InputAction @Start => m_Wrapper.m_Gameplay_Start;
        public InputAction @GoDown => m_Wrapper.m_Gameplay_GoDown;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Shoot.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                @GoLeft.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGoLeft;
                @GoLeft.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGoLeft;
                @GoLeft.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGoLeft;
                @GoRight.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGoRight;
                @GoRight.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGoRight;
                @GoRight.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGoRight;
                @Dash.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDash;
                @Shield.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShield;
                @Shield.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShield;
                @Shield.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShield;
                @Start.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Start.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @Start.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnStart;
                @GoDown.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGoDown;
                @GoDown.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGoDown;
                @GoDown.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnGoDown;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @GoLeft.started += instance.OnGoLeft;
                @GoLeft.performed += instance.OnGoLeft;
                @GoLeft.canceled += instance.OnGoLeft;
                @GoRight.started += instance.OnGoRight;
                @GoRight.performed += instance.OnGoRight;
                @GoRight.canceled += instance.OnGoRight;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Shield.started += instance.OnShield;
                @Shield.performed += instance.OnShield;
                @Shield.canceled += instance.OnShield;
                @Start.started += instance.OnStart;
                @Start.performed += instance.OnStart;
                @Start.canceled += instance.OnStart;
                @GoDown.started += instance.OnGoDown;
                @GoDown.performed += instance.OnGoDown;
                @GoDown.canceled += instance.OnGoDown;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnGoLeft(InputAction.CallbackContext context);
        void OnGoRight(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnShield(InputAction.CallbackContext context);
        void OnStart(InputAction.CallbackContext context);
        void OnGoDown(InputAction.CallbackContext context);
    }
}

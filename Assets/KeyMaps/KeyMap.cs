//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/KeyMaps/KeyMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @KeyMap: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @KeyMap()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""KeyMap"",
    ""maps"": [
        {
            ""name"": ""Controls"",
            ""id"": ""e422c075-9832-40df-992f-dae170c0e107"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""e45b1397-04c5-412f-86fb-1bf16e2bacc6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""61281b0e-e222-4d0d-953c-e93fbcc78a87"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Tests"",
            ""id"": ""c5f5ffc4-6a8b-417b-92f7-0eedf03d9552"",
            ""actions"": [
                {
                    ""name"": ""SpawnItem"",
                    ""type"": ""Button"",
                    ""id"": ""0ef4bdd9-4a87-4bc1-8575-6b28692df1fc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RemoveItem"",
                    ""type"": ""Button"",
                    ""id"": ""04539405-59da-45fd-809c-38adae25c265"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bdf8058a-15d4-48a2-973d-155e45224f6f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpawnItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af7ef960-7ae2-45c0-b895-45ec724ba8a1"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RemoveItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Controls
        m_Controls = asset.FindActionMap("Controls", throwIfNotFound: true);
        m_Controls_Movement = m_Controls.FindAction("Movement", throwIfNotFound: true);
        // Tests
        m_Tests = asset.FindActionMap("Tests", throwIfNotFound: true);
        m_Tests_SpawnItem = m_Tests.FindAction("SpawnItem", throwIfNotFound: true);
        m_Tests_RemoveItem = m_Tests.FindAction("RemoveItem", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Controls
    private readonly InputActionMap m_Controls;
    private List<IControlsActions> m_ControlsActionsCallbackInterfaces = new List<IControlsActions>();
    private readonly InputAction m_Controls_Movement;
    public struct ControlsActions
    {
        private @KeyMap m_Wrapper;
        public ControlsActions(@KeyMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Controls_Movement;
        public InputActionMap Get() { return m_Wrapper.m_Controls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlsActions set) { return set.Get(); }
        public void AddCallbacks(IControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_ControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ControlsActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
        }

        private void UnregisterCallbacks(IControlsActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
        }

        public void RemoveCallbacks(IControlsActions instance)
        {
            if (m_Wrapper.m_ControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_ControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ControlsActions @Controls => new ControlsActions(this);

    // Tests
    private readonly InputActionMap m_Tests;
    private List<ITestsActions> m_TestsActionsCallbackInterfaces = new List<ITestsActions>();
    private readonly InputAction m_Tests_SpawnItem;
    private readonly InputAction m_Tests_RemoveItem;
    public struct TestsActions
    {
        private @KeyMap m_Wrapper;
        public TestsActions(@KeyMap wrapper) { m_Wrapper = wrapper; }
        public InputAction @SpawnItem => m_Wrapper.m_Tests_SpawnItem;
        public InputAction @RemoveItem => m_Wrapper.m_Tests_RemoveItem;
        public InputActionMap Get() { return m_Wrapper.m_Tests; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestsActions set) { return set.Get(); }
        public void AddCallbacks(ITestsActions instance)
        {
            if (instance == null || m_Wrapper.m_TestsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_TestsActionsCallbackInterfaces.Add(instance);
            @SpawnItem.started += instance.OnSpawnItem;
            @SpawnItem.performed += instance.OnSpawnItem;
            @SpawnItem.canceled += instance.OnSpawnItem;
            @RemoveItem.started += instance.OnRemoveItem;
            @RemoveItem.performed += instance.OnRemoveItem;
            @RemoveItem.canceled += instance.OnRemoveItem;
        }

        private void UnregisterCallbacks(ITestsActions instance)
        {
            @SpawnItem.started -= instance.OnSpawnItem;
            @SpawnItem.performed -= instance.OnSpawnItem;
            @SpawnItem.canceled -= instance.OnSpawnItem;
            @RemoveItem.started -= instance.OnRemoveItem;
            @RemoveItem.performed -= instance.OnRemoveItem;
            @RemoveItem.canceled -= instance.OnRemoveItem;
        }

        public void RemoveCallbacks(ITestsActions instance)
        {
            if (m_Wrapper.m_TestsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ITestsActions instance)
        {
            foreach (var item in m_Wrapper.m_TestsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_TestsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public TestsActions @Tests => new TestsActions(this);
    public interface IControlsActions
    {
        void OnMovement(InputAction.CallbackContext context);
    }
    public interface ITestsActions
    {
        void OnSpawnItem(InputAction.CallbackContext context);
        void OnRemoveItem(InputAction.CallbackContext context);
    }
}

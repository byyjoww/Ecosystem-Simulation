using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RawAxisInputController : MonoBehaviour
{
    #region INITIALIZE
    bool isEnabled;
    
    public void OnEnable()
    {
        Debug.Log($"Initiating a PlayerInputController.");
        Init();
    }

    public void Init()
    {
        if (isEnabled)
        {
            return;
        }

        isEnabled = true;
    }
    #endregion

    #region UPDATE
    private void Update()
    {
        CheckForVertical();
        CheckForHorizontal();
        CheckForAbility();
    }
    #endregion

    #region BASE_MOVEMENT
    [Header("Vertical Input")]
    [SerializeField] private bool useRawVInput = true;
    [ReadOnly, SerializeField] private float verticalInput;
    [SerializeField] private FloatScriptableEvent OnReceiveVerticalInput;
    private void CheckForVertical()
    {
        if (!isEnabled || !useRawVInput)
        {
            return;
        }

        verticalInput = Input.GetAxis("Vertical");
        if (OnReceiveVerticalInput)
        {
            OnReceiveVerticalInput.Raise(verticalInput);
        }
        else
        {
            Debug.LogError($"OnReceiveVerticalInput isn't assigned!");
        }
        
    }

    [Header("Horizontal Input")]
    [SerializeField] private bool useRawHInput = true;
    [ReadOnly, SerializeField] private float horizontalInput;
    [SerializeField] private FloatScriptableEvent OnReceiveHorizontalInput;
    private void CheckForHorizontal()
    {
        if (!isEnabled || !useRawHInput)
        {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        if (OnReceiveHorizontalInput)
        {
            OnReceiveHorizontalInput.Raise(horizontalInput);
        }
        else
        {
            Debug.LogError($"OnReceiveHorizontalInput isn't assigned!");
        }        
    }

    #endregion

    #region SKILLS
    [Header("Shortcuts")]
    //[SerializeField] private ShortcutData shortcutTable;

    [SerializeField] BoolScriptableEvent OnJump;
    [SerializeField] BoolScriptableEvent OnSprint;
    [SerializeField] BoolScriptableEvent OnShoot;
    [SerializeField] BoolScriptableEvent OnAim;

    private void CheckForAbility()
    {
        if (!isEnabled)
        {
            return;
        }

        //JUMP
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump.Raise(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJump.Raise(false);
        }

        //SPRINT
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnSprint.Raise(true);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            OnSprint.Raise(false);
        }

        //SHOOT
        if (Input.GetMouseButtonDown(0))
        {
            OnShoot.Raise(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnShoot.Raise(false);
        }

        //SHOOT
        if (Input.GetMouseButtonDown(1))
        {
            OnAim.Raise(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            OnAim.Raise(false);
        }
    }
    #endregion
}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// Input Manager
// Detects mouse inputs and selects entities
// When a unit is selected, mouse events are delegated to the appropriate unit class


class InputManager : MonoBehaviour {
    private IList<Entity> selected;
    private Vector3 mouse_one_pressed;
    private Vector3 mouse_one_released;
    public Texture box_select_texture;
    private bool drawBox;

    public PlayerUI ui;

    public bool GameOver = false;


    void Start()
    {
        selected = new List<Entity>();
        drawBox = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckSelectedUnits();
        HandleInput();
        if (GameOver){
            ui.endGame();
        }
    }

    private void CheckSelectedUnits()
    {
        IList<Entity> to_remove = new List<Entity>();

        foreach (Entity E in selected)
        {
            if (E == null)
            {
                to_remove.Add(E);
            }
        }

        foreach (Entity E in to_remove)
        {
            if (E == null)
            {
                selected.Remove(E);
            }
        }
    }

    void HandleInput()
    {
        if (!GameOver){
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseOneEvent();
                mouse_one_pressed = Input.mousePosition;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                HandleMouseTwoEvent();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mouse_one_released = Input.mousePosition;
                if (mouse_one_pressed != mouse_one_released)
                    BoxSelectGO();
                drawBox = false;
            }
            else if (Input.GetMouseButton(0))
            {
                drawBox = true;
            }

            if (Input.GetKey(KeyCode.Delete))
            {
                deleteSelected();
            }
        }

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void HandleMouseOneEvent()
    {
        RaycastHit hit;
        // Stop checking MouseDown if the mouse is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (selected != null && !Input.GetKey("left ctrl") && !Input.GetKey("left shift"))
        {
            DeselectGO();
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("Choosable")))
        {
            SelectGO(hit.transform.gameObject);
        }

    }

    private void target(RaycastHit hit, Entity.TypeOfTarget type)
    {
        foreach (Entity E in selected)
        {
            E.handleReceiveTarget(hit, type);
        }
    }

    void deleteSelected()
    {
        for (int i = selected.Count - 1; i >= 0; i--)
        {
            Entity E = selected[i];
            if (E.CompareTag("Player"))
            {
                if (E.entityName != "Town Center")
                {
                    E.Delete();
                    selected.RemoveAt(i);
                }
            }
        }
    }

    void HandleMouseTwoEvent()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("Enemy"))
        || Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("Choosable")))
        {
            target(hit, Entity.TypeOfTarget.Entity);
        }
        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("Ground")))
        {
            target(hit, Entity.TypeOfTarget.GroundPosition);
        }

    }

    void SelectGO(GameObject GO)
    {
        Entity en = GO.GetComponent<Entity>();
        //Debug.Log(GO.name);
        if (!en) return;
        if (!selected.Contains(en))
        {
            en.setSelected(true);
            selected.Add(en);
            if (selected.Count == 1)
            {
                ui.setSelectedEntity(en);
            }
            else if (selected.Count > 1)
            {
                ui.selectMany(selected.Count);
            }
        }
    }

    void BoxSelectGO()
    {
        Vector2 max = new Vector2(Mathf.Max(mouse_one_pressed.x, mouse_one_released.x), Mathf.Max(mouse_one_pressed.y, mouse_one_released.y));
        Vector2 min = new Vector2(Mathf.Min(mouse_one_pressed.x, mouse_one_released.x), Mathf.Min(mouse_one_pressed.y, mouse_one_released.y));
        GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("Player");
        bool empty = selected.Count == 0;

        foreach (GameObject GO in playerUnits)
        {
            Vector3 cameraPoint = Camera.main.WorldToScreenPoint(GO.transform.position);
            if (max.x > cameraPoint.x && min.x < cameraPoint.x && max.y > cameraPoint.y && min.y < cameraPoint.y)
            {
                SelectGO(GO);
            }
        }

    }

    void OnGUI()
    {
        if (drawBox)
        {
            Rect boxArea = new Rect(new Vector2(mouse_one_pressed.x, Screen.height - mouse_one_pressed.y), new Vector2(Input.mousePosition.x - mouse_one_pressed.x, mouse_one_pressed.y - Input.mousePosition.y));
            GUI.DrawTexture(boxArea, box_select_texture);
        }
    }


    void DeselectGO()
    {
        foreach (Entity U in selected)
        {
            U.setSelected(false);
        }

        selected.Clear();
        ui.deselect();
    }
}


using UnityEngine;
using System.Collections.Generic;

class InputManager : MonoBehaviour {
    private IList<Unit> selected;
    private Vector3 mouse_one_pressed;
    private Vector3 mouse_one_released;

    void Start()
    {
        selected = new List<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
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
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void HandleMouseOneEvent()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("Ground")))
        {
            MoveSelected(hit.point);
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("Choosable")))
        {
            SelectGO(hit.transform.gameObject);
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("Enemy")))
        {
            SetTarget(hit.transform.gameObject);
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("Building")))
        {
            ActivateBulding(hit.transform.gameObject, hit.point);
        }
    }

    private void ActivateBulding(GameObject GO, Vector3 V)
    {
        GO.GetComponent<UnitSpawner>().SpawnEntity();
    }

    private void SetTarget(GameObject GO)
    {
        foreach (Unit U in selected)
        {
            MoveablePiece MP = (MoveablePiece)U;
            MP.SetTarget(GO);
        }
    }

    void HandleMouseTwoEvent()
    {
        if (selected != null)
        {
            DeselectGO();
        }
    }

    void SelectGO(GameObject GO)
    {
        MoveablePiece MP = GO.GetComponent<MoveablePiece>();
        MP.setSelected(true);

        if ((selected.Count == 0 || Input.GetKey("left ctrl")) && !selected.Contains(MP))
        {
            selected.Add(MP);
        }
        else
        {
            DeselectGO();
            selected.Clear();
            selected.Add(MP);
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
                MoveablePiece MP = GO.GetComponent<MoveablePiece>();
                MP.setSelected(true);
                if ((empty || Input.GetKey("left ctrl")) && !selected.Contains(MP))
                {
                    selected.Add(MP);
                }
                else
                {
                    DeselectGO();
                    selected.Clear();
                    selected.Add(MP);
                    empty = true;
                }
            }
        }

    }

    void MoveSelected(Vector3 V)
    {
        foreach (Unit U in selected)
        {
            MoveablePiece MP = (MoveablePiece)U;
            MP.SetFuturePosition(V);
        }
    }

    void DeselectGO()
    {
        foreach (Unit U in selected)
        {
            U.setSelected(false);
        }

        selected.Clear();
    }
}


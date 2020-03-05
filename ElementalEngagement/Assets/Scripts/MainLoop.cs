using UnityEngine;

public class MainLoop : MonoBehaviour {
    // Start is called before the first frame update
    public Material selected_mat;
    private Material selected_base_mat;
    private GameObject selected;

    void Start()
    {

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
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleMouseTwoEvent();
        }
    }

    void HandleMouseOneEvent()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("Ground")))
        {
            MoveSelected(hit.point);
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 250.0f, LayerMask.GetMask("Choosable")) && selected == null)
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
        if (selected == null)
        {
            GO.GetComponent<UnitSpawner>().SpawnEntity();
            GO.GetComponent<UnitSpawner>().SetWaypoint(V);
        }
    }

    private void SetTarget(GameObject GO)
    {
        if (selected != null)
        {
            selected.GetComponent<MoveablePiece>().SetTarget(GO);
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
        selected = GO;
        Renderer R = selected.GetComponent<Renderer>();
        selected_base_mat = R.material;
        R.material = selected_mat;

		selected.GetComponent<MoveablePiece>().setSelected(true);

	}

    void MoveSelected(Vector3 V)
    {
        if (selected != null)
        {
            selected.GetComponent<MoveablePiece>().SetFuturePosition(V);
        }
    }

    void DeselectGO()
    {

		selected.GetComponent<MoveablePiece>().setSelected(false);

		Renderer R = selected.GetComponent<Renderer>();
        R.material = selected_base_mat;
        selected = null;
        selected_base_mat = null;
    }
}

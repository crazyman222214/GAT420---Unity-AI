using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    [SerializeField] AIAgent[] agents;
    [SerializeField] LayerMask layerMask;

    int index = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) index = ++index % agents.Length;

        if (Input.GetMouseButtonDown(0) )
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, layerMask))
            {
                Instantiate(agents[index], hitInfo.point, Quaternion.identity);
            }
        }
    }
}

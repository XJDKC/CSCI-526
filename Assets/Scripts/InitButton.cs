using UnityEngine;
using UnityEngine.EventSystems;

public class InitButton : MonoBehaviour
{
    private GameObject _lastSelect;

    // Start is called before the first frame update
    void Start()
    {
        _lastSelect = new GameObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {
            _lastSelect = EventSystem.current.currentSelectedGameObject;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject((_lastSelect));
        }
    }
}

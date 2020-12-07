using UnityEngine;

namespace Painting
{
    public class OpenLinesMenu : MonoBehaviour
    {
        [SerializeField] private GameObject linesMenu;

        public void CreateLinesMenu()
        {
            Debug.Log("in");
            Instantiate(linesMenu, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

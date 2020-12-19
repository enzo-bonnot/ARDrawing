using UnityEngine;

namespace Painting
{
    public class OpenLinesMenu : MonoBehaviour
    {
        [SerializeField] private GameObject linesMenu;

        public void CreateLinesMenu()
        {
            Instantiate(linesMenu, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

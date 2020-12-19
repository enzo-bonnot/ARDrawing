using UnityEngine;

namespace Painting
{
    public class ColorPicker : MonoBehaviour
    {
        [SerializeField]
        private HSVPicker.ColorPicker picker;

        [SerializeField] private Material material;
        // Start is called before the first frame update
        void Start()
        {
            picker.onValueChanged.AddListener(color =>
            {
                material.color = color;
            });
            material.color = picker.CurrentColor;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

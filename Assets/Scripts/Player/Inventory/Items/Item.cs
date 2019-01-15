using UnityEngine;

namespace Assets.Scripts.Player.Inventory.Items
{
    [CreateAssetMenu(menuName = "Items/Item")]
    public class Item : ScriptableObject
    {

        #region Fields

        [SerializeField] private int _id;
        [SerializeField] private string _name;
        [SerializeField] private Sprite _icon;

        #endregion

        #region Properties

        public int Id => _id;
        public string Name => _name;
        public Sprite Icon => _icon;

        #endregion

    }
}

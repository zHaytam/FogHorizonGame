using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerBehaviour : Singleton<PlayerBehaviour>
    {

        #region Fields

        private SkeletonMecanim _skeletonMecanim;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _skeletonMecanim = GetComponent<SkeletonMecanim>();
        }

        #endregion

        #region Public Methods

        public void ChangeSlotAttachment(string slotName, string toAttachment)
        {
            _skeletonMecanim.Skeleton.SetAttachment(slotName, toAttachment);
        }

        #endregion

    }
}

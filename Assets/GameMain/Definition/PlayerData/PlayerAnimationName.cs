using UnityEngine;
using GameFramework;

namespace KSG
{
    public class PlayerAnimationName : IReference
    {
        [Header("Animation参数命名")]
        [SerializeField] private string IsRunningParameterName = "isRunning";
        [SerializeField] private string SpeedParameterName = "Speed";
        [SerializeField] private string IsAimParameterName = "Aim";
        [SerializeField] private string IsShootParameterName = "Shoot";
        [SerializeField] private string ShootAnimationName = "Momoi_Original_Normal_Attack_Ing";


        public int isRunningParameterHash { get; private set; }
        public int SpeedParameterHash { get; private set; }
        public int isAimParameterName { get; private set; }
        public int isShootParameterName { get; private set; }
        public int shootAnimationName { get; private set; }


        public void InitializeData()
        {
            isRunningParameterHash = Animator.StringToHash(IsRunningParameterName);
            SpeedParameterHash = Animator.StringToHash(SpeedParameterName);
            isAimParameterName = Animator.StringToHash(IsAimParameterName);
            isShootParameterName = Animator.StringToHash(IsShootParameterName);
            
            shootAnimationName = Animator.StringToHash(ShootAnimationName);
            
        }
        
        public static PlayerAnimationName Create()
        {
            PlayerAnimationName state = ReferencePool.Acquire<PlayerAnimationName>();
            return state;
        }
        public void Clear()
        {
            isRunningParameterHash = 0;
            SpeedParameterHash = 0;
            isAimParameterName = 0;
            isShootParameterName = 0;
            shootAnimationName = 0;
        }
    }
}
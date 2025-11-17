using System;
using UnityEngine;

namespace KSG
{
    public class PlayerAnimationName
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
    }
}
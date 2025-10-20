using System;
using UnityEngine;

namespace KSG
{
    public class PlayerAnimationName
    {
        [Header("Animation参数命名")]
        [SerializeField] private string IsRunningParameterName = "isRunning";
        [SerializeField] private string SpeedParameterName = "Speed";

        public int isRunningParameterHash { get; private set; }
        public int SpeedParameterHash { get; private set; }
        public void InitializeData()
        {
            isRunningParameterHash = Animator.StringToHash(IsRunningParameterName);
            SpeedParameterHash = Animator.StringToHash(SpeedParameterName);
        }
    }
}
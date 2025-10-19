using System;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;

namespace KSG
{
	public class PlayerLogic : Entity
	{
		[Header("玩家输入")] 
		public bool isRunning = false;
		public Vector2 PlayerMoveInput = Vector2.zero;
		
		[Header("默认挂载")]
		public Rigidbody2D rb;
		public Animator  animator;
        public CharacterController characterController;
		
		protected PlayerInputAction inputAction;
		protected PlayerInputAction.PlayerActions PlayerActions;

		[Header("运行数据")]
		public PlayerData playerData;
		public PlayerAnimationName playerAnimationName;

		protected IFsm<PlayerLogic> fsm;
		protected List<FsmState<PlayerLogic>> stateList;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);

			playerData = userData as PlayerData;

			playerAnimationName = new PlayerAnimationName();
		}

		//TODO  fist 创建有限状态机
	}
}
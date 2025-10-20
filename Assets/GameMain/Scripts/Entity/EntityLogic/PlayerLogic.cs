using System;
using System.Collections.Generic;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;

namespace KSG
{
	public class PlayerLogic : Entity
	{
		[Header("玩家输入")]
		public bool isRunning = false;
		public Vector2 playerMoveInput = Vector2.zero;

		[Header("默认挂载")]
		public Rigidbody2D rb;
		public Animator animator;
		public CharacterController characterController;

		protected PlayerInputAction inputAction;
		protected PlayerInputAction.PlayerActions PlayerActions;

		[Header("运行数据")]
		public PlayerData playerData;
		public PlayerAnimationName playerAnimationName;
        public Vector3 playerMovement = Vector3.zero;


		protected IFsm<PlayerLogic> fsm;
		protected List<FsmState<PlayerLogic>> stateList;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);

			playerData = userData as PlayerData;

			playerAnimationName = new PlayerAnimationName();
			PlayerActions = inputAction.Player;
		}

		protected override void OnShow(object userData)
		{
			base.OnShow(userData);

			if (playerData == null)
			{
				Log.Error("Player data is invalid");
				return;
			}
			CreateFsm();
			inputAction.Enable();
			playerAnimationName.InitializeData();
			AddInputActionsCallbacks(); // 添加输入回调
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);
		}

		protected void CreateFsm()
		{
			AddFsmState();
			fsm = GameEntry.Fsm.CreateFsm<PlayerLogic>(gameObject.name, this, stateList.ToArray());
			StartState();
		}

		protected void AddFsmState()
		{
			stateList.Add(PlayerIdleState.Create());
			stateList.Add(PlayerMoveState.Create());
			stateList.Add(PlayerRunState.Create());
		}

		protected void StartState()
		{
			fsm.Start<PlayerIdleState>();
		}

		private void OnDestroy()
		{
			GameEntry.Fsm.DestroyFsm(fsm);
		}

		#region Input Function
		protected virtual void AddInputActionsCallbacks()
		{
			PlayerActions.Move.canceled += OnMovementCanceled;
			PlayerActions.Move.performed += GetplayerMoveInput;
			PlayerActions.Run.performed += GetRunInput;
		}

		protected virtual void RemoveInputActionsCallbacks()
		{
			PlayerActions.Move.canceled -= OnMovementCanceled;
			PlayerActions.Move.performed -= GetplayerMoveInput;
			PlayerActions.Run.performed -= GetRunInput;
		}

		void GetplayerMoveInput(InputAction.CallbackContext context)
		{
			playerMoveInput = context.ReadValue<Vector2>();
		}
		void OnMovementCanceled(InputAction.CallbackContext context)
		{
			playerMoveInput = Vector2.zero;
		}

        void GetRunInput(InputAction.CallbackContext ctx)
        {
            isRunning = !isRunning;
        }
		#endregion

		#region Animation Function
		public void PlayAnimation(int animationHash, float value, float dampvale = 0.1f)
		{
			animator.SetFloat(animationHash, value, dampvale, Time.deltaTime);
		}

		public void PlayAnimation(int animationHash)
		{
			animator.Play(animationHash);
		}

		public void StartAnimation(int animationHash)
		{
			animator.SetBool(animationHash, true);
		}

		public void StopAnimation(int animationHash)
		{
			animator.SetBool(animationHash, false);
		}

		#endregion
	}
}
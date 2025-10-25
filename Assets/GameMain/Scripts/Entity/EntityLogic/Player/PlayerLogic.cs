using System;
using System.Collections.Generic;
using GameFramework.Event;
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

		public bool isAim = false;
		public bool isShoot = false;

		[Header("默认挂载")]
		public Rigidbody rb;
		public Animator animator;
		public Transform CameraParent;
		public CharacterController characterController;

		[Header("运行数据")]
		public PlayerData playerData;
		public PlayerAnimationName playerAnimationName;
		public Vector3 playerMovement = Vector3.zero;
		protected PlayerInputAction inputActions;
		protected PlayerInputAction.PlayerActions PlayerActions;
		protected CameraData cameraData;
		protected CameraLogic cameraEntityLogic;
		protected IFsm<PlayerLogic> MoveFsmManager;
		protected IFsm<PlayerLogic> ShootFsmManager;
		protected List<FsmState<PlayerLogic>> MoveStateList;
		protected List<FsmState<PlayerLogic>> ShootStateList;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowVirtualCameraSuccess);

			playerData = userData as PlayerData;

			playerAnimationName = new PlayerAnimationName();
			inputActions = new PlayerInputAction();
			MoveStateList = new List<FsmState<PlayerLogic>>();
			ShootStateList = new List<FsmState<PlayerLogic>>();
			PlayerActions = inputActions.Player;

			rb = GetComponent<Rigidbody>();
			animator = GetComponent<Animator>();
			characterController = GetComponent<CharacterController>();
		}

		protected override void OnShow(object userData)
		{
			base.OnShow(userData);

			if (playerData == null)
			{
				Log.Error("Player data is invalid");
				return;
			}

			CreateMoveFsm();
			CreateShootFsm();
			ShowVirtualCamera();
			inputActions.Enable();
			AddInputActionsCallbacks(); // 添加输入回调
			playerAnimationName.InitializeData();
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);
		}

		protected override void OnHide(bool isShutdown, object userData)
		{
			base.OnHide(isShutdown, userData);

			GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowVirtualCameraSuccess);

			inputActions.Disable();
			RemoveInputActionsCallbacks(); // 移除输入回调
		}
		private void OnDestroy()
		{
			GameEntry.Fsm.DestroyFsm(MoveFsmManager);
			GameEntry.Fsm.DestroyFsm(ShootFsmManager);
		}
		private void ShowVirtualCamera()
		{
			CameraParent = GameObject.Find("CameraRoot").transform;
			cameraData = new CameraData(GameEntry.Entity.GenerateSerialId(), (int)EnumCamera.PlayerCamera);
			GameEntry.Entity.ShowCamera(cameraData);
		}
		/// <summary>
		/// 显示虚拟相机成功
		/// </summary>
		private void OnShowVirtualCameraSuccess(object sender, GameEventArgs e)
		{
			ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
			if (ne.EntityLogicType != typeof(CameraLogic))
			{
				return;
			}
			// GameEntry.Entity.AttachEntity(GameEntry.Entity.GetEntity(cameraData.entityId), this.Entity, CameraParent, cameraData);
			cameraEntityLogic = GameEntry.Entity.GetEntity(cameraData.Id).Logic as CameraLogic;
			cameraEntityLogic.SetTarget(CameraParent);
		}

		#region 状态机函数
		protected void CreateMoveFsm()
		{
			AddFsmMoveState();
			MoveFsmManager = GameEntry.Fsm.CreateFsm<PlayerLogic>(gameObject.name + " PlayerMoveFsm", this, MoveStateList.ToArray());
			StartMoveState();
		}
		protected void CreateShootFsm()
		{
			AddFsmShootState();
			ShootFsmManager = GameEntry.Fsm.CreateFsm<PlayerLogic>(gameObject.name + " PlayerShootFsm", this, ShootStateList.ToArray());
			StartShootState();
        }
		protected void AddFsmMoveState()
		{
			MoveStateList.Add(PlayerIdleState.Create());
			MoveStateList.Add(PlayerMoveState.Create());
			MoveStateList.Add(PlayerRunState.Create());
		}

		protected void AddFsmShootState()
		{
			ShootStateList.Add(PlayerNormalState.Create());
			ShootStateList.Add(PlayerAimState.Create());
			ShootStateList.Add(PlayerShootState.Create());
        }
		protected void StartMoveState()
		{
			MoveFsmManager.Start<PlayerIdleState>();
		}
		protected void StartShootState()
        {
			ShootFsmManager.Start<PlayerNormalState>();
        }
		#endregion
		#region Input Function
		protected virtual void AddInputActionsCallbacks()
		{
			PlayerActions.Move.canceled += OnMovementCanceled;
			PlayerActions.Move.performed += GetplayerMoveInput;
			PlayerActions.Run.performed += GetRunInput;

			PlayerActions.Aim.performed += OnPlayerAimPerformed;
			PlayerActions.Aim.canceled += OnPlayerAimcanceled;

			PlayerActions.Shoot.performed += OnPlayerShootPerformed;
			PlayerActions.Shoot.canceled += OnPlayerShootCanceled;
		}

		protected virtual void RemoveInputActionsCallbacks()
		{
			PlayerActions.Move.canceled -= OnMovementCanceled;
			PlayerActions.Move.performed -= GetplayerMoveInput;
			PlayerActions.Run.performed -= GetRunInput;

			PlayerActions.Aim.performed -= OnPlayerAimPerformed;
			PlayerActions.Aim.canceled -= OnPlayerAimcanceled;

			PlayerActions.Shoot.performed -= OnPlayerShootPerformed;
			PlayerActions.Shoot.canceled -= OnPlayerShootCanceled;
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

		void OnPlayerAimPerformed(InputAction.CallbackContext context)
		{
			isAim = true;
		}
		void OnPlayerAimcanceled(InputAction.CallbackContext context)
		{
			isAim = false;
		}
		void OnPlayerShootPerformed(InputAction.CallbackContext context)
        {
			isShoot = true;
        }
		void OnPlayerShootCanceled(InputAction.CallbackContext context)
        {
			isShoot = false;
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
			if (animator.GetBool(animationHash))
            {
				return;
            }

			animator.SetBool(animationHash, true);
		}

		public void StopAnimation(int animationHash)
		{
			if (!animator.GetBool(animationHash))
			{
				return;
			}
			
			animator.SetBool(animationHash, false);
		}

		#endregion
	}
}
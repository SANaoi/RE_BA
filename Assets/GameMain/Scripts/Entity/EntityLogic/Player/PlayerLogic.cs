using System;
using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.Animations.Rigging;
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
		protected Rigidbody rb;
		protected Animator animator;
		protected Transform CameraParent;
		protected CharacterController characterController;
		public MultiParentConstraint WeaponRigging;
		public MultiAimConstraint AimRigging;

		[Header("运行数据")]
		public bool isAimOrShootState = false;
		public float targetWeaponRiggingWeight;
		public float targetAimRiggingWeight;

		public PlayerData playerData;
		public UnityEngine.Camera m_Camera;
		public PlayerAnimationName playerAnimationName;
		public Vector3 playerMovement = Vector3.zero;
		protected PlayerInputAction inputActions;
		protected PlayerInputAction.PlayerActions PlayerActions;
		public CameraData cameraData;
		public CameraLogic cameraEntityLogic;
		protected IFsm<PlayerLogic> MoveFsmManager;
		protected IFsm<PlayerLogic> ShootFsmManager;
		protected List<FsmState<PlayerLogic>> MoveStateList;
		protected List<FsmState<PlayerLogic>> ShootStateList;

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowVirtualCameraSuccess);

			m_Camera = UnityEngine.Camera.main;
			playerData = userData as PlayerData;

			playerAnimationName = new PlayerAnimationName();
			playerAnimationName.InitializeData();

			inputActions = new PlayerInputAction();
			MoveStateList = new List<FsmState<PlayerLogic>>();
			ShootStateList = new List<FsmState<PlayerLogic>>();
			PlayerActions = inputActions.Player;

			rb = GetComponentInChildren<Rigidbody>();
			animator = GetComponentInChildren<Animator>();
			AimRigging = GetComponentInChildren<MultiAimConstraint>();
			WeaponRigging = GetComponentInChildren<MultiParentConstraint>();
			characterController = GetComponentInChildren<CharacterController>();
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
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);

			Move();
			SetRigWeight();
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
			// GameEntry.Entity.AttachEntity(GameEntry.Entity.GetEntity(cameraData.Id), this.Entity, CameraParent, cameraData);
			cameraEntityLogic = GameEntry.Entity.GetEntity(cameraData.Id).Logic as CameraLogic;
			cameraEntityLogic.SetTarget(CameraParent);
		}

		private void SetRigWeight()
		{
			if (AimRigging.weight != targetAimRiggingWeight)
			{
				AimRigging.weight = Mathf.Lerp(AimRigging.weight, targetAimRiggingWeight, 4 * Time.deltaTime);
				AimRigging.weight = AimRigging.weight >= 0.01f ? AimRigging.weight : 0f;
				AimRigging.weight = AimRigging.weight <= 0.99f ? AimRigging.weight : 1f;

			}
			if (WeaponRigging.weight != targetWeaponRiggingWeight)
			{
				WeaponRigging.weight = Mathf.Lerp(WeaponRigging.weight, targetWeaponRiggingWeight, 4 * Time.deltaTime);
				WeaponRigging.weight = WeaponRigging.weight >= 0.01f ? WeaponRigging.weight : 0f;
				WeaponRigging.weight = WeaponRigging.weight <= 0.99f ? WeaponRigging.weight : 1f;

			}
		}
		#region Move Function
		void Move()
		{
			if (!isAimOrShootState)
			{
				CaculateInputDirection();
				RotateTransform();
			}
			else
			{
				CaculateInputDirection();
				RotateWithCamera();
			}

		}

		void CaculateInputDirection()
		{
			Vector3 camForwardProjection = new Vector3(m_Camera.transform.forward.x, 0, m_Camera.transform.forward.z).normalized;
			playerMovement = camForwardProjection * playerMoveInput.y + m_Camera.transform.right * playerMoveInput.x;
			playerMovement = transform.InverseTransformDirection(playerMovement);

		}
		void RotateTransform()
		{
			float rad = Mathf.Atan2(playerMovement.x, playerMovement.z);
			transform.Rotate(0, rad * 400 * Time.deltaTime, 0f);
		}
		private void RotateWithCamera()
		{
			// 1. 获取相机的水平方向（忽略Y轴高度，只保留XZ平面的方向）
			Vector3 cameraHorizontalForward = m_Camera.transform.forward;
			cameraHorizontalForward.y = 0; // 消除垂直方向的影响，确保角色在水平面上旋转
			cameraHorizontalForward.Normalize(); // 归一化向量，避免长度异常

			// 如果相机水平方向有效（非零向量）
			if (cameraHorizontalForward.sqrMagnitude > 0.01f)
			{
				// 2. 计算目标旋转：让角色面向相机的水平前方
				Quaternion targetRotation = Quaternion.LookRotation(cameraHorizontalForward);

				// 3. 平滑过渡到目标旋转（避免瞬间跳转）
				transform.rotation = Quaternion.Lerp(
					transform.rotation,
					targetRotation,
					100f * Time.deltaTime
				);
			}
		}
		#endregion
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
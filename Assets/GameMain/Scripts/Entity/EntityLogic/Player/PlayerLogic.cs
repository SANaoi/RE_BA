using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityGameFramework.Runtime;

namespace KSG
{
	public class PlayerLogic : EnitityTargetable
	{
		[Header("玩家输入")]
		public bool isRunning = false;
		public Vector2 playerMoveInput = Vector2.zero;

		public bool isAim = false;
		public bool isShoot = false;
		public bool isDashing = false;

		[Header("默认挂载")]
		public Rigidbody rb;
		protected Animator animator;
		protected Transform CameraParent;
		protected CharacterController characterController;
		public MultiParentConstraint WeaponRigging;
		public MultiAimConstraint AimRigging;

		[Header("运行数据")]
		public bool isAimOrShootState = false;
		public float targetWeaponRiggingWeight;
		public float targetAimRiggingWeight;
		public float lastDashTime = -100f;

		public EneityDataPlayer playerData;
		public UnityEngine.Camera m_Camera;
		public PlayerAnimationName playerAnimationName;
		public Vector3 playerMovement = Vector3.zero;
		protected PlayerInputAction inputActions;
		protected PlayerInputAction.PlayerActions PlayerActions;
		public EneityDataCamera cameraData;
		public CameraLogic cameraEntityLogic;

		protected CrosshairForm crosshairForm;
		protected IFsm<PlayerLogic> MoveFsmManager;
		protected IFsm<PlayerLogic> ShootFsmManager;
		protected List<FsmState<PlayerLogic>> MoveStateList;
		protected List<FsmState<PlayerLogic>> ShootStateList;

		protected override float MaxHP
		{
			get
			{
				if (playerData != null)
					return playerData.MaxHP;
				else
					return 0;
			}
		}

		protected override void OnInit(object userData)
		{
			base.OnInit(userData);
			// GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowVirtualCameraSuccess);
			GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OpenCrosshairFormSuccess);

			m_Camera = Camera.main;
			playerData = userData as EneityDataPlayer;

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
			OpenCrosshairForm();
			inputActions.Enable();
			AddInputActionsCallbacks(); // 添加输入回调
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);

			Move();
			SetRigWeight();
			SetCrosshairSize();
		}

		protected override void OnHide(bool isShutdown, object userData)
		{
			base.OnHide(isShutdown, userData);

			// GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowVirtualCameraSuccess);
			GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OpenCrosshairFormSuccess);

			inputActions.Disable();
			RemoveInputActionsCallbacks(); // 移除输入回调
		}
		private void OnDestroy()
		{
			// 取出状态机所有状态
			FsmState<PlayerLogic>[] MoveStates = MoveFsmManager.GetAllStates();
			FsmState<PlayerLogic>[] ShootStates = ShootFsmManager.GetAllStates();


			GameEntry.Fsm.DestroyFsm(MoveFsmManager);
			GameEntry.Fsm.DestroyFsm(ShootFsmManager);

			//把状态实例归还引用池
			foreach (var item in MoveStates)
			{
				ReferencePool.Release((IReference)item);
			}
			foreach (var item in ShootStates)
			{
				ReferencePool.Release((IReference)item);
			}
		}
		private void ShowVirtualCamera()
		{
			CameraParent = GameObject.Find("CameraRoot").transform;
			cameraData = new EneityDataCamera(GameEntry.Entity.GenerateSerialId(), (int)EnumCamera.PlayerCamera);
			GameEntry.Event.Fire
			(
				this, ShowEntityInLevelEventArgs.Create
				(
					typeof(CameraLogic), 
					"Camera", 
					"Camera", 
					Constant.AssetPriority.CameraAsset, 
					OnShowVirtualCameraSuccess, 
					cameraData
				)
			);
			// GameEntry.Entity.ShowCamera(cameraData);
		}

		private void OpenCrosshairForm()
		{
			GameEntry.UI.OpenUIForm(EnumUIForm.CrosshairForm);
		}
		/// <summary>
		/// 显示虚拟相机成功
		/// </summary>
		private void OnShowVirtualCameraSuccess(Entity entity)
		{
			// GameEntry.Entity.AttachEntity(GameEntry.Entity.GetEntity(cameraData.Id), this.Entity, CameraParent, cameraData);
			cameraEntityLogic = entity.Logic as CameraLogic;
			cameraEntityLogic.SetTarget(CameraParent);
		}

		private void OpenCrosshairFormSuccess(object sender, GameEventArgs e)
		{
			OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
			if (ne.UIForm.Logic.GetType() != typeof(CrosshairForm))
			{
				Log.Warning("UIFormLogic is not CrosshairForm.");
				return;
			}

			crosshairForm = (CrosshairForm)ne.UIForm.Logic;
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

		public void SetCrosshairSize()
		{
			float size = PlayerConstantData.CrosshairData.RESTINGSIZE;
			if (isShoot)
			{
				size += PlayerConstantData.CrosshairData.SHOOTINGSPREAD;
			}

			if (playerMoveInput == Vector2.zero)
			{
				// 静止射击
				size += 0f;
			}
			else if (isRunning)
			{
				size += PlayerConstantData.CrosshairData.RUNNINGSPREAD;
			}
			else if (!isRunning)
			{
				size += PlayerConstantData.CrosshairData.WALKINGSPREAD;
			}

			if (isAim)
			{
				size += PlayerConstantData.CrosshairData.AIMINGSPREAD;
			}

			if (crosshairForm != null)
			{
				crosshairForm.targetSize = size;
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
				// transform.rotation = Quaternion.Lerp(
				// 	transform.rotation,
				// 	targetRotation,
				// 	100f * Time.deltaTime
				// );
				transform.rotation = targetRotation;
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
			MoveStateList.Add(PlayerDashState.Create());
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

			PlayerActions.Dash.performed += OnPlayerDashPerformed;
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

			PlayerActions.Dash.performed -= OnPlayerDashPerformed;

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
		void OnPlayerDashPerformed(InputAction.CallbackContext context)
		{
			if (Time.time > lastDashTime + PlayerConstantData.DashData.DASHCOOLDOWN)
			{
				isDashing = true;
			}
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
		public Coroutine StartDashCoroutine(IEnumerator routine)
		{
			return StartCoroutine(routine);
		}
		#endregion
	}
}
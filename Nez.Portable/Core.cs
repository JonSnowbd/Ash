using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez.Systems;
using Nez.Textures;
using Nez.Timers;
using Nez.Tweens;
using System;
using System.Collections;

namespace Nez
{
    public class Core : Game
	{
		/// <summary>
		/// core emitter. emits only Core level events.
		/// </summary>
		public static Emitter<CoreEvents> Emitter;

		/// <summary>
		/// enables/disables if we should quit the app when escape is pressed
		/// </summary>
		public static bool ExitOnEscapeKeypress = true;

		/// <summary>
		/// This is updated per frame, total amount of batcher draw flushes.
		/// </summary>
		public static int DrawCalls = 0;

		/// <summary>
		/// enables/disables pausing when focus is lost. No update or render methods will be called if true when not in focus.
		/// </summary>
		public static bool PauseOnFocusLost = true;

		/// <summary>
		/// global access to the graphicsDevice
		/// </summary>
		public new static GraphicsDevice GraphicsDevice;

		/// <summary>
		/// global content manager for loading any assets that should stick around between scenes
		/// </summary>
		public new static NezContentManager Content;

		/// <summary>
		/// default SamplerState used by Materials. Note that this must be set at launch! Changing it after that time will result in only
		/// Materials created after it was set having the new SamplerState
		/// </summary>
		public static SamplerState DefaultSamplerState = new SamplerState
		{
			Filter = TextureFilter.Point
		};

		/// <summary>
		/// default wrapped SamplerState. Determined by the Filter of the defaultSamplerState.
		/// </summary>
		/// <value>The default state of the wraped sampler.</value>
		public static SamplerState DefaultWrappedSamplerState =>
			DefaultSamplerState.Filter == TextureFilter.Point
				? SamplerState.PointWrap
				: SamplerState.LinearWrap;

		/// <summary>
		/// default GameServiceContainer access
		/// </summary>
		/// <value>The services.</value>
		public new static GameServiceContainer Services => ((Game) _instance).Services;

		/// <summary>
		/// A lightweight list of <c>System.Action</c> delegates that are ran
		/// before Scene updates, and after Input updates.
		/// </summary>
		public static FastList<Action> UpdateHooks = new FastList<Action>();

		/// <summary>
		/// provides access to the single Core/Game instance
		/// </summary>
		public static Core Instance => _instance;

		/// <summary>
		/// facilitates easy access to the global Content instance for internal classes
		/// </summary>
		public static Core _instance;

		public string Title;
		IScene _scene;
		IScene _nextScene;

		/// <summary>
		/// used to coalesce GraphicsDeviceReset events
		/// </summary>
		ITimer _graphicsDeviceChangeTimer;

		// globally accessible systems
		FastList<GlobalManager> _globalManagers = new FastList<GlobalManager>();
		CoroutineManager _coroutineManager = new CoroutineManager();
		TimerManager _timerManager = new TimerManager();


		/// <summary>
		/// The currently active Scene. Note that if set, the Scene will not actually change until the end of the Update
		/// </summary>
		public static IScene Scene
		{
			get => _instance._scene;
			set
			{
				Insist.IsNotNull(value, "Scene cannot be null!");

				// handle our initial Scene. If we have no Scene and one is assigned directly wire it up
				if (_instance._scene == null)
				{
					_instance._scene = value;
					_instance._scene.Begin();
					_instance.OnSceneChanged();
				}
				else
				{
					_instance._nextScene = value;
				}
			}
		}


		public Core(int width = 1280, int height = 720, bool isFullScreen = false, string windowTitle = "Nez", string contentDirectory = "Content")
		{
			Title = windowTitle;

			_instance = this;
			Emitter = new Emitter<CoreEvents>(new CoreEventsComparer());

			var graphicsManager = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = width,
				PreferredBackBufferHeight = height,
				IsFullScreen = isFullScreen,
				SynchronizeWithVerticalRetrace = true
			};
			graphicsManager.DeviceReset += OnGraphicsDeviceReset;
			graphicsManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;

			Screen.Initialize(graphicsManager);
			Window.ClientSizeChanged += OnGraphicsDeviceReset;
			Window.OrientationChanged += OnOrientationChanged;

			base.Content.RootDirectory = contentDirectory;
			Content = new NezGlobalContentManager(Services, base.Content.RootDirectory);
			IsMouseVisible = true;
			IsFixedTimeStep = false;

			// setup systems
			RegisterGlobalManager(_coroutineManager);
			RegisterGlobalManager(new TweenManager());
			RegisterGlobalManager(_timerManager);
			RegisterGlobalManager(new RenderTarget());
		}

		void OnOrientationChanged(object sender, EventArgs e)
		{
			Emitter.Emit(CoreEvents.OrientationChanged);
		}

		/// <summary>
		/// this gets called whenever the screen size changes
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected void OnGraphicsDeviceReset(object sender, EventArgs e)
		{
			// we coalese these to avoid spamming events
			if (_graphicsDeviceChangeTimer != null)
			{
				_graphicsDeviceChangeTimer.Reset();
			}
			else
			{
				_graphicsDeviceChangeTimer = Schedule(0.05f, false, this, t =>
				{
					(t.Context as Core)._graphicsDeviceChangeTimer = null;
					Emitter.Emit(CoreEvents.GraphicsDeviceReset);
				});
			}
		}

		#region Passthroughs to Game

		public new static void Exit()
		{
			((Game) _instance).Exit();
		}

		#endregion

		#region Game overides

		protected override void Initialize()
		{
			base.Initialize();

			// prep the default Graphics system
			GraphicsDevice = base.GraphicsDevice;
			var font = Content.LoadBitmapFont("./DefaultContent/Dev.fnt");
			var fonts = Content.LoadBitmapFont("./DefaultContent/DevSmall.fnt");
			var fontn = Content.LoadBitmapFont("./DefaultContent/DevNarrow.fnt");
			Graphics.Instance = new Graphics(font, fonts, fontn);
		}

		protected override void Update(GameTime gameTime)
		{
			if (PauseOnFocusLost && !IsActive)
			{
				SuppressDraw();
				return;
			}

			// update all our systems and global managers
			Time.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
			Input.Update();

			for (int i = 0; i < UpdateHooks.Length; i++)
			{
				UpdateHooks[i]();
			}

			if (ExitOnEscapeKeypress &&
			    (Input.IsKeyDown(Keys.Escape) || Input.GamePads[0].IsButtonReleased(Buttons.Back)))
			{
				base.Exit();
				return;
			}

			if (_scene != null)
			{
				for (var i = _globalManagers.Length - 1; i >= 0; i--)
				{
					if (_globalManagers.Buffer[i].Enabled)
						_globalManagers.Buffer[i].Update();
				}

				_scene.Update();

				if (_nextScene != null)
				{
					_scene.End();

					_scene = _nextScene;
					_nextScene = null;
					OnSceneChanged();

					_scene.Begin();
				}
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			DrawCalls = 0;

			if (PauseOnFocusLost && !IsActive)
				return;

			if (_scene != null)
			{
				_scene.Render();
			}
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			base.OnExiting(sender, args);
			Emitter.Emit(CoreEvents.Exiting);
		}

		#endregion

		/// <summary>
		/// Called after a Scene ends, before the next Scene begins
		/// </summary>
		void OnSceneChanged()
		{
			Emitter.Emit(CoreEvents.SceneChanged);
			Time.SceneChanged();
			GC.Collect();
		}


		#region Global Managers

		/// <summary>
		/// adds a global manager object that will have its update method called each frame before Scene.update is called
		/// </summary>
		/// <returns>The global manager.</returns>
		/// <param name="manager">Manager.</param>
		public static void RegisterGlobalManager(GlobalManager manager)
		{
			_instance._globalManagers.Add(manager);
			manager.Enabled = true;
		}

		/// <summary>
		/// removes the global manager object
		/// </summary>
		/// <returns>The global manager.</returns>
		/// <param name="manager">Manager.</param>
		public static void UnregisterGlobalManager(GlobalManager manager)
		{
			_instance._globalManagers.Remove(manager);
			manager.Enabled = false;
		}

		/// <summary>
		/// gets the global manager of type T
		/// </summary>
		/// <returns>The global manager.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T GetGlobalManager<T>() where T : GlobalManager
		{
			for (var i = 0; i < _instance._globalManagers.Length; i++)
			{
				if (_instance._globalManagers.Buffer[i] is T)
					return _instance._globalManagers.Buffer[i] as T;
			}

			return null;
		}

		#endregion


		#region Systems access

		/// <summary>
		/// starts a coroutine. Coroutines can yeild ints/floats to delay for seconds or yeild to other calls to startCoroutine.
		/// Yielding null will make the coroutine get ticked the next frame.
		/// </summary>
		/// <returns>The coroutine.</returns>
		/// <param name="enumerator">Enumerator.</param>
		public static ICoroutine StartCoroutine(IEnumerator enumerator)
		{
			return _instance._coroutineManager.StartCoroutine(enumerator);
		}

		/// <summary>
		/// schedules a one-time or repeating timer that will call the passed in Action
		/// </summary>
		/// <param name="timeInSeconds">Time in seconds.</param>
		/// <param name="repeats">If set to <c>true</c> repeats.</param>
		/// <param name="context">Context.</param>
		/// <param name="onTime">On time.</param>
		public static ITimer Schedule(float timeInSeconds, bool repeats, object context, Action<ITimer> onTime)
		{
			return _instance._timerManager.Schedule(timeInSeconds, repeats, context, onTime);
		}

		/// <summary>
		/// schedules a one-time timer that will call the passed in Action after timeInSeconds
		/// </summary>
		/// <param name="timeInSeconds">Time in seconds.</param>
		/// <param name="context">Context.</param>
		/// <param name="onTime">On time.</param>
		public static ITimer Schedule(float timeInSeconds, object context, Action<ITimer> onTime)
		{
			return _instance._timerManager.Schedule(timeInSeconds, false, context, onTime);
		}

		/// <summary>
		/// schedules a one-time or repeating timer that will call the passed in Action
		/// </summary>
		/// <param name="timeInSeconds">Time in seconds.</param>
		/// <param name="repeats">If set to <c>true</c> repeats.</param>
		/// <param name="onTime">On time.</param>
		public static ITimer Schedule(float timeInSeconds, bool repeats, Action<ITimer> onTime)
		{
			return _instance._timerManager.Schedule(timeInSeconds, repeats, null, onTime);
		}

		/// <summary>
		/// schedules a one-time timer that will call the passed in Action after timeInSeconds
		/// </summary>
		/// <param name="timeInSeconds">Time in seconds.</param>
		/// <param name="onTime">On time.</param>
		public static ITimer Schedule(float timeInSeconds, Action<ITimer> onTime)
		{
			return _instance._timerManager.Schedule(timeInSeconds, false, null, onTime);
		}

		#endregion
	}
}
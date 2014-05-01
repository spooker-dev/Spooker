//-----------------------------------------------------------------------------
// StateUI.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

using Gwen.Control;
using Gwen.Input;
using Gwen.Skin;
using Gwen.Renderer;
using SFML.Window;
using Spooker.Core;
using Spooker.Graphics;

namespace Spooker.GameStates
{
	////////////////////////////////////////////////////////////
	/// <summary>
	/// Abstract class used for handling game input, drawing
	/// and updating for one scene with additional GUI component.
	/// </summary>
	////////////////////////////////////////////////////////////
	public abstract class StateGUI : State
	{
		#region Variables
		private readonly Canvas _gamegui;
		private readonly GuiInput _ginput;
		#endregion

		#region Constructors and Destructors
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Creates new instance of game state with GUI component.
		/// </summary>
		////////////////////////////////////////////////////////////
		protected StateGUI(GameWindow game, string guiImagePath, string fontName, int fontSize): base(game)
		{
			// create GWEN renderer
			var renderer = new GuiRenderer(GraphicsDevice);

			// Create GWEN skin
			var skin = new TexturedBase(renderer, guiImagePath);

			// set default font
			skin.SetDefaultFont(fontName, fontSize);

			// Create a Canvas (it's root, on which all other GWEN controls are created)
			_gamegui = new Canvas(skin);
			_gamegui.SetSize(
				(int)GraphicsDevice.Size.X,
				(int)GraphicsDevice.Size.Y);
			_gamegui.ShouldDrawBackground = false;
			_gamegui.KeyboardInputEnabled = true;

			// Create GWEN input processor
			_ginput = new GuiInput(GraphicsDevice, _gamegui);

			// Load our GUI
			LoadGUI (_gamegui);
		}
		#endregion

		#region Input bindings
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user tries to enter text.
		/// </summary>
		////////////////////////////////////////////////////////////
		public override void TextEntered(TextEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user tries to move with mouse wheel.
		/// </summary>
		////////////////////////////////////////////////////////////
		public override void MouseWheelMoved(MouseWheelEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user tries to move with mouse.
		/// </summary>
		////////////////////////////////////////////////////////////
		public override void MouseMoved(MouseMoveEventArgs e)
		{
			_ginput.ProcessMessage(e);
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user presses mouse button.
		/// </summary>
		////////////////////////////////////////////////////////////
		public override void MouseButtonPressed(MouseButtonEventArgs e)
		{
			_ginput.ProcessMessage(new SfmlMouseButtonEventArgs(e, true));
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user releases mouse button.
		/// </summary>
		////////////////////////////////////////////////////////////
		public override void MouseButtonReleased(MouseButtonEventArgs e)
		{
			_ginput.ProcessMessage(new SfmlMouseButtonEventArgs(e, false));
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user presses keyboard key.
		/// </summary>
		////////////////////////////////////////////////////////////
		public override void KeyPressed(KeyEventArgs e)
		{
			_ginput.ProcessMessage(new SfmlKeyEventArgs(e, true));
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when user releases keyboard key.
		/// </summary>
		////////////////////////////////////////////////////////////
		public override void KeyReleased(KeyEventArgs e)
		{
			_ginput.ProcessMessage(new SfmlKeyEventArgs(e, false));
		}

		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when a state is added to game (pushed to stack).
		/// </summary>
		////////////////////////////////////////////////////////////
		public override void Enter()
		{
			base.Enter ();
			LoadGUI (_gamegui);
		}
		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called when a state is removed from game (popped from stack).
		/// </summary>
		////////////////////////////////////////////////////////////
		public override void Leave()
		{
			base.Leave ();

			// Clear all loaded GUI components from GUI manager.
			// Yes, it is okay to clear all, we will load another
			// when we will add another state to game.
			Gwen.GuiManager.Clear ();
		}


		////////////////////////////////////////////////////////////
		/// <summary>
		/// Called after loading of GUI instance, use this for 
		/// initializing GUI objects
		/// </summary>
		////////////////////////////////////////////////////////////
		public virtual void LoadGUI(Canvas gameGUI) { }

		#endregion

		#region Functions
		internal void DrawGUI()
		{
			_gamegui.RenderCanvas ();
		}
		#endregion
	}
}
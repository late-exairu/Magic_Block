using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBlock
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		public static int windowWidth = 1800; // def 600
		public static int windowHeight = 800; // def 760

		GameArea gameArea;
		Matrix gameMatrix;
		Brick myBrick;
		bool gameStart = false;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			graphics.IsFullScreen = false;
			graphics.PreferredBackBufferHeight = windowHeight;
			graphics.PreferredBackBufferWidth = windowWidth;
		}

		protected override void Initialize()
		{
			// Initialize Start


			// Initialize End
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);
			// LoadContent Start
			
			gameArea = new GameArea("gameAreaBG", this.Content);
			gameMatrix = new Matrix(14, 18, this.Content);
			myBrick = new Brick(this.Content);

			// LoadContent End
		}

		protected override void UnloadContent()
		{
			// UnloadContent Start
			// UnloadContent End
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			// Update Start

			if (gameStart == false)
			{
				gameMatrix.NewBrick();
				gameStart = true;
			}
			gameMatrix.Controls();
			gameMatrix.Update(this.Content);

			// Update End
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin();
			// Draw Start

			gameArea.Draw(spriteBatch);
			gameMatrix.Draw(spriteBatch);

			// Draw End
			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBlock
{
	class GameArea
	{
		private Texture2D areaTxtr;
		private Rectangle areaRect;
		public static int areaWidth = 560;
		public static int areaHeight = 720;

		public GameArea(string txtrName, ContentManager content)
		{
			this.areaTxtr = (content.Load<Texture2D>(txtrName));
			this.areaRect = new Rectangle(Game1.windowWidth / 2 - areaWidth / 2, Game1.windowHeight / 2 - areaHeight / 2, areaWidth, areaHeight);
		}

		public Rectangle AreaRect
		{
			get { return areaRect; }
			set { areaRect = value; }
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(areaTxtr, areaRect, Color.White);
		}
	}
}

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
	class Block
	{
		public static int width = 40;
		public static int height = 40;
		private Texture2D txtr;
		private Rectangle rect;
		string txtrName;
		Dictionary<int, string> blockColors = new Dictionary<int, string>
		{
			[1] = "b_gray",
			[2] = "b_black",
			[3] = "b_darkBlue",
			[4] = "b_blue",
			[5] = "b_green",
			[6] = "b_yellow",
			[7] = "b_orange",
			[8] = "b_red",
			[9] = "b_violet"
		};

		// Block constructor with random color assigning
		public Block(int rectX, int rectY, int rectWidth, int rectHeight, ContentManager content)
		{
			Random rnd = new Random();
			int brickRnd = rnd.Next(1, 10);
			txtr = content.Load<Texture2D>(blockColors[brickRnd]);
			rect = new Rectangle(rectX, rectY, rectWidth, rectHeight);
		}

		// Constructor with direct string color assigning
		public Block(string txtrName, int rectX, int rectY, int rectWidth, int rectHeight, ContentManager content)
		{
			txtr = content.Load<Texture2D>(txtrName);
			rect = new Rectangle(rectX, rectY, rectWidth, rectHeight);
		}

		// Properties
		public Texture2D Txtr
		{
			get { return txtr; }
			set { txtr = value; }
		}

		public Rectangle Rect
		{
			get { return rect; }
			set { rect = value; }
		}

		public string TxtrName
		{
			get { return txtrName; }
			set { txtrName = value; }
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(txtr, rect, Color.White);
		}

	}
}

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
	class Brick
	{
		private int block1;
		private int block2;
		private int block3;

		public Brick(ContentManager content)
		{
			int[] randArray = new int[3];

			Random randNum = new Random();
			for (int i = 0; i < randArray.Length; i++)
			{
				randArray[i] = randNum.Next(1, 10);
			}

			block1 = randArray[0];
			block2 = randArray[1];
			block3 = randArray[2];
		}
	}
}

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

	class Matrix
	{
		int[,] numsMtrx;
		int[,] printedNumsMtrx;
		int[,] differenceMtrx;
		int[,] indexToDestroyMtrx;
		Block[,] blocksMtrx;
		SpriteFont font;
		Dictionary<int, string> blockColors = new Dictionary<int, string>
		{
			[0] = "b_none",
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
		int numX = 6;
		int numY = 0;

		// Initialize Matrix
		public Matrix(int mtrxWidth, int mtrxHeight, ContentManager content)
		{
			font = content.Load<SpriteFont>("Arial");
			NumsMtrx = new int[mtrxWidth, mtrxHeight];
			blocksMtrx = new Block[mtrxWidth, mtrxHeight];
			printedNumsMtrx = new int[mtrxWidth, mtrxHeight];
			differenceMtrx = new int[mtrxWidth, mtrxHeight];
			indexToDestroyMtrx = new int[mtrxWidth, mtrxHeight];

			for (int i = 0; i < blocksMtrx.GetLength(0); i++)
			{
				for (int ii = 0; ii < blocksMtrx.GetLength(1); ii++)
				{
					blocksMtrx[i, ii] = new Block(blockColors[NumsMtrx[i, ii]], i * Block.width, ii * Block.height, Block.width, Block.height, content);
				}
			}
		}

		// Properties
		public int[,] NumsMtrx
		{
			get { return numsMtrx; }
			set { numsMtrx = value; }
		}

		internal Block[,] BlocksMtrx
		{
			get { return blocksMtrx; }
			set { blocksMtrx = value; }
		}

		//
		// Methods
		//
		public void NewBrick()
		{
			int[] randArray = new int[3];
			Random randNum = new Random();
			for (int i = 0; i < randArray.Length; i++)
			{
				// randArray[i] = randNum.Next(1, 10);
				NumsMtrx[6, i] = randNum.Next(1, 10);
			}
		}

		// Controls
		public void Controls()
		{
			if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A)) // Move Left
			{
				if ((numX != 0) && (NumsMtrx[numX - 1, numY] == 0) && (NumsMtrx[numX - 1, numY + 1] == 0) && (NumsMtrx[numX - 1, numY + 2] == 0))
				{
					NumsMtrx[numX - 1, numY] = NumsMtrx[numX, numY];
					NumsMtrx[numX - 1, numY + 1] = NumsMtrx[numX, numY + 1];
					NumsMtrx[numX - 1, numY + 2] = NumsMtrx[numX, numY + 2];
					NumsMtrx[numX, numY] = 0;
					NumsMtrx[numX, numY + 1] = 0;
					NumsMtrx[numX, numY + 2] = 0;
					numX--;
				}
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D)) // Move Right
			{
				if ((numX != NumsMtrx.GetLength(0) - 1) && (NumsMtrx[numX + 1, numY] == 0) && (NumsMtrx[numX + 1, numY + 1] == 0) && (NumsMtrx[numX + 1, numY + 2] == 0))
				{
					NumsMtrx[numX + 1, numY] = NumsMtrx[numX, numY];
					NumsMtrx[numX + 1, numY + 1] = NumsMtrx[numX, numY + 1];
					NumsMtrx[numX + 1, numY + 2] = NumsMtrx[numX, numY + 2];
					NumsMtrx[numX, numY] = 0;
					NumsMtrx[numX, numY + 1] = 0;
					NumsMtrx[numX, numY + 2] = 0;
					numX++;
				}
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S)) // Move Down
			{
				if ((numY + 2 != NumsMtrx.GetLength(1) - 1) && (NumsMtrx[numX, numY + 3] == 0))
				{
					NumsMtrx[numX, numY + 3] = NumsMtrx[numX, numY + 2];
					NumsMtrx[numX, numY + 2] = NumsMtrx[numX, numY + 1];
					NumsMtrx[numX, numY + 1] = NumsMtrx[numX, numY];
					NumsMtrx[numX, numY] = 0;
					numY++;
				} else if ((numY + 2 == NumsMtrx.GetLength(1) - 1) || (NumsMtrx[numX, numY + 3] >= 0)) // Drop blocks
				{
					numX = 6;
					numY = 0;

					// Call for check matches and destroy
					GenerateDiffMtrx();
					MatchCheck();
					NewBrick();
				}
			}

			if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W)) // Shuffle
			{
				int temp = NumsMtrx[numX, numY + 2];
				NumsMtrx[numX, numY + 2] = NumsMtrx[numX, numY + 1];
				NumsMtrx[numX, numY + 1] = NumsMtrx[numX, numY];
				NumsMtrx[numX, numY] = temp;
			}
		}

		// Genereate matrix that have "1" on every unequal index between numsMtrx and printedNumsMtrx
		public void GenerateDiffMtrx()
		{
			Array.Clear(differenceMtrx, 0, 252);

			for (int i = 0; i < numsMtrx.GetLength(0); i++)
			{
				for (int ii = 0; ii < numsMtrx.GetLength(1); ii++)
				{
					if (numsMtrx[i, ii] != printedNumsMtrx[i, ii])
					{
						differenceMtrx[i, ii] = 1;
					}
				}
			}

			// Copy numsMtrx nums to printedNumsMtrx
			CopyMtrxNums(printedNumsMtrx, numsMtrx);
		}

		// Copying donor matrix nums to recipient matrix nums
		public void CopyMtrxNums(int[,] recipient, int[,] donor)
		{
			for (int i = 0; i < numsMtrx.GetLength(0); i++)
			{
				for (int ii = 0; ii < numsMtrx.GetLength(1); ii++)
				{
					recipient[i, ii] = donor[i, ii];
				}
			}
		}

		// Find matched NumsMtrx nums
		public void MatchCheck()
		{
			// Copy nums from differenceMtrx to indexToDestroyMtrx
			// CopyMtrxNums(indexToDestroyMtrx, differenceMtrx);
			Array.Clear(indexToDestroyMtrx, 0, 252);
			for (int i = 0; i < NumsMtrx.GetLength(0); i++)
			{
				for (int ii = 0; ii < NumsMtrx.GetLength(1); ii++)
				{
					VerticalCheck();
					//int matchedXNumsCount = 0;
					//int matchedYNumsCount = 0;

					//if (differenceMtrx[i, ii] == 1)
					//{
					//	// horizontal check

					//	// check to left from diffIndex 
					//	for (int check = 1; check < 3; check++)
					//	{
					//		int indexX = i - check;

					//		if ((indexX < 0) || (NumsMtrx[i, ii] != NumsMtrx[indexX, ii]))
					//			break;

					//		indexToDestroyMtrx[indexX, ii] = 1;
					//		matchedXNumsCount++;
					//	}
					//	// check to right from diffIndex 
					//	for (int check = 1; check < 3; check++)
					//	{
					//		int indexX = i + check;

					//		if ((indexX >= numsMtrx.GetLength(0)) || (NumsMtrx[i, ii] != NumsMtrx[indexX, ii]))
					//			break;

					//		indexToDestroyMtrx[indexX, ii] = 1;
					//		matchedXNumsCount++;
					//	}

					//	// mark with 1 current indexToDestroyMtrx index, if two or more indexes in row matched
					//	if (matchedXNumsCount >= 2)
					//	{
					//		indexToDestroyMtrx[i, ii] = 1;
					//	} else
					//	{
					//		indexToDestroyMtrx[i, ii] = 0;
					//	}

					//	// vertical check

					//	// check to bottom from diffIndex 
					//	for (int check = 1; check < 5; check++)
					//	{
					//		int indexY = ii + check;

					//		if (indexY >= numsMtrx.GetLength(1))
					//			break;

					//		indexToDestroyMtrx[i, indexY] = 1;
					//		matchedYNumsCount++;
					//	}

					//	// mark with 1 current indexToDestroyMtrx index, if two or more indexes in row matched
					//	if (matchedYNumsCount >= 2)
					//	{
					//		indexToDestroyMtrx[i, ii] = 1;
					//	}
					//	else
					//	{
					//		indexToDestroyMtrx[i, ii] = 0;
					//	}

					//	// destroy by indexToDestroyMtrx
					//	//if (matchedXNumsCount >= 2 || matchedYNumsCount >= 2)
					//	//{
					//	//	for (int x = 0; x < NumsMtrx.GetLength(0); x++)
					//	//	{
					//	//		for (int y = 0; y < NumsMtrx.GetLength(1); y++)
					//	//		{
					//	//			if (indexToDestroyMtrx[x, y] == 1)
					//	//			{
					//	//				for (int v = 0; v < NumsMtrx.GetLength(1); v++)
					//	//				{
					//	//					NumsMtrx[x, y - v] = NumsMtrx[x, y - v - 1];
					//	//					if (y - v - 1 <= 0)
					//	//						break;
					//	//				}
					//	//			}
					//	//		}
					//	//	}
					//	//}
					//}
				}
			}
		}

		public void VerticalCheck()
		{
			for (int i = 0; i < blocksMtrx.GetLength(0); i++)
			{
				for (int ii = 0; ii < blocksMtrx.GetLength(1); ii++)
				{
					// find 1 in differenceMtrx
					if (differenceMtrx[i, ii] == 1)
					{
						int rowWidth = 5;
						int tryWidth = 5;

						// iteration per tryWidth (per 5, 4, 3)
						for (int rowTries = rowWidth % tryWidth; rowTries >= 0; rowTries--)
						{
							int startPos = 2;
							bool lastMatched = false;

							// comparsion of each num in row with next
							for (int iter = tryWidth; iter > 0; iter--)
							{
								if ((i - startPos >= 0) && (NumsMtrx[i - startPos, ii] == NumsMtrx[i - startPos + 1, ii]))
								{
									lastMatched = true;
								} else
								{
									lastMatched = false;
									break;
								}

								startPos--;
							}

							if (lastMatched == true)
							{
								for (int iter = tryWidth; iter > 0; iter--)
								{
									indexToDestroyMtrx[i - startPos, ii] = 1;
								}
							}
						}

						tryWidth--;
					}
				}
			}
		}

		public void Update(ContentManager content)
		{
			for (int i = 0; i < blocksMtrx.GetLength(0); i++)
			{
				for (int ii = 0; ii < blocksMtrx.GetLength(1); ii++)
				{
					blocksMtrx[i, ii].Txtr = content.Load<Texture2D>(blockColors[NumsMtrx[i, ii]]);
				}
			}
		}

		// Draw Matrix
		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < blocksMtrx.GetLength(0); i++)
			{
				for (int ii = 0; ii < blocksMtrx.GetLength(1); ii++)
				{
					spriteBatch.Draw(blocksMtrx[i, ii].Txtr,
									 new Vector2(blocksMtrx[i, ii].Rect.X + (Game1.windowWidth / 2) - (GameArea.areaWidth / 2),
												 blocksMtrx[i, ii].Rect.Y + (Game1.windowHeight / 2) - (GameArea.areaHeight / 2)),
									 Color.White);
					//spriteBatch.DrawString(font, Convert.ToString(NumsMtrx[i, ii]),
					//							 new Vector2(blocksMtrx[i, ii].Rect.X + (Game1.windowWidth / 2) - (GameArea.areaWidth / 2),
					//							 blocksMtrx[i, ii].Rect.Y + (Game1.windowHeight / 2) - (GameArea.areaHeight / 2)),
					//							 Color.White);
					//spriteBatch.DrawString(font, Convert.ToString(numX),
					//							 new Vector2(blocksMtrx[i, ii].Rect.X + (Game1.windowWidth / 2) - (GameArea.areaWidth / 2),
					//							 blocksMtrx[i, ii].Rect.Y + (Game1.windowHeight / 2) - (GameArea.areaHeight / 2) + 20),
					//							 Color.White);
				}
			}

			for (int i = 0; i < blocksMtrx.GetLength(0); i++)
			{
				for (int ii = 0; ii < blocksMtrx.GetLength(1); ii++)
				{
					spriteBatch.Draw(blocksMtrx[i, ii].Txtr,
									 new Vector2(blocksMtrx[i, ii].Rect.X + 20,
												 blocksMtrx[i, ii].Rect.Y + (Game1.windowHeight / 2) - (GameArea.areaHeight / 2)),
									 Color.White);
					spriteBatch.DrawString(font, Convert.ToString(printedNumsMtrx[i, ii]),
												 new Vector2(blocksMtrx[i, ii].Rect.X + 20,
															 blocksMtrx[i, ii].Rect.Y + (Game1.windowHeight / 2) - (GameArea.areaHeight / 2)),
												 Color.White);
					spriteBatch.DrawString(font, Convert.ToString(numsMtrx[i, ii]),
												 new Vector2(blocksMtrx[i, ii].Rect.X + 40,
															 blocksMtrx[i, ii].Rect.Y + (Game1.windowHeight / 2) - (GameArea.areaHeight / 2)),
												 Color.White);
					spriteBatch.DrawString(font, Convert.ToString(differenceMtrx[i, ii]),
												 new Vector2(blocksMtrx[i, ii].Rect.X + 20,
															 blocksMtrx[i, ii].Rect.Y + (Game1.windowHeight / 2) - (GameArea.areaHeight / 2) + 20),
												 Color.White);
					spriteBatch.DrawString(font, Convert.ToString(indexToDestroyMtrx[i, ii]),
												 new Vector2(blocksMtrx[i, ii].Rect.X + 40,
															 blocksMtrx[i, ii].Rect.Y + (Game1.windowHeight / 2) - (GameArea.areaHeight / 2) + 20),
												 Color.White);
				}
			}

			//spriteBatch.DrawString(font, String.Format("{0}", GameTime.TotalGameTime.TotalMilliseconds),
			//							 new Vector2(10, 10), Color.White);
		}
	}
}

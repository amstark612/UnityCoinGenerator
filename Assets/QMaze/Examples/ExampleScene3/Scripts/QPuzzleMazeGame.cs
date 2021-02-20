using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace qtools.qmaze.example3
{
	public class QPuzzleMazeGame : MonoBehaviour 
	{
		private int CHILD_MAZE_SIZE = 7;

		public QMazeEngine baseMazeEngine;
		public QMazeEngine childMazeEngine_1;
		public QMazeEngine childMazeEngine_2;
		public Material baseMazeMaterialPrefab;
		public Material childMazeMaterialPrefab;
		public QPuzzleGamePlayer player;
		public Transform finishTransform;
		public QPuzzleMazeCamera mazeCamera;
		public QMaterialAlpha finishAlpha;
		public QMaterialAlpha playerAlpha;
        public Text levelText; 

		private QMazeEngine prevMazeEngine;
		private QMazeEngine nextMazeEngine;

		private QRectInt prevRect;
		private QRectInt nextRect;

        private int currentLevel;

		void Start () 
		{
			baseMazeEngine.transform.position = new Vector3(0, -3, 0);

            currentLevel = 0;
			generateNextLevel();
		}

		private void generateNextLevel()
		{
			baseMazeEngine.destroyImmediateMazeGeometry();

			List<QVector2IntDir> exitPositionList = baseMazeEngine.getExitPositionList();
			if (exitPositionList.Count > 1)
			{
				exitPositionList.RemoveAt(0);
				baseMazeEngine.setExitPositionList(exitPositionList);
			}

			List<QVector2Int> obstaclePositionList = new List<QVector2Int>();
			if (prevMazeEngine == null)
			{
				prevRect = new QRectInt(QMath.getRandom(1, baseMazeEngine.getMazeWidth()  - CHILD_MAZE_SIZE - 2),
				                        QMath.getRandom(1, baseMazeEngine.getMazeHeight() - CHILD_MAZE_SIZE - 2), CHILD_MAZE_SIZE, CHILD_MAZE_SIZE);
				obstaclePositionList.AddRange(rectToList(prevRect));
				prevMazeEngine = createChildMaze(prevRect, childMazeEngine_1);
				prevMazeEngine.generateMaze();

				player.setPosition(prevMazeEngine.transform.TransformPoint(prevMazeEngine.getFinishPositionList()[0].toVector3()));
			}
			else
			{
				prevMazeEngine.destroyImmediateMazeGeometry();
				prevRect = nextRect;
				prevMazeEngine = nextMazeEngine;
				obstaclePositionList.AddRange(rectToList(prevRect));
			}

			nextRect = new QRectInt(QMath.getRandom(1, baseMazeEngine.getMazeWidth()  - CHILD_MAZE_SIZE - 2),
			                        QMath.getRandom(1, baseMazeEngine.getMazeHeight() - CHILD_MAZE_SIZE - 2), CHILD_MAZE_SIZE, CHILD_MAZE_SIZE);
			while (isRectNear(prevRect, nextRect))
			{				
				nextRect.x = QMath.getRandom(1, baseMazeEngine.getMazeWidth()  - CHILD_MAZE_SIZE - 2);
				nextRect.y = QMath.getRandom(1, baseMazeEngine.getMazeHeight() - CHILD_MAZE_SIZE - 2);
			}

			obstaclePositionList.AddRange(rectToList(nextRect));

			baseMazeEngine.setObstaclePositionList(obstaclePositionList);
			nextMazeEngine = createChildMaze(nextRect, prevMazeEngine == childMazeEngine_1 ? childMazeEngine_2 : childMazeEngine_1);
			nextMazeEngine.generateMaze();
			List<QVector2IntDir> nextMazeEngineFinishPositionList = nextMazeEngine.getFinishPositionList();

			finishTransform.parent = nextMazeEngine.getMazeData()[nextMazeEngineFinishPositionList[0].x][nextMazeEngineFinishPositionList[0].y].geometry.transform;
			finishTransform.localPosition = new Vector3();

			player.setGoal(nextMazeEngine.transform.TransformPoint(nextMazeEngineFinishPositionList[0].toVector3()), goalReachedHandler);

			baseMazeEngine.generateMaze();

            currentLevel++;
            levelText.text = "LEVEL: " + currentLevel;
		}

        private void goalReachedHandler()
        {
			StartCoroutine(finishAnimation());
		}

		private IEnumerator finishAnimation()
		{
			yield return new WaitForSeconds(0.4f);
			mazeCamera.setFollowMode(false);
			yield return new WaitForSeconds(1.0f);
			finishAlpha.setAlpha(0);
			baseMazeEngine.GetComponent<QMazeMover>().hide(0);
			prevMazeEngine.GetComponent<QMazeMover>().hide(0, hideCompleteHandler);
		}

		private void hideCompleteHandler()
		{
			generateNextLevel();
		}

		private bool isRectNear(QRectInt first, QRectInt second, int offset = 1)
		{
			if (first.x < second.x + second.width  + offset && first.x + first.width  + offset > second.x &&
			    first.y < second.y + second.height + offset && first.y + first.height + offset > second.y) 
				return true;
			else
				return false;
		}

		private QMazeEngine createChildMaze(QRectInt rect, QMazeEngine mazeEngine)
		{
			mazeEngine.setMazeWidth(rect.width);
			mazeEngine.setMazeHeight(rect.height);
			mazeEngine.transform.position = new Vector3(0, 0, 0);
			mazeEngine.transform.position = mazeEngine.transform.TransformPoint(new Vector3(rect.x, -3, -rect.y));

			List<QVector2IntDir> finishPositionList = new List<QVector2IntDir>();
			finishPositionList.Add(new QVector2IntDir(rect.width / 2, rect.height / 2, QMazeOutputDirection.NotSpecified));
			mazeEngine.setFinishPositionList(finishPositionList);

			List<QVector2IntDir> exitPositionList = new List<QVector2IntDir>();
			QVector2IntDir mazeExit = getExitForRect(rect);
			exitPositionList.Add(mazeExit);
			mazeEngine.setExitPositionList(exitPositionList);            

			List<QVector2IntDir> baseMazeEngineExitPositionlist = baseMazeEngine.getExitPositionList();
			baseMazeEngineExitPositionlist.Add(new QVector2IntDir(rect.x + mazeExit.x + QMazeOutput.dx[mazeExit.direction],
			                                                      rect.y + mazeExit.y + QMazeOutput.dy[mazeExit.direction],
			                                                      QMazeOutput.opposite[mazeExit.direction]));
			baseMazeEngine.setExitPositionList(baseMazeEngineExitPositionlist);

			return mazeEngine;
		}

		private QVector2IntDir getExitForRect(QRectInt rect)
		{
			QMazeOutputDirection dir;
			int ix = QMath.getRandom(0, rect.width);
			int iy;
			if (ix == 0 || ix == rect.width - 1)
			{
				iy = QMath.getRandom(0, rect.height);
				dir = (ix == 0 ? QMazeOutputDirection.W : QMazeOutputDirection.E);
			}
			else
			{
				if (QMath.getRandom() > 0.5)
				{
					iy = 0;
					dir = QMazeOutputDirection.N;
				}
				else
				{
					iy = rect.height - 1;
					dir = QMazeOutputDirection.S;
				}
			}
			return new QVector2IntDir(ix, iy, dir);
		}

		public void childMazePieceHandler(QMazePieceData pieceData)
		{
			pieceData.geometry.GetComponent<MeshRenderer>().material = childMazeMaterialPrefab;
		}

		public void baseMazeGenerateCompleteHandler(QMazeEngine mazeEngine)
		{
			finishTransform.parent = null;
			prevMazeEngine.gameObject.GetComponent<QMazeMover>().show(0.0f);
			nextMazeEngine.gameObject.GetComponent<QMazeMover>().show(1.0f);
			baseMazeEngine.GetComponent<QMazeMover>().show(2.0f);
			StartCoroutine(startAnimation());
		}

		private IEnumerator startAnimation()
		{
			yield return new WaitForSeconds(2.5f);
			finishAlpha.setAlpha(1);
			playerAlpha.setAlpha(1);
			yield return new WaitForSeconds(1.0f);
			mazeCamera.setFollowMode(true);
		}

		private List<QVector2Int> rectToList(QRectInt rect)
		{
			List<QVector2Int> result = new List<QVector2Int>();
			for (int ix = rect.x; ix < rect.x + rect.width; ix++)
			{
				for (int iy = rect.y; iy < rect.y + rect.height; iy++)
				{
					result.Add(new QVector2Int(ix, iy));
				}
			}
			return result;
		}
	}

	public class QRectInt
	{
		public int x;
		public int y;
		public int width;
		public int height;

		public QRectInt(int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}
	}
}
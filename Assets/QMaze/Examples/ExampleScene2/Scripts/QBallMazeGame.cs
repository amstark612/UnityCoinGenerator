using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace qtools.qmaze.example2
{
	public class QBallMazeGame : MonoBehaviour 
	{	
		public QMazeEngine mazeEnginePrefab;
		public GameObject blockPrefab;
        public Text levelText;

		private Queue<QMazeEngine> parts;
		private int currentPartId;
		private int lastExitY;

		void Start () 
		{
			currentPartId = 0 ;
			parts = new Queue<QMazeEngine>();

			generateNextPart();
			generateNextPart();
		}

		private void generateNextPart()
		{
			QMazeEngine mazeEngine = (QMazeEngine)GameObject.Instantiate(mazeEnginePrefab);
			mazeEngine.getMazePiecePack().getPiece(QMazePieceType.Intersection).use = false;
			mazeEngine.transform.position = new Vector3(currentPartId * mazeEngine.getMazeWidth() * mazeEngine.getMazePieceWidth(), 0, 0);
			parts.Enqueue(mazeEngine);

			if (currentPartId == 0) 
			{
				List<QVector2IntDir> startPositionList = new List<QVector2IntDir>();
				startPositionList.Add(new QVector2IntDir(0, 0, QMazeOutputDirection.NotSpecified));
				mazeEngine.setStartPositionList(startPositionList);

				lastExitY = QMath.getRandom(0, mazeEngine.getMazeHeight() - 1);

				List<QVector2IntDir> exitPositionList = new List<QVector2IntDir>();
				exitPositionList.Add(new QVector2IntDir(mazeEngine.getMazeWidth() - 1, lastExitY, QMazeOutputDirection.E));
				mazeEngine.setExitPositionList(exitPositionList);
			}
			else
			{
				List<QVector2IntDir> exitPositionList = new List<QVector2IntDir>();
				exitPositionList.Add(new QVector2IntDir(0, lastExitY, QMazeOutputDirection.W));

				lastExitY = QMath.getRandom(0, mazeEngine.getMazeHeight() - 1);

				exitPositionList.Add(new QVector2IntDir(mazeEngine.getMazeWidth() - 1, lastExitY, QMazeOutputDirection.E));
				mazeEngine.setExitPositionList(exitPositionList);

			}

			GameObject block = (GameObject)GameObject.Instantiate(blockPrefab);
			block.transform.parent = mazeEngine.gameObject.transform;
			block.transform.position = new Vector3(((currentPartId + 1) * mazeEngine.getMazeWidth() - 0.5f) * mazeEngine.getMazePieceWidth(), 0, - lastExitY * mazeEngine.getMazePieceHeight());
			block.GetComponent<QBlock>().triggerHandlerEvent += blockHandler;
			mazeEngine.generateMazeAsync(this, 0.016f);

            levelText.text = "LEVEL: " + currentPartId;
			currentPartId++;
		}

		void blockHandler()
		{
			generateNextPart();

			if (parts.Count > 3)
			{
				QMazeEngine mazeEngine = parts.Dequeue();
				Destroy(mazeEngine.gameObject);
			}
		}

		public void test(QMazePieceData[][] mazeData)
		{

			QMazeOutput output = new QMazeOutput();
			output.outputDirList.Add(QMazeOutputDirection.N);
			output.outputDirList.Add(QMazeOutputDirection.S);

			mazeData[1][1] = new QMazePieceData(1,1);
			mazeData[1][1].outputs = new List<QMazeOutput>();
			mazeData[1][1].outputs.Add(output);
		}
	}
}
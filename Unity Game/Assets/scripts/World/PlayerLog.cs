using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerLog : MonoBehaviour {

	public static int maxLines = 24;
	public static Queue<string> queue = new Queue<string>();
	public static string stats = "";
	public static bool showLog = true;

	public static void addStat(string message) {
		if (queue.Count >= maxLines)
			queue.Dequeue();
		
		queue.Enqueue(message);
		
		stats = "";
		foreach (string st in queue)
			stats = stats + st + "\n";
	}

	public void OnGUI(){
		if (showLog) {
			int boxWidth = 300;
			int boxHeight = 400;
			int left = 20;
			int top = 20;

			GUI.Label (new Rect (left, top, boxWidth, boxHeight), stats, GUI.skin.textArea);
		}
	}
}

using UnityEngine;
using System.Collections;
using GameSparks.Api.Requests;

public class Ach_Score : Achievement {

	public int scoreLimit = 500000;

	public override bool CheckNew ()
	{
		int score = ScoreManager.Instance.Score;

		if(!earned && score >= scoreLimit)
		{
			Achieve ();
            return true;
		}
        return false;
	}

	public override void Achieve ()
	{
		base.Achieve ();

//		new LogEventRequest_Evt_Ach_Score ().Send ((response) => 
//			{
//				if(response.HasErrors)
//				{
//					DebugConsole.Log(name + " failed!", "warning");
//				}
//				else
//				{
//					DebugConsole.Log(name + "succeded!");
//				}
//			}
//		);

	}

}

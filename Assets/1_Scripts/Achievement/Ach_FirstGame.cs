using UnityEngine;
using System.Collections;
using GameSparks.Api.Requests;

public class Ach_FirstGame : Achievement {


	public override bool CheckNew ()
	{
		if(!earned)
		{
			Achieve ();
            return true;
		}

        return false;
	}

	public override void Achieve ()
	{
		base.Achieve ();


//		new LogEventRequest_Evt_Ach_First_Play ().Send ((response) => 
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

using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	public bool auto = false;
	public bool selected = false;

	public string name = "Anonymous Powerup";
	public string desctription = "Powerup description.";
	public int cost = 10;

//	public Sprite sprite;
//	public Sprite selectedSprite;

    public PowerUpButton button;

	/// <summary>
	/// Called if powerup is selected on game start.
	/// Spawns the powerup ball or does the action.
	/// </summary>
	public void OnGameStart()
	{
		if(auto)
		{
			Action ();
		}
		else
		{
			SpawnPowerUpBall ();
		}
	}

	/// <summary>
	/// Cleans up the powerup, reverts the effects.
	/// </summary>
	public void OnGameStop()
	{
		CleanUp ();
        Deselect (false);
	}

	public void SpawnPowerUpBall()
	{
		SpawnManager.Instance.SpawnPowerUpBall (this);
	}

	/// <summary>
	/// Toggle this powerup.
	/// Returns new state.
	/// </summary>
	public bool Toggle()
	{
		if (!selected)
		{
			Select ();
			return true;
		}
		else
		{
			Deselect ();
			return false;
		}
	}


	/// <summary>
	/// Select this powerup.
	/// Add OnGameStart and OnGameStop to the respected GameManager delegates.
	/// </summary>
	public void Select()
	{
        // If user dont have enough coins
        if(!CheckCoins())
        {
            UIManager.Instance.outofcoinsPanel.Open();
//            UIManager.Instance.shopPanel.Open();
            return;
        }

		selected = true;
		GameManager.Instance.OnGameStart += OnGameStart;
		GameManager.Instance.OnGameStop += OnGameStop;

//        button.SetAppeareance(true);
        button.Select();

		GoogleAnalyticsManager.Instance.Event_EnablePowerUp (this);

        MetaManager.Instance.UpdateMeta(MetaManager.MetaType.coins, cost, new object[]{false, true});

        UIManager.Instance.preGamePanel.ShowPowerUpDescription(this);
	}

    public void Deselect(bool updateCurrency = true)
	{
        if (!selected)
            return;
        
		selected = false;
		GameManager.Instance.OnGameStart -= OnGameStart;
		GameManager.Instance.OnGameStop -= OnGameStop;

        button.Select(false);
//        button.SetAppeareance(false);

		GoogleAnalyticsManager.Instance.Event_DisablePowerUp (this);

        if(updateCurrency)
        {
            MetaManager.Instance.UpdateMeta(MetaManager.MetaType.coins, cost, new object[]{true, true});
        }

        UIManager.Instance.preGamePanel.ShowPowerUpDescription(this);
	}

	public virtual void Action()
	{
		
	}

	public virtual void CleanUp()
	{
        
	}

    bool CheckCoins()
    {
        int currentCoins = (int)MetaManager.Instance.Get(MetaManager.MetaType.coins);

        return currentCoins >= cost;
    }

}

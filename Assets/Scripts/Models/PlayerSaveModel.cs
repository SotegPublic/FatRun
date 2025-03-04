using System;
using System.Collections.Generic;

namespace Runner.Core
{
    [Serializable]
    public class PlayerSaveModel
    {
        public int PlayerCurrency;
        public int RunnerModelID;
        public int RunAnimationID;
        public int KickAnimationID;
        public int DanceAnimationID;
        public List<int> BuyedItemsIDs = new List<int>();

        public float SoundVolume;
        public string LanguageID;
        public bool IsModelInited;
        public float SpeedModifier;
        public int WinStreak;
        public int MaxWinStreak;

        public PlayerSaveModel(PlayerGameModel gameModel)
        {
            PlayerCurrency = gameModel.PlayerCurrency.Value;
            RunnerModelID = gameModel.RunnerModelID;
            RunAnimationID = gameModel.RunAnimationID;
            KickAnimationID = gameModel.KickAnimationID;
            DanceAnimationID = gameModel.DanceAnimationID;
            BuyedItemsIDs = gameModel.BuyedItemsIDs;
            IsModelInited = gameModel.IsModelInited;
            SoundVolume = gameModel.SoundVolume;
            LanguageID = gameModel.LanguageID;
            BuyedItemsIDs = gameModel.BuyedItemsIDs;
            SpeedModifier = gameModel.SpeedModifier;
            WinStreak = gameModel.WinStreak;
            MaxWinStreak = gameModel.MaxWinStreak;
        }

        public void ClearModel()
        {
            BuyedItemsIDs.Clear();
        }
    }
}
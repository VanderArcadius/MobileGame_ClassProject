﻿// Global switches for the overall game to use...

namespace TRP.Models
{
    public static class GameGlobals
    {
        // Available slots for penguin party
        public const int availCharactersSlots = 6;

        public const int MaxNumberPartyPlayers = 6;

        // Available slots for monster party
        public const int availMonstersSlots = 6;

        // Holds the random value for the sytem
        private static int _ForcedRandomValue = 1;

        // What number should return for random numbers (1 is good choice...)
        public static int ForcedRandomValue
        {
            get => _ForcedRandomValue;
        }

        // Available stat points for character create and edit
        public const int availStatPoints = 10;

        // Turn on to force Rolls to be non random
        public static bool ForceRollsToNotRandom = false;

        // What number to use for ToHit values (1,2, 19, 20)
        public static int ForceToHitValue = 20;

        // Forces Monsters to hit with a set value
        // Zero, because don't want to add it in unless it is used...
        public static int ForceMonsterDamangeBonusValue = 0;

        // Forces Characters to hit with a set value
        // Zero, because don't want to add it in unless it is used...
        public static int ForceCharacterDamangeBonusValue = 0;

        // Allow Random Items when monsters die...
        public static bool AllowMonsterDropItems = true;

        // Turn Off Random Number Generation, and use the passed in values.
        public static void SetForcedRandomNumbersValueAndToHit(int value, int hit)
        {
            SetForcedRandomNumbersValue(value);
            ForceToHitValue = hit;
        }

        // Turn Off Random Number Generation, and use the passed in values.
        public static void SetForcedRandomNumbersValue(int value)
        {
            EnableRandomValues();
            _ForcedRandomValue = value;
        }

        // Set the forced hit value
        public static void SetForcedHitValue(int hit)
        {
            ForceToHitValue = hit;
        }

        // Flip the Random State (false to true etc...)
        // Call this after setting, to restore...
        public static void ToggleRandomState()
        {
            ForceRollsToNotRandom = !ForceRollsToNotRandom;
        }

        // Turn Random State Off
        public static void DisableRandomValues()
        {
            ForceRollsToNotRandom = false;
        }

        // Turn Random State On
        public static void EnableRandomValues()
        {
            ForceRollsToNotRandom = true;
        }

        // Debug Settings
        public static bool EnableCriticalMissProblems = false;
        public static bool EnableCriticalHitDamage = false;
        public static bool EnableMonsterStolenItem = false;
        public static double MonsterStealsChance = 0;
        public static bool EnableRevivalOnce = false;
        public static bool EnableReverseOrder = false;
        public static double ReverseChance = 0;
        public static bool EnableMulligan = false;
        public static double MulliganChance = 0;
        public static bool EnableRebound = false;
        public static double ReboundChance = 0;
        public static bool EnableZombies = false;
        public static double ZombieChance = 0;

        // Set switch for allowing monsters to steal items 
        public static void SetMonstersToStealItems(bool val)
        {
            EnableMonsterStolenItem = val;
        }

        // Set switch for allowing monsters to steal items 
        public static void SetMiracleMaxUsage(bool val)
        {
            EnableRevivalOnce = val;
        }

        // Set switch for allowing reverse order
        public static void SetReverseOrder(bool val)
        {
            EnableReverseOrder = val;
        }

        // Set switch for allowing mulligan e.g. second chance to hit after a miss
        public static void SetMulligan(bool val)
        {
            EnableMulligan = val;
        }

        // Set switch for allowing mulligan e.g. second chance to hit after a miss
        public static void SetRebound(bool val)
        {
            EnableRebound = val;
        }

        //Set switch for allowing zombies
        public static void SetZombies(bool val)
        {
            EnableZombies = val;
        }

    }
}

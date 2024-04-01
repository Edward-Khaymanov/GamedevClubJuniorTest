using System;
using UnityEngine;

namespace ClubTest
{
    public class CONSTANTS
    {
        public const float BULLET_LIFE_TIME_IN_SECONDS = 5f;
        public const string PLAYER_SAVE_FILE_NAME = "player.json";
        public const string APP_FIRST_LAUCH_FILE_NAME = "IsFirstLauch";

        public const int UNIT_DETECTOR_MAX_UNITS = 20;
        public static readonly TimeSpan UNIT_DETECTION_INTERVAL = TimeSpan.FromMilliseconds(200);

        public static readonly LayerMask EnemyMask = LayerMask.GetMask("Enemy");
        public static readonly LayerMask PlayerMask = LayerMask.GetMask("Player");

        public const string ITEMS_PATH = "Items/";
        public const string ENEMY_TEMPLATES_PATH = "EnemyTemplates";

        public const int SCENE_GAME_INDEX = 1;
    }
}
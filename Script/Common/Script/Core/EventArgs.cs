using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameBase
{
    /// <summary>
    /// 事件定义
    /// </summary>
    public enum EVENT_TYPE
    {
        EVENT_NONE = 0,

        EVENT_SYSTEM_START,

        EVENT_LOGIC_LOAD,
        EVENT_LOGIC_LOAD_FINISH,

        EVENT_LOGIC_TIMER_CONTROLLER_TIME_OUT,

        EVENT_LOGIC_LOGIC_START,
        EVENT_LOGIC_FIGHT_START,
        EVENT_LOGIC_FIGHT_FINISH,
        EVENT_LOGIC_UPDATE_STRENGTH,
        EVENT_LOGIC_UPDATE_DIAMOND,
        EVENT_LOGIC_UPDATE_GOLD,

        EVENT_LOGIC_SELECT_SCENE,
        EVENT_LOGIC_DISABLE_SCREEN_OPERATE,
        EVENT_LOGIC_ENABLE_SCREEN_OPERATE,
        EVENT_LOGIC_CAPTURE_RESULT,

        EVENT_UI_CREATE,

        EVENT_UI_SHOWED,
        EVENT_UI_HIDED,
        EVENT_UI_BACK_MAIN,

        EVENT_UI_FIGHT_STAGE_ATTRACT_ROUND_START,
        EVENT_UI_FIGHT_STAGE_ATTRACT_ROUND_RESULT,
        EVENT_UI_FIGHT_STAGE_ATTRACT_FINISH,
        EVENT_UI_FIGHT_STAGE_SCORE_FINISH,
        EVENT_UI_FIGHT_STAGE_ATTRACT_MENBER_SELECT,

        EVENT_UI_SHOW_MESSAGEBOX,

        EVENT_LEVEL_WIN,
        EVENT_LEVEL_FAIL,
        EVENT_LEVEL_START,

        //specil for timer
        EVENT_TIMER_START = 1000,
        EVENT_TIMER_END = 2000,

        //fight
        EVENT_FIGHT_START = 2001,

        EVENT_MOTION_MOVE,
        EVENT_MOTION_HIT,
        EVENT_MOTION_FLY,
        EVENT_MOTION_RISE,
        EVENT_MOTION_RISE_FINISH,
        EVENT_MOTION_FINISH,

        EVENT_FIGHT_ATTR_DAMAGE,

        EVENT_FIGHT_END = 3001,
    }

   
 
}

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



/// <summary>
/// 事件定义
/// </summary>
public enum EVENT_TYPE
{
    EVENT_NONE = 0,

    EVENT_LOGIC_ROLE_LEVEL_UP,
    EVENT_LOGIC_PASS_STAGE,
    EVENT_LOGIC_EXIT_STAGE,
    EVENT_LOGIC_KILL_MONSTER,
    EVENT_LOGIC_GEM_LEVEL_UP,
    EVENT_LOGIC_GEM_ACT_SUIT,
    EVENT_LOGIC_EQUIP_GET,
    EVENT_LOGIC_EQUIP_REFRESH,
    EVENT_LOGIC_EQUIP_DESTORY,
    EVENT_LOGIC_EQUIP_STORE,
    EVENT_LOGIC_GAMBLING,
    EVENT_LOGIC_WATCH_MOVIE,
    EVENT_LOGIC_SYSTEMSETTING_CHANGE,

    EVENT_LOGIC_SOMEONE_SUPER_ARMOR,
}

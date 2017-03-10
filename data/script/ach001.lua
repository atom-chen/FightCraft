--通关星级成就
x100001_g_scriptId = 100001

--事件过滤
function x100001_OnEventFilter(uuid, achId, achKey)
  	return 1
end

--成就完成进度
function x100001_OnAchievementProgress(uuid, reachNum)

  	return LuaGetDungeonAllStar(uuid)
  	
end
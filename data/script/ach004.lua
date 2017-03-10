--战斗力
x100004_g_scriptId = 100004

--事件过滤
function x100004_OnEventFilter(uuid, achEventId, fightvalue)

	if fightvalue ~= nil and tonumber(fightvalue) <= LuaGetLineupFightValue(uuid) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100004_OnAchievementProgress(uuid, reachNum)
  	return reachNum
end


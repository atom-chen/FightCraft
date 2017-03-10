--五级宝石
x100008_g_scriptId = 100008

--事件过滤
function x100008_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllGemsByLevel(uuid, 5) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100008_OnAchievementProgress(uuid, reachNum)
  	return 1
end


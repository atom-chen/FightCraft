--七级宝石
x100009_g_scriptId = 100009

--事件过滤
function x100009_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllGemsByLevel(uuid, 7) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100009_OnAchievementProgress(uuid, reachNum)
  	return 1
end


--九级宝石
x100010_g_scriptId = 100010

--事件过滤
function x100010_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllGemsByLevel(uuid, 9) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100010_OnAchievementProgress(uuid, reachNum)
  	return 1
end


--三级宝石
x100007_g_scriptId = 100007

--事件过滤
function x100007_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllGemsByLevel(uuid, 3) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100007_OnAchievementProgress(uuid, reachNum)
  	return 1
end



x100020_g_scriptId = 100020
x100020_g_stageId = 2

--事件过滤
function x100020_OnEventFilter(param1, param2, param3, param4, param5)

	if param4 ~= nil and tonumber(param4) == x100020_g_stageId then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100020_OnAchievementProgress(uuid, reachNum)
  	return 1
end


--普通关卡和精英关卡成就
x100002_g_scriptId = 100002

--事件过滤
function x100002_OnEventFilter(param1, param2, param3, stageId, param5, param6, param7, param8, param9)

	if stageId ~= nil and param9 ~= nil and tonumber(stageId) == tonumber(param9) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100002_OnAchievementProgress(uuid, reachNum)
  	return reachNum
end


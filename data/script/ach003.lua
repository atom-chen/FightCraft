--vip等级成就
x100003_g_scriptId = 100003


--事件过滤
function x100003_OnEventFilter(uuid, achEventId, vipLevel)

	if vipLevel ~= nil and tonumber(vipLevel) <= LuaGetVipLevel(uuid) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100003_OnAchievementProgress(uuid, reachNum)
  	return reachNum
end


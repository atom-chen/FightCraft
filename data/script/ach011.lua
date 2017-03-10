--绿色装备数量
x100011_g_scriptId = 100011


--事件过滤
function x100011_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllEquipsByQuality(uuid, 1) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100011_OnAchievementProgress(uuid, reachNum)
  	return 1
end


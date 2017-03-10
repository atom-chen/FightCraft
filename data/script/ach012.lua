--蓝色装备数量
x100012_g_scriptId = 100012

--事件过滤
function x100012_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllEquipsByQuality(uuid, 2) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100012_OnAchievementProgress(uuid, reachNum)
  	return 1
end


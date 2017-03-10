--紫色装备数量
x100013_g_scriptId = 100013


--事件过滤
function x100013_OnEventFilter(uuid, achEventId, count)

	if count ~= nil and tonumber(count) <= LuaGetAllEquipsByQuality(uuid, 3) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100013_OnAchievementProgress(uuid, reachNum)
  	return 1
end


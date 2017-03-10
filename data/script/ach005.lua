--装备强化等级
x100005_g_scriptId = 100005

--事件过滤
function x100005_OnEventFilter(uuid, achEventId, enchanceLevel)

	if enchanceLevel ~= nil and 5 <= LuaGetAllEquipsByEnchanceLevel(uuid, enchanceLevel) then
  		return 1
  	else
  		return 0
  	end
  	
end

--成就完成进度
function x100005_OnAchievementProgress(uuid, reachNum)
  	return 1
end


--金币消耗事件 计数器
x200002_g_scriptId = 200002


function x200002_OnCounter(uuid, eventId, count)
	
	--参考AchievementKeyTable.csv
	local achKey = 50

	if count > 0 then
		LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + count)
	end
	
	local strMsg = string.format("x200002_OnCounter uuid=%d, eventId=%d, count=%d", 
					uuid, eventId, count)
	LuaWriteLog(strMsg)
	
  	return
  	
end


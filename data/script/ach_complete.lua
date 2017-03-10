--完成成就 计数器
x200009_g_scriptId = 200009


function x200009_OnCounter(uuid, eventId)

	--参考AchievementKeyTable.csv
	local achKey = 66
	LuaSetAchievementValue(uuid, achKey, LuaGetAchievementValue(uuid, achKey) + 1)
	
	local strMsg = string.format("x200009_OnCounter uuid=%d, eventId=%d", 
					uuid, eventId)
	LuaWriteLog(strMsg)
	
  	return
  	
end


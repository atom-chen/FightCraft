--获得武将 计数器
x200006_g_scriptId = 200006


function x200006_OnCounter(uuid, eventId, petLevel)

	--参考AchievementKeyTable.csv
	local achKey = 55
	LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 1))
	
	local strMsg = string.format("x200006_OnCounter uuid=%d, eventId=%d, petLevel=%d", 
					uuid, eventId, petLevel)
	LuaWriteLog(strMsg)
	
  	return
  	
end


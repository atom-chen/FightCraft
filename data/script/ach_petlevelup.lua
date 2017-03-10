--武将升级 计数器
x200007_g_scriptId = 200007


function x200007_OnCounter(uuid, eventId)

	--参考AchievementKeyTable.csv
	local achKey = 56
    LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 10))
	
	achKey = 57
	LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 15))
	
	achKey = 58
	LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 20))
	
	achKey = 59
	LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 25))
	
	achKey = 60
	LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 30))
	
	achKey = 61
	LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 35))
	
	achKey = 62
	LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 40))
	
	achKey = 63
	LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 45))
	
	achKey = 64
	LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 50))
	
	achKey = 65
	LuaSetAchievementValue(uuid, achKey, LuaGetPetMoreThanLevelCount(uuid, 60))
	
	local strMsg = string.format("x200007_OnCounter uuid=%d, eventId=%d", 
					uuid, eventId)
	LuaWriteLog(strMsg)
	
  	return
  	
end

